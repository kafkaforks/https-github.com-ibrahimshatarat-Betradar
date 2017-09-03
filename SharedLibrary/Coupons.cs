using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using RestSharp;

namespace SharedLibrary
{
    public class Coupons : Core
    {

        public void CalculatedCouponSystemsSelected(string json)
        {
            try
            {
                dynamic dynJson = JsonConvert.DeserializeObject(json);
                foreach (var item in dynJson)
                {
                    Logg.logger.Trace("{0} {1} {2} {3}\n", item.id, item.displayName,
                        item.slug, item.imageUrl);
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public string CalculateSystemCombination(long p_coupon_id, int p_loop_length)
        {

            var command = new NpgsqlCommand( Globals.DB_Functions.CpCalculateSystemCombination.ToDescription().ToString());
            try
            {
                command.Parameters.AddWithValue("p_coupon_id", NpgsqlDbType.Bigint, p_coupon_id);
                command.Parameters.AddWithValue("p_loop_length", NpgsqlDbType.Integer, p_loop_length);

                var result = "-1";
                var ds =  select(command);
                var value = ds.Tables[0].Rows[0][0];
                if (ds.Tables.Count > 0)
                {
                    return value.ToString();
                }
                else
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex);
                return "-1";
            }

        }

        public  DataSet GetUnfinalizedCouponsOdds()
        {
            try
            {
                var command = new NpgsqlCommand( Globals.DB_Functions.SelectCouponOddsTemp.ToDescription().ToString());
                var data =  select(command);
                return data;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        private  DataSet GetUnfinalizedCouponsOddsTest()
        {
            try
            {
                var command = new NpgsqlCommand("select_coupon_odds_temp_test");
                var data =  select(command);
                return data;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public  List<string> GetDBQueueTest()
        {
            var db_data = new List<string>();
            try
            {
                var ds =  GetUnfinalizedCouponsOddsTest();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    db_data = ds.Tables[0].AsEnumerable()
                                               .Select(r => r.Field<string>("select_coupon_odds_temp_test"))
                                               .ToList();

                }
                return db_data;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public  List<string> GetDBQueue()
        {
            var db_data = new List<string>();
            try
            {
                var ds =  GetUnfinalizedCouponsOdds();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        db_data.Add(ds.Tables[0].Rows[i][0].ToString());
                    }

                }
                return db_data;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public bool CouponMove(long couponId, int action)
        {
            if (action == 3)
            {
                MoveCouponAll(5, couponId, null);
                return true;
            }
            else if (action == 2)
            {
                MoveCouponAll(6, couponId, null);
                return true;
            }
            return false;
        }

        public bool CouponOddsMove(long couponId)
        {

            string rollbackArch = "BEGIN;" +
                                  "SAVEPOINT HERE;" +
                                  "INSERT INTO cp_coupon_odds_arch (id, coupon_id, feed_type, match_id, odd_active, odd_changed, odd_finalised, odd_id, odd_name, odd_odd, odd_outcome, odd_outcome_id, odd_probability, odd_special, odd_type, odd_type_id, odd_player_id, odd_team_id, mid_otid_ocid_sid) " +
                                  "SELECT id, coupon_id, feed_type, match_id, odd_active, odd_changed, odd_finalised, odd_id, odd_name, odd_odd, odd_outcome, odd_outcome_id, odd_probability, odd_special, odd_type, odd_type_id, odd_player_id, odd_team_id, mid_otid_ocid_sid FROM cp_coupon_odds WHERE coupon_id = " + couponId + "; " +
                                  "DELETE FROM cp_coupon_odds WHERE coupon_id = " + couponId + "; ";

            NpgsqlCommand objCommand = new NpgsqlCommand(rollbackArch);
            objCommand.CommandText = rollbackArch;

            try
            {
                insertNonProc(objCommand);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                string rollbackArchOperation = "ROLLBACK TO SAVEPOINT HERE; END;";
                objCommand.CommandText += rollbackArchOperation;
                try
                {
                    insertNonProc(objCommand);
                }
                catch (Exception exc)
                {
                    Logg.logger.Fatal(exc.Message);
                }
                return false;
            }

            String rollbackEnd = " END;";
            objCommand.CommandText += rollbackEnd;
            try
            {
                insertNonProc(objCommand);
            }
            catch (Exception e)
            {
                Logg.logger.Fatal(e.Message);
            }
            return true;

        }

        public bool UpdateCouponSendApi(long coupon_id)
        {
            try
            {
                var command = new NpgsqlCommand( Globals.DB_Functions.UpdateSentCounpons.ToDescription().ToString());
                command.Parameters.AddWithValue("p_coupon_id", NpgsqlDbType.Bigint, coupon_id);
                var ret =  insert(command);
                if (ret == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return false;
            }
        }

        public string CouponOddsCalculate(long coupon_id)
        {
            try
            {
                var command = new NpgsqlCommand( Globals.DB_Functions.CouponWinningOdds.ToDescription().ToString());
                command.Parameters.AddWithValue("p_coupon_id", NpgsqlDbType.Bigint, coupon_id);
                var ret =  select(command);
                string value = "";
                if (ret.Tables.Count > 0 && ret.Tables[0].Rows.Count > 0)
                {
                    value = ret.Tables[0].Rows[0][0].ToString();
                }
                return value;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public  void GetDummyData()
        {
            var command = new NpgsqlCommand( Globals.DB_Functions.GetFullFinalisedCoupons.ToDescription().ToString());
            var data =  select(command);
            var dt = new DataTable();
        }
        public DataSet getAllUNfinalisedMid()
        {
            var dyMerchants = new Merchants();
            var command = new NpgsqlCommand(Globals.DB_Functions.GetUnfinalisedMid.ToDescription().ToString());
            try
            {
                var ds = select(command);

                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }

        }
        public  void GetAllFinalizedCoupons()
        {
            try
            {
                var command = new NpgsqlCommand( Globals.DB_Functions.GetFullFinalisedCoupons.ToDescription().ToString());
                // var data = select(command);
                //TODO send to seamless
                //sendAllToApi(data);
                Globals.timerOnOff = true;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
            finally
            {

            }
        }

        public void GetFinalisedOdds()
        {
            try
            {
                var command = new NpgsqlCommand( Globals.DB_Functions.GetFullFinalisedCoupons.ToDescription().ToString());
                var data = select(command);
                //TODO send to seamless
                //sendAllToApi(data);
                Globals.timerOnOff = true;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
            finally
            {

            }
        }

        public void sendAllToApi(DataSet data)
        {
            var dt = new DataTable();
            try
            {
                if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                {
                    dt = data.Tables[0];
                    if (dt.Rows.Count > 0)
                    {

                        Parallel.For(0, dt.Rows.Count, index =>
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[index][1].ToString()))
                            {
                                var id = long.Parse(dt.Rows[index][0].ToString());
                                var merchant_id = long.Parse(dt.Rows[index][2].ToString());
                                var toSend = dt.Rows[index][1].ToString();
                                //CouponOddsCalculate(id);
                                if (toSend != "{There is one or more odds not finalized yet!}")
                                {
                                    //sendToMerchantServices(toSend, id, merchant_id);
                                    //Task.Factory.StartNew(() => sendToMerchantServices(toSend, id, merchant_id));

                                }
                            }
                        }
                            );

                        //for(int i = 0; i < dt.Rows.Count; i++)
                        //{
                        //    var id = long.Parse(dt.Rows[i][0].ToString());
                        //    var merchant_id = long.Parse(dt.Rows[i][1].ToString());
                        //    var toSend = CouponOddsCalculate(id);
                        //    if (toSend != "{There is one or more odds not finalized yet!}")
                        //    {
                        //        Task.Factory.StartNew(() => sendToMerchantServices(toSend, id, merchant_id));

                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);

            }
        }

        public void MatchFinalize(string MatchPattern)
        {
            try
            {
                var command = new NpgsqlCommand( Globals.DB_Functions.FinalizeOdd.ToDescription().ToString());
                command.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, MatchPattern);
                 insert(command);
            }
            catch (Exception ex)
            {
                //BetClearingQueue.StringQueue.Enqueue(element_string)
                Logg.logger.Fatal(ex.Message);
            }
        }

        public void BetCancelDB(string MatchPattern)
        {
            try
            {
                var command = new NpgsqlCommand( Globals.DB_Functions.CancelOdd.ToDescription().ToString());
                command.Parameters.AddWithValue("p_mid_otid_ocid_sid", NpgsqlDbType.Text, MatchPattern);
                 insert(command);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public DataSet GetFinalizedCouponsIds()
        {
            try
            {
                var command = new NpgsqlCommand( Globals.DB_Functions.FinalizeOddCoupons.ToDescription().ToString());
                return  select(command);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public DataSet GetCouponSystems(long Coupon_Id)
        {
            try
            {
                var command = new NpgsqlCommand( Globals.DB_Functions.FinalizeOddCoupons.ToDescription().ToString());
                command.Parameters.AddWithValue("p_coupon_id", NpgsqlDbType.Bigint, Coupon_Id);
                return  select(command);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public  void sendToMerchantServices(string rawModeData, long id, long merchant_id)
        {
            IRestResponse response = null;

            try
            {
                var result = (from myRow in Globals.MerchantsDs.Tables[0].AsEnumerable()
                              where myRow.Field<long>("vendor_id") == merchant_id
                              select myRow).ToList();

                var client = new RestClient(config.AppSettings.Get("Seamless_Services") + result[0]["prefix"]);
                var request = new RestRequest(Method.POST);
                request.Timeout = 3000;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", rawModeData, ParameterType.RequestBody);
                response =
                    
                        client.Execute(request);
                // response = client.Execute(request);
                if (!string.IsNullOrEmpty(response.Content))
                {
                    if (new JavaScriptSerializer().Deserialize<SeamlessResponse>(response.Content).error_codes[0] == 0)
                    {

                         UpdateSentCouponsOnce(id, 2, rawModeData, response.Content);
#if DEBUG
                        Console.WriteLine(":::::::::::::::::::: END  ::::::::::::::::::::");
#endif
                    }
                    else
                    {
                         insertSeamlessCount(id, rawModeData, response.Content);
#if DEBUG
                        Console.WriteLine(":::::::::::::::::::: NOT END  ::::::::::::::::::::");
#endif
                    }
                }
                else
                {
                     insertSeamlessCount(id, rawModeData, response.Content);
#if DEBUG
                    Console.WriteLine(":::::::::::::::::::: EMPTY  ::::::::::::::::::::");
#endif
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                 insertSeamlessCount(id, rawModeData, response.Content);
            }
        }

        public void sendToMerchantServices_(string rawModeData, long id, long merchant_id)
        {
            IRestResponse response = null;

            try
            {
                var merc = new Merchants();
                merc =  selectDyMerchants(merchant_id, "");
                var client = new RestClient(config.AppSettings.Get("Seamless_Services") + merc.prefix);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", rawModeData, ParameterType.RequestBody);
                response =  client.Execute(request);
                if (response.Content != null)
                {
                    if (new JavaScriptSerializer().Deserialize<SeamlessResponse>(response.Content).error_codes[0] == 0)
                    {

                        UpdateSentCouponsOnce(id, 2, rawModeData, response.Content);
                    }
                    else
                    {

                    }
                }
                //insertSeamlessCount(id, rawModeData, response.Content);
                Console.WriteLine(":::::::::::::::::::: ENDDDDDDDD  ::::::::::::::::::::");
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                insertSeamlessCount(id, rawModeData, response.Content);
            }
        }

        public long MoveCouponAll(long p_table_id, long? p_coupon_id, DateTime? p_lastupdate)
        {
            var command = new NpgsqlCommand("coupon_move");
            try
            {
                command.Parameters.AddWithValue("p_table_id", NpgsqlDbType.Bigint, p_table_id);
                if (p_coupon_id != null)
                {
                    command.Parameters.AddWithValue("p_coupon_id", NpgsqlDbType.Bigint, p_coupon_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_coupon_id", NpgsqlDbType.Bigint, DBNull.Value);

                }
                if (p_lastupdate != null)
                {
                    command.Parameters.AddWithValue("p_lastupdate", NpgsqlDbType.Timestamp, p_lastupdate);
                }
                else
                {
                    command.Parameters.AddWithValue("p_lastupdate", NpgsqlDbType.Timestamp, DBNull.Value);
                }

                return  insert(command);

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }

        public void UpdateSentCouponsOnce(long counpon_id, int action, string request, string response)
        {
            var command = new NpgsqlCommand( Globals.DB_Functions.UpdateSentCouponsOnce.ToDescription().ToString());
            try
            {
                command.Parameters.AddWithValue("p_coupon_id", NpgsqlDbType.Bigint, counpon_id);
                command.Parameters.AddWithValue("p_action", NpgsqlDbType.Integer, action);
                command.Parameters.AddWithValue("p_request", NpgsqlDbType.Text, request);
                command.Parameters.AddWithValue("p_response", NpgsqlDbType.Text, response);

                insertNonReturn(command);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public long insertSeamlessCount(long counpon_id, string request, string response)
        {
            var command = new NpgsqlCommand( Globals.DB_Functions.UpdateSendSeamless.ToDescription().ToString());
            try
            {
                command.Parameters.AddWithValue("p_coupon_id", NpgsqlDbType.Bigint, counpon_id);
                command.Parameters.AddWithValue("p_request", NpgsqlDbType.Text, request);
                command.Parameters.AddWithValue("p_response", NpgsqlDbType.Text, response);

                return  update(command);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return -1;
            }
        }
        public void WorkAlive()
        {
            var command = new NpgsqlCommand( Globals.DB_Functions.UpdateLiveMatchesShow.ToDescription().ToString());
            try
            {
                select(command);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public DataSet selectAllMerchants(long? p_merchant_id)
        {
            var dyMerchants = new Merchants();
            var command = new NpgsqlCommand( Globals.DB_Functions.SelectMerchants.ToDescription().ToString());
            try
            {

                if (p_merchant_id != null)
                {
                    command.Parameters.AddWithValue("p_merchant_id", NpgsqlDbType.Bigint, p_merchant_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_merchant_id", NpgsqlDbType.Bigint, DBNull.Value);
                }


                var ds =  select(command);

                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }

        }

        public  Merchants selectDyMerchants(long p_merchant_id, string p_username)
        {
            var dyMerchants = new Merchants();
            var command = new NpgsqlCommand( Globals.DB_Functions.selectDyMerchants.ToDescription().ToString());
            try
            {

                if (!string.IsNullOrEmpty(p_username))
                {
                    command.Parameters.AddWithValue("p_username", NpgsqlDbType.Text, p_username);
                }
                else
                {
                    command.Parameters.AddWithValue("p_username", NpgsqlDbType.Text, DBNull.Value);
                }

                if (p_merchant_id != null)
                {
                    command.Parameters.AddWithValue("p_merchant_id", NpgsqlDbType.Bigint, p_merchant_id);
                }
                else
                {
                    command.Parameters.AddWithValue("p_merchant_id", NpgsqlDbType.Bigint, DBNull.Value);
                }


                var ds =  select(command);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dyMerchants.id = Convert.ToInt64((ds.Tables[0].Rows[0]["id"]));
                        dyMerchants.username = Convert.ToString((ds.Tables[0].Rows[0]["username"]));
                        dyMerchants.merchant_name = Convert.ToString((ds.Tables[0].Rows[0]["merchant_name"]));
                        dyMerchants.prefix = Convert.ToString((ds.Tables[0].Rows[0]["prefix"]));
                        dyMerchants.vendor_id = Convert.ToInt32((ds.Tables[0].Rows[0]["vendor_id"]));
                        dyMerchants.domain_m = Convert.ToString((ds.Tables[0].Rows[0]["domain_m"]));
                        dyMerchants.last_update = Convert.ToDateTime((ds.Tables[0].Rows[0]["last_update"]));
                        dyMerchants.profit_margin = Convert.ToDecimal((ds.Tables[0].Rows[0]["profit_margin"]));
                        dyMerchants.seamlessurl = Convert.ToString((ds.Tables[0].Rows[0]["seamlessurl"]));
                        dyMerchants.skin = Convert.ToString((ds.Tables[0].Rows[0]["skin"]));
                    }
                }

                if (dyMerchants != null)
                {
                    return dyMerchants;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }

        }
     








        public void insertNonReturn(NpgsqlCommand objCommand)
        {
            long errorNumber = -1;
            long result = -20;
            object one;
            objCommand.CommandType = CommandType.StoredProcedure;
            try
            {
                objCommand.Connection = connection();
                //objCommand.CommandTimeout = 5;
                 objCommand.Connection.Open();
                one =  objCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                StackTrace trace = new StackTrace(true);
                var frame = trace.GetFrame(1);
                var altMessage = "  Error#: " + errorNumber.ToString() + "  METHOD: " + frame.GetMethod().Name + "  LINE:  " + frame.GetFileLineNumber();
                Logg.logger.Fatal(ex.Message + altMessage);
                // Task.Factory.StartNew(() => Globals.Queue_Errors.Enqueue(objCommand));
            }
            finally
            {
                objCommand.Connection.Close();
            }
        }

        
    }
}
