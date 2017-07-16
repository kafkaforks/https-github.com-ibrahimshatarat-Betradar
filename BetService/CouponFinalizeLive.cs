using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SharedLibrary;

namespace BetService
{
    partial class CouponFinalizeLive : ServiceBase
    {
        private Timer timer1 = null;
        public CouponFinalizeLive()
        {
            InitializeComponent();
        }
        public CouponFinalizeLive(string[] args)
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
            timer1 = new Timer();
            timer1.Interval = 100;
            timer1.Elapsed += timer1_Tick;
            timer1.Enabled = true;
            timer1.Start();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            timer1.Stop();
            finalize();
            var coup = new Coupons();
            coup.GetAllFinalizedCoupons();
            timer1.Start();
        }

        private void after_finalise()
        {

        }

        private void finalize()
        {
            var coupons = new Coupons();
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                var count_processed = 0;
                Console.WriteLine(BetClearingQueueLive.StringQueueLive.Count);
                if (BetClearingQueueLive.StringQueueLive.Count > 0)
                {
                    var db_list = coupons.GetDBQueue();
                    var count = db_list.Count;
                    while (BetClearingQueueLive.StringQueueLive.Count > 0)
                    {
                        var element_string = BetClearingQueueLive.StringQueueLive.Dequeue();
                        var tagged = db_list.Select((item, i) => new { Item = item, Index = (int?)i });
                        int? index = (from pair in tagged
                                      where pair.Item == element_string
                                      select pair.Index).FirstOrDefault();

                        if (index != null && index > 0)
                        {
                            db_list.RemoveAt((int)index);
#if DEBUG
                            Console.WriteLine(element_string);
#endif
                            Task.Factory.StartNew(() =>
                            {
                                var coupon = new Coupons();
                                coupon.MatchFinalize(element_string);
                            });
                        }
#if DEBUG
                        count_processed += 1;
                        Console.WriteLine(":::::::::::::::::::::::::::::::::::::: " + count_processed + " ::::::::::::::::::::::::::::::::::::::");
#endif
                    }
                    System.Threading.Thread.Sleep(500);
                    stopwatch.Stop();
#if DEBUG
                    Logg.logger.Fatal("THIS IS :   " + stopwatch.ElapsedMilliseconds + "  ::::  COUNT: " +
                                      count.ToString());
#endif
                }
                else
                {
#if DEBUG
                    Console.WriteLine("This Queue is empty !!!");
#endif
                }
            }
            catch (Exception ex)
            {
                //this.EventLog.WriteEntry(ex.Message, EventLogEntryType.Information);
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Bet Error  " + ex.Message, EventLogEntryType.Information, 101, 1);
                }
            }
        }
    }
}
