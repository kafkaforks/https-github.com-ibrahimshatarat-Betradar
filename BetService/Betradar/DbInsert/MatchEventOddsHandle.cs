using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BetService.Classes.DbInsert;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using SharedLibrary.RPC;
using Sportradar.SDK.FeedProviders.Lcoo;
using Sportradar.SDK.FeedProviders.LiveOdds.LiveOdds;

namespace BetService.Classes.DbInsert
{
    public class MatchEventOddsHandle : Core
    {

        public MatchEventOddsHandle(MatchEventOdds args)
        {
            MatchEventOdds_Queue_WatchQueueMatches(args);
        }
        public object GetPropertyValue(object obj, string propertyName)
        {
            var _propertyNames = propertyName.Split('.');

            for (var i = 0; i < _propertyNames.Length; i++)
            {
                if (obj != null)
                {
                    var _propertyInfo = obj.GetType().GetProperty(_propertyNames[i]);
                    if (_propertyInfo != null)
                        obj = _propertyInfo.GetValue(obj);
                    else
                        obj = null;
                }
            }

            return obj;
        }
        public void MatchEventOdds_Queue_WatchQueueMatches(MatchEventOdds queueElement)
        {
            //TODO: uncomment ;
            //var d = new JavaScriptSerializer().Serialize(queueElement);
            //var write = new StreamWriter(@"C:\Users\heythamwork\Google Drive\ecoPayz\betradar-History\2_14_2017\Live\Betradar\Betradar\bin\Debug\logs\"+queueElement.MatchEntity.MatchId.ToString());
            //write.Write(d);

            var common = new Common();
            var entity = queueElement.MatchEntity;
            var queue = new Queue<Globals.Rollback>();
            var queueMatch = new Queue<Globals.Rollback>();
            var match = queueElement.MatchEntity;
            //var client = new Client();
            //var proxy = client.Serverproxy();
            try
            {
                if (match.BetResults != null)
                {
                    //Parallel.ForEach(match.BetResults, bet_result =>
                    //{
                   
                        foreach (var bet_result in match.BetResults)
                        {
                            var bet = new BetClearQueueElement();
                            bet.MatchId = match.MatchId;
                            bet.OddsId = bet_result.OddsType;
                            if (bet_result.OutcomeId != null)
                            {
                                bet.OutcomeId = int.Parse(bet_result.OutcomeId);
                            }
                            bet.SpecialBetValue = bet_result.SpecialBetValue;
                            bet.PlayerId = bet_result.PlayerId;
                            bet.TeamId = bet_result.TeamId;
                            var mid = EncodeUnifiedBetClearQueueElement(bet);
                            common.insertCpLcooBetclearOdds(bet_result, match.MatchId, mid);
                            Task.Factory.StartNew(() =>
                            {
                                var coupon = new Coupons();
                                coupon.MatchFinalize(mid);
                            });
                        }
                        // Task.Factory.StartNew(() => sendToRpc(mid));
                        //sendToRpc(mid);
                    //});

                }
                common.insertMatch(match, 1);

                //Task.Factory.StartNew(() => common.insertMatchDataAllDetails((MatchHeader)queueElement.MatchEntity., null));
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
            try
            {
                // Here we add the odds of the match

                int[] visible_odd_types = new int[] { 10, 46, 60, 42,20,225,52 };
                if (match.Odds != null)
                {

                    foreach (var Odds in match.Odds)
                    {
                        foreach (var odd in Odds.Odds)
                        {
                            if (odd.Value != "OFF")
                            {
                                // New Pre match Odds Insert to new Table.

                                var probability = "";
                                if (match.Probabilities != null)
                                {

                                    foreach (var prob in match.Probabilities)
                                    {
                                        if (prob.OddsType == Odds.OddsType)
                                        {
                                            foreach (var odd_prob in prob.OddsProbabilities)
                                            {
                                                if (odd_prob.Outcome == odd.OutCome)
                                                {
                                                    if (odd_prob.Value != null)
                                                    {
                                                        probability = odd_prob.Value;
                                                    }
                                                    else
                                                    {
                                                        probability = String.Empty;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (visible_odd_types.Contains(Odds.OddsType))
                                {
                                    var insertNewMatchOdds = common.insertCpLcooOdds(odd, match.MatchId, Odds.OddsType,
                                        "", "", true, probability);
                                }
                                else
                                {
                                    var insertNewMatchOdds = common.insertCpLcooOdds(odd, match.MatchId, Odds.OddsType,
                                        "", "", false, probability);
                                }

                            }
                            else
                            {
                                common.updateCpLcooOdds(match.MatchId, Odds.OddsType);
                            }

                            #region old odds
                            //var commandOdds = new NpgsqlCommand(Globals.DB_Functions.InsertCouponOdds.ToDescription());
                            //commandOdds.Parameters.AddWithValue("p_match_id", NpgsqlDbType.Bigint, match.MatchId);
                            //if (Odds.OddsType != null)
                            //{
                            //    commandOdds.Parameters.AddWithValue("p_odd_type_id", NpgsqlDbType.Bigint, Odds.OddsType);
                            //}
                            //else
                            //{
                            //    commandOdds.Parameters.AddWithValue("p_odd_type_id", NpgsqlDbType.Bigint, DBNull.Value);
                            //}

                            //commandOdds.Parameters.AddWithValue("p_odd_type_name", NpgsqlDbType.Text, DBNull.Value);
                            //commandOdds.Parameters.AddWithValue("p_odd_type_name_tr", NpgsqlDbType.Text, DBNull.Value);
                            //if (odd.Id != null)
                            //{
                            //    commandOdds.Parameters.AddWithValue("p_odd_id", NpgsqlDbType.Bigint, odd.Id);
                            //}
                            //else
                            //{
                            //    commandOdds.Parameters.AddWithValue("p_odd_id", NpgsqlDbType.Bigint, DBNull.Value);
                            //}

                            //commandOdds.Parameters.AddWithValue("p_odd_name", NpgsqlDbType.Text, DBNull.Value);
                            //if (visible_odd_types.Contains(Odds.OddsType))
                            //{
                            //    commandOdds.Parameters.AddWithValue("p_odd_visible", NpgsqlDbType.Boolean, true);
                            //}
                            //else
                            //{
                            //    commandOdds.Parameters.AddWithValue("p_odd_visible", NpgsqlDbType.Boolean,false);
                            //}

                            //if (odd.SpecialBetValue != null)
                            //{
                            //    commandOdds.Parameters.AddWithValue("p_odd_special", NpgsqlDbType.Text, odd.SpecialBetValue);
                            //}
                            //else
                            //{
                            //    commandOdds.Parameters.AddWithValue("p_odd_special", NpgsqlDbType.Text, DBNull.Value);
                            //}
                            //if (odd.Value != null)
                            //{
                            //    commandOdds.Parameters.AddWithValue("p_odd_odd", NpgsqlDbType.Text, odd.Value);
                            //}
                            //else
                            //{
                            //    commandOdds.Parameters.AddWithValue("p_odd_odd", NpgsqlDbType.Text, DBNull.Value);
                            //}
                            //if (match.Probabilities != null)
                            //{
                            //    var probability = "";
                            //    foreach (var prob in match.Probabilities)
                            //    {
                            //        if (prob.OddsType == Odds.OddsType)
                            //        {
                            //            foreach (var odd_prob in prob.OddsProbabilities)
                            //            {
                            //                if (odd_prob.Outcome == odd.OutCome)
                            //                {
                            //                    if (odd_prob.Value != null)
                            //                    {
                            //                        probability = odd_prob.Value;
                            //                    }
                            //                    else
                            //                    {
                            //                        probability = String.Empty;
                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //    commandOdds.Parameters.AddWithValue("p_odd_probability", NpgsqlDbType.Text, probability);
                            //   // commandOdds.Parameters.AddWithValue("p_odd_probability", NpgsqlDbType.Text, DBNull.Value);
                            //}
                            //else
                            //{
                            //    commandOdds.Parameters.AddWithValue("p_odd_probability", NpgsqlDbType.Text, DBNull.Value);
                            //}

                            //commandOdds.Parameters.AddWithValue("p_odd_format", NpgsqlDbType.Text, DBNull.Value);
                            //var odds_insert_id = common.insert(commandOdds);
                            #endregion
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
            }


#if DEBUG
            OutCount += 1;
            Logg.logger.Error("InCount Count = " + InCount + "  ||||| OutCount = " + OutCount);
#endif
        }
    }
}
