using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Betradar.Classes.Socket;
using Npgsql;
using SharedLibrary;
using Sportradar.SDK.Common.Interfaces;
using Sportradar.SDK.Services.Sdk;

namespace BetService
{
   public  class Main
    {
       // public static Sdk m_sdk = Sdk.Instance;
        public void StartBetradarAll()
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
            var enabled_feeds = new List<IStartable>();
            // Task.Factory.StartNew(() => TaskHandler.StartErrorWatch());
            // Task.Factory.StartNew(() => TaskHandler.StartOddChangeWatch());
            // TaskHandler.StartOddChangeWatch();


            #region socket_read


            if (Betradar.m_sdk.BetPal != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(Betradar.m_sdk.BetPal, "BetPal", TimeSpan.FromHours(12)));
            }
            if (Betradar.m_sdk.LiveOdds != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(Betradar.m_sdk.LiveOdds, "LiveOdds", TimeSpan.FromHours(12)));
            }
            if (Betradar.m_sdk.LiveOddsVdr != null)
            {
                enabled_feeds.Add(new LiveOddsRaceModule(Betradar.m_sdk.LiveOddsVdr, "LiveOddsVdr", TimeSpan.FromHours(2)));
            }
            if (Betradar.m_sdk.LiveOddsVbl != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(Betradar.m_sdk.LiveOddsVbl, "LiveOddsVbl", TimeSpan.FromHours(2)));
            }
            if (Betradar.m_sdk.LiveOddsVfc != null)
            {
                enabled_feeds.Add(new LIveOddsWithOutrightsModule(Betradar.m_sdk.LiveOddsVfc, "LiveOddsVfc", TimeSpan.FromHours(2)));
            }
            if (Betradar.m_sdk.LiveOddsVfl != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(Betradar.m_sdk.LiveOddsVfl, "LiveOddsVfl", TimeSpan.FromHours(2)));
            }
            if (Betradar.m_sdk.LiveOddsVhc != null)
            {
                enabled_feeds.Add(new LiveOddsRaceModule(Betradar.m_sdk.LiveOddsVhc, "LiveOddsVhc", TimeSpan.FromHours(2)));
            }
            if (Betradar.m_sdk.LiveOddsVto != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(Betradar.m_sdk.LiveOddsVto, "LiveOddsVto", TimeSpan.FromHours(2)));
            }
            if (Betradar.m_sdk.LivePlex != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(Betradar.m_sdk.LivePlex, "LivePlex", TimeSpan.FromHours(12)));
            }
            if (Betradar.m_sdk.SoccerRoulette != null)
            {
                enabled_feeds.Add(new LiveOddsMatchModule(Betradar.m_sdk.SoccerRoulette, "SoccerRoulette", TimeSpan.FromHours(12)));
            }
            //if (Betradar.m_sdk.LiveScout != null)
            //{
            //    var cfm = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //    var sdk_section = (SdkConfigurationSection)cfm.GetSection("Sdk");
            //    enabled_feeds.Add(new LiveScoutModule(Betradar.m_sdk.LiveScout, "LiveScout", sdk_section.LiveScout.Test));
            //}
            if (Betradar.m_sdk.Lcoo != null)
            {
                enabled_feeds.Add(new LcooModule(Betradar.m_sdk.Lcoo, "Fixtures"));
            }
            if (Betradar.m_sdk.OddsCreator != null)
            {
                enabled_feeds.Add(new OddsCreatorModule(Betradar.m_sdk.OddsCreator, "OddsCreator"));
            }

            enabled_feeds.ForEach(x => x.Start());
            #endregion

            // Console.ReadLine();
            // enabled_feeds.ForEach(xx => xx.Stop());
            // m_sdk.Stop();
        }
    }
}
