using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Betradar.Classes;
using Betradar.Classes.DbInsert;
using Betradar.Classes.Socket;
using Npgsql;
using ServiceStack;
using SharedLibrary;
using SharedLibrary.RPC;
using Sportradar.SDK.Common.Interfaces;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using Sportradar.SDK.Services.Sdk;
using Sportradar.SDK.Services.SdkConfiguration;

namespace Betradar
{

    class Program : Core
    {
       
        public static Sdk m_sdk = Sdk.Instance;
        static void Main(string[] args)
        {
            Globals.Queue_Errors = new BetQueue<NpgsqlCommand>();
            Globals.Queue_Odd_Change = new BetQueue<OddChangeQueue>();
            Globals.Queue_BetClearQueueElementLive = new BetQueue<BetClearQueueElementLive>();
            #region TESTING
           // Console.ReadLine();
            //var client = new Client();
            //var c = new Core();
            //var proxy = client.ServerproxyLive();
            //var belive = new BetClearQueueElementLive();
            //belive.MatchId = 23456789;
            //belive.OddId = 98765432;
            //belive.TypeId = 123123;

            //proxy.AddStringQueueLive(c.EncodeUnifiedBetClearQueueElementLive(belive));
            ////var c = new Core();
            ////var soso = c.CreateLiveOddsChannelName(11707883, "tr");
           // Console.ReadLine();
            //var com = new Common();
            //var belive = new BetClearQueueElementLive();
            //belive.MatchId = 23456789;
            //belive.OddId = 98765432;
            //belive.TypeId = 123123;
            //    //23456789|98765432|123123
            //var toto = com.EncodeUnifiedBetClearQueueElementLive(belive);
            ////var c = new Core();

            ////var tut = c.CreateLiveOddsChannelName(11, "tr");
            //Console.ReadLine();

            //var clientSock = new HybridgeClient();
            //var resp = clientSock.SendData(config.AppSettings.Get("HybridgeClientTokenLive"), "{\"WHAT???\": \"Hello\"}");
            //var c = new Core();
            //c.SendToHybridgeSocket(11, 22, 33, "{bobo}");
            //Console.ReadLine();


            //try
            //{
            //    var count = 0;
            //    var client = new Client();
            //    var proxy = client.Serverproxy();
            //    var cop = new Coupons();
            //    var data = cop.GetDBQueueTest();
            //    for (int i = 0; i < data.Count; i++)
            //    {
            //        proxy.AddStringQueue(data[i]);
            //        count += 1;
            //        Console.WriteLine(data[i] + " ::: " + count);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Logg.logger.Fatal(ex.Message);
            //}
            //Console.ReadLine();
            //client = new ClientWebSocket();
            ///var uri = new Uri(host);
            // client.ConnectAsync(uri, CancellationToken.None);
            //using (var ws = new WebSocket("ws://195.244.59.34/socket"))
            //{
            // ws.ConnectAsync();


            //var count = 0;
            //for (int i = 0; i < 1000000; i++)
            //{
            //    // var z = client.State;
            //    count += 1;
            //    Console.WriteLine("count is : " + (count).ToString());
            //    Task.Factory.StartNew(
            //            () =>
            //                new Core().sendToPusher("This is Heytham testing!!!!   count is :"+count));

            //}
            //}
            // client.Close();

            //Globals.Queue_ScoreCardSummary = new BetQueue<ScoreCardSummaryEventArgs>();
            //Globals.Queue_MatchEventOdds = new BetQueue<MatchEventOdds>();
            //Globals.Queue_ThreeBallEvent = new BetQueue<ThreeBallEventArgs>();
            //Globals.Queue_BetCancel = new BetQueue<BetCancelEventArgs>();
            //Globals.Queue_BetCancelUndo = new BetQueue<BetCancelUndoEventArgs>();
            //Globals.Queue_BetClear = new BetQueue<BetClearEventArgs>();
            //Globals.Queue_BetClearRollBack = new BetQueue<BetClearRollbackEventArgs>();
            //Globals.Queue_Feed = new BetQueue<string>();
            //Globals.Queue_ScoreCardSummary = new BetQueue<ScoreCardSummaryEventArgs>();
            //Globals.Queue_OutRightEventOdds = new BetQueue<OutrightEventOdds>();
            //Globals.Queue_BetStart =  new BetQueue<BetStartEventArgs>();
            //Globals.Queue_BetStop = new BetQueue<BetStopEventArgs>();
            //Globals.Queue_OddsChange = new BetQueue<OddsChangeEventArgs>();
            //Globals.Queue_OutrightBetClear = new BetQueue<OutrightBetClearEventArgs>();
            //Globals.Queue_OutrightBetStart = new BetQueue<OutrightBetStartEventArgs>();
            //Globals.Queue_OutrightBetStop = new BetQueue<OutrightBetStopEventArgs>();


            //Task.Factory.StartNew(() => new test());
            //Task.Factory.StartNew(() => new OutrightEventOdds_Queue());
            //Task.Factory.StartNew(() => new ScoreCardSummary_Queue());
            //Task.Factory.StartNew(() => new MatchEventOdds_Queue());
            //Task.Factory.StartNew(() => new ThreeBallEvent_Queue());
            //Task.Factory.StartNew(() => new BetCancel_Queue());
            //Task.Factory.StartNew(() => new BetCancelUndo_Queue());

            //Task.Factory.StartNew(() => new BetClearRollBack_Queue());
            //Task.Factory.StartNew(() => new BetStart_Queue());
            //Task.Factory.StartNew(() => new BetStop_Queue());
            //Task.Factory.StartNew(() => new OddsChange_Queue());
            //Task.Factory.StartNew(() => new OutrightBetClear_Queue());
            //Task.Factory.StartNew(() => new OutrightBetStart_Queue());
            //Task.Factory.StartNew(() => new OutrightBetStop_Queue());
            //Task.Factory.StartNew(() => new feed());
            //Task.Factory.StartNew(() => pingNotipier());
            #endregion

           // Task.Factory.StartNew(() => TaskHandler.StartErrorWatch());
           // Task.Factory.StartNew(() => TaskHandler.StartOddChangeWatch());
            // TaskHandler.StartOddChangeWatch();
            m_sdk.Initialize();
            m_sdk.Start();

            #region socket_read
            var enabled_feeds = new List<IStartable>();

            if (m_sdk.BetPal != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(m_sdk.BetPal, "BetPal", TimeSpan.FromHours(12)));
            }
            if (m_sdk.LiveOdds != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(m_sdk.LiveOdds, "LiveOdds", TimeSpan.FromHours(12)));
            }
            if (m_sdk.LiveOddsVdr != null)
            {
                enabled_feeds.Add(new LiveOddsRaceModule(m_sdk.LiveOddsVdr, "LiveOddsVdr", TimeSpan.FromHours(2)));
            }
            if (m_sdk.LiveOddsVbl != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(m_sdk.LiveOddsVbl, "LiveOddsVbl", TimeSpan.FromHours(2)));
            }
            if (m_sdk.LiveOddsVfc != null)
            {
                enabled_feeds.Add(new LIveOddsWithOutrightsModule(m_sdk.LiveOddsVfc, "LiveOddsVfc", TimeSpan.FromHours(2)));
            }
            if (m_sdk.LiveOddsVfl != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(m_sdk.LiveOddsVfl, "LiveOddsVfl", TimeSpan.FromHours(2)));
            }
            if (m_sdk.LiveOddsVhc != null)
            {
                enabled_feeds.Add(new LiveOddsRaceModule(m_sdk.LiveOddsVhc, "LiveOddsVhc", TimeSpan.FromHours(2)));
            }
            if (m_sdk.LiveOddsVto != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(m_sdk.LiveOddsVto, "LiveOddsVto", TimeSpan.FromHours(2)));
            }
            if (m_sdk.LivePlex != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(m_sdk.LivePlex, "LivePlex", TimeSpan.FromHours(12)));
            }
            if (m_sdk.SoccerRoulette != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(m_sdk.SoccerRoulette, "SoccerRoulette", TimeSpan.FromHours(12)));
            }
            if (m_sdk.LiveScout != null)
            {
                var cfm = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var sdk_section = (SdkConfigurationSection)cfm.GetSection("Sdk");
                enabled_feeds.Add(new LiveScoutModule(m_sdk.LiveScout, "LiveScout", sdk_section.LiveScout.Test));
            }
            if (m_sdk.Lcoo != null)
            {
                enabled_feeds.Add(new LcooModule(m_sdk.Lcoo, "Fixtures"));
            }
            if (m_sdk.OddsCreator != null)
            {
                enabled_feeds.Add(new OddsCreatorModule(m_sdk.OddsCreator, "OddsCreator"));
            }

            enabled_feeds.ForEach(x => x.Start());
            #endregion

            Console.ReadLine();
            enabled_feeds.ForEach(xx => xx.Stop());
            m_sdk.Stop();
        }
        public void ReadData()
        {
            //var cache = RedisConnectorHelper.Connection.GetDatabase();
            //var devicesCount = 10000;
            //for (int i = 0; i < devicesCount; i++)
            //{
            //    var value = cache.StringGet($"heytham:shayeb");
            //    Console.WriteLine($"Valor={value}");
            //}

        }
        public static Boolean RollbackOperation(long couponId, int action)
        {
            NpgsqlConnection con;
            NpgsqlCommand cmd;
            var connectionBuilder = new NpgsqlConnectionStringBuilder();
            connectionBuilder.Host = config.AppSettings.Get("DB_Host");
            connectionBuilder.Port = int.Parse(config.AppSettings.Get("DB_Port"));
            connectionBuilder.Database = config.AppSettings.Get("DB_Database");
            connectionBuilder.Username = config.AppSettings.Get("DB_Username");
            connectionBuilder.Password = config.AppSettings.Get("DB_Password");
            connectionBuilder.Timeout = 20;
            connectionBuilder.Pooling = true;
            connectionBuilder.CommandTimeout = 20;
            con = new NpgsqlConnection(connectionBuilder.ConnectionString);
            try
            {
                String select = "START TRANSACTION; SAVEPOINT HERE; SELECT coupon_date, amount, total_odds, merchant_id, odds_id, user_id, last_status FROM cp_coupon_temp WHERE id = " + couponId + ";";

                /*SELECT*/
                using (cmd = new NpgsqlCommand(select, con))
                {
                    if (con.State != ConnectionState.Open) con.Open();
                    NpgsqlDataReader read = cmd.ExecuteReader();

                    /*SELECT*/
                    /*INSERT*/

                    /*şimdi action*/
                    if (action == 1)
                    {
                        String insert = "INSERT INTO cp_coupon_finalised  (id, coupon_date, amount, total_odds, merchant_id, odds_id, user_id, last_status) VALUES (" + couponId + ", ";
                        Boolean forTheFirstTime = true;
                        while (read.Read())
                        {
                            for (int i = 0; i < read.FieldCount; i++)
                            {

                                if (forTheFirstTime)
                                {
                                    insert += "'" + read[i].ToString() + "', ";
                                    forTheFirstTime = false;
                                    continue;
                                }
                                insert += read[i].ToString();
                                if (i + 1 < read.FieldCount)
                                    insert += ", ";
                                else
                                    insert += ");";
                            }
                        }
                        con.Close();
                        using (cmd = new NpgsqlCommand(insert, con))
                        {
                            if (con.State != ConnectionState.Open) con.Open();
                            cmd.CommandText = insert;
                            cmd.ExecuteScalar();

                        }


                    }
                    else
                    {

                        String insert = "INSERT INTO cp_coupon_cancelled  (id, coupon_date, amount, total_odds, merchant_id, odds_id, user_id, last_status) VALUES (" + couponId + ", ";
                        Boolean forTheFirstTime = true;
                        while (read.Read())
                        {
                            for (int i = 0; i < read.FieldCount; i++)
                            {

                                if (forTheFirstTime)
                                {
                                    insert += "'" + read[i].ToString() + "', ";
                                    forTheFirstTime = false;
                                    continue;
                                }
                                insert += read[i].ToString();
                                if (i + 1 < read.FieldCount)
                                    insert += ", ";
                                else
                                    insert += ");";
                            }
                        }

                        using (cmd = new NpgsqlCommand(insert, con))
                        {
                            if (con.State != ConnectionState.Open) con.Open();
                            cmd.CommandText = insert;
                            cmd.ExecuteScalar();

                        }
                    }
                }
                /*INSERT*/

                /*DELETE*/
                String delete = "DELETE FROM cp_coupon_temp WHERE id = " + couponId + ";";
                using (cmd = new NpgsqlCommand(delete, con))
                {
                    if (con.State != ConnectionState.Open) con.Open();
                    cmd.CommandText = delete;
                    cmd.ExecuteScalar();

                }
                /*DELETE*/

            }
            catch (SystemException ex)
            {

                String Rollback = "ROLLBACK TO SAVEPOINT HERE;";
                using (cmd = new NpgsqlCommand(Rollback, con))
                {
                    if (con.State != ConnectionState.Open) con.Open();
                    cmd.CommandText = Rollback;
                    cmd.ExecuteScalar();

                }

                Console.WriteLine(ex.Message);
                con.Close();
                return false;
            }

            String EndTransaction = "END";
            using (cmd = new NpgsqlCommand(EndTransaction, con))
            {
                if (con.State != ConnectionState.Open) con.Open();
                cmd.CommandText = EndTransaction;
                cmd.ExecuteScalar();

            }
            con.Close();
            return true;
        }
    }
}
