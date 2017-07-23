using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Betradar.Classes.Socket;
using Npgsql;
using SharedLibrary;
using Sportradar.SDK.Common.Interfaces;
using Sportradar.SDK.Services.Sdk;
using Sportradar.SDK.Services.SdkConfiguration;
using ConfigurationManager = NLog.Internal.ConfigurationManager;
using Timer = System.Timers.Timer;

namespace BetService
{
    partial class Betradar : ServiceBase
    {
        private static Timer AliveTimer = null;
        public static long TimerCount = 0;
        private Timer timer1 = null;
        Thread m_thread = null;
        public static Sdk m_sdk = Sdk.Instance;
        private List<IStartable> enabled_feeds;
        Thread _thread;

        public Betradar()
        {
            InitializeComponent();
        }

        public Betradar(string[] args)
        {
            InitializeComponent();
            TestStartupAndStop(args);
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }

        protected override void OnStart(string[] args)
        {
            
            // instantiate the thread
            // m_thread = new Thread(new ThreadStart(ThreadProc));
            // start the thread
            // m_thread.Start();
            //Debugger.Break();

            timer1 = new Timer();
            timer1.Interval = 11000;
            timer1.Elapsed += timer1_Tick;
            timer1.Enabled = true;
            timer1.Start();


            //StartBetradarAll();
            AliveTimer = new Timer();
            AliveTimer.Interval = 600000;
            AliveTimer.Elapsed += AliveTimer_Tick;
            AliveTimer.Enabled = true;
            AliveTimer.Start();
            initFeed();
            // _thread = new Thread(new Main().StartBetradarAll);
            // _thread.Start();

        }

        public void ThreadProc()
        {
            // we're going to wait 5 minutes between calls to GetEmployees, so 
            // set the interval to 300000 milliseconds 
            // (1000 milliseconds = 1 second, 5 * 60 * 1000 = 300000)
            int interval = 300000; // 5 minutes    
                                   // this variable tracks how many milliseconds have gone by since 
                                   // the last call to GetEmployees. Set it to zero to indicate we're 
                                   // starting fresh
            int elapsed = 0;
            // because we don't want to use 100% of the CPU, we will be 
            // sleeping for 1 second between checks to see if it's time to 
            // call GetEmployees
            int waitTime = 100000; // 1 second
            try
            {
                // do this loop forever (or until the service is stopped)
                while (true)
                {
                    // if enough time has passed
                    if (interval >= elapsed)
                    {
                       // var co = new Coupons();
                        // reset how much time has passed to zero
                        elapsed = 0;
                        // call GetEmployees
                        //co.GetDummyData();
                    }
                    // Sleep for 1 second
                    Thread.Sleep(waitTime);
                    // indicate that 1 additional second has passed
                    elapsed += waitTime;
                }
            }
            catch (ThreadAbortException)
            {
                // we want to eat the excetion because we don't care if the 
                // thread has aborted since we probably did it on purpose by 
                // stopping the service.
            }
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            timer1.Enabled = false;
            WorkAlive();
            timer1.Enabled = true;
        }

        private void AliveTimer_Tick(object sender, ElapsedEventArgs e)
        {
            AliveTimer.Enabled= false;
            initFeed();

            AliveTimer.Enabled = true;
        }

        private void timerKeepAlive_Tick(object sender, ElapsedEventArgs e)
        {
            TimerCount+=1;
        }

        private void initFeed()
        {
            //Task.Factory.StartNew(HybridgeClient.JoinChannel);
            //HybridgeClient.JoinChannel();
            if (!m_sdk.IsStarted)
            {
                if (!m_sdk.IsInitialized)
                {
                    m_sdk.Initialize();
                }
                m_sdk.Start();
                enabled_feeds = new List<IStartable>();
                StartBetradarAll();
            }
            //else
            //{
            //    reloadBetradar();
            //}
        }

        private void reloadBetradar()
        {
            enabled_feeds.ForEach(xx => xx.Stop());
            m_sdk.Stop();
            // m_sdk.Initialize();
            m_sdk.Start();
            enabled_feeds.ForEach(xx => xx.Start());
            //enabled_feeds = new List<IStartable>();
        }

        protected override void OnStop()
        {
            enabled_feeds.ForEach(xx => xx.Stop());
            m_sdk.Stop();
            AliveTimer.Stop();
            AliveTimer.Dispose();
            AliveTimer = null;
        }

        public void WorkAlive()
        {
            var cou = new SharedLibrary.Coupons();
            cou.WorkAlive();
        }

        private void StartBetradarAll()
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


            #region socket_read


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
            //if (m_sdk.LiveScout != null)
            //{
            //    var cfm = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //    var sdk_section = (SdkConfigurationSection)cfm.GetSection("Sdk");
            //    enabled_feeds.Add(new LiveScoutModule(m_sdk.LiveScout, "LiveScout", sdk_section.LiveScout.Test));
            //}
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

            // Console.ReadLine();
            // enabled_feeds.ForEach(xx => xx.Stop());
            // m_sdk.Stop();
        }
    }
}
