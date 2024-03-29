﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Npgsql;
using SharedLibrary;
using Timer = System.Timers.Timer;

namespace BetService
{
    public partial class CouponFinalize : ServiceBase
    {

        public static Timer timerFinalise = null;
        public static Timer timerRedisChannel = null;
        private Timer AliveTimer = null;
       // Thread m_thread = null;


        public CouponFinalize()
        {
            InitializeComponent();
        }

        public CouponFinalize(string[] args)
        {
            InitializeComponent();
#if DEBUG
            TestStartupAndStop(args);
#endif
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
            //m_thread = new Thread(new ThreadStart(ThreadProc));
            // start the thread
            //Debugger.Break();
            Globals.timerOnOff = true;
            Globals.redisTimerOnOff = true;
            //m_thread.Start(); 
            timerFinalise = new Timer();
            timerFinalise.AutoReset = false;
            timerFinalise.Interval = 30000;
            lock (timerFinalise)
            {
                timerFinalise.Elapsed += TimerFinaliseTick;
            }
            timerFinalise.Enabled = true;
            timerFinalise.Start();

            AliveTimer = new Timer();
            AliveTimer.Interval = 11000;
            AliveTimer.Elapsed += AliveTimer_Tick;
            AliveTimer.Enabled = true;
            AliveTimer.Start();

            timerRedisChannel = new Timer();
            timerRedisChannel.Interval = 1000;
            timerRedisChannel.Elapsed += timerRedisChannel_Tick;
            timerRedisChannel.Enabled = true;
            timerRedisChannel.Start();

        }

        protected override void OnStop()
        {
            // Service stopped. Also stop the timers.
            timerFinalise.Stop();
            timerFinalise.Dispose();
            timerFinalise = null;

            AliveTimer.Stop();
            AliveTimer.Dispose();
            AliveTimer = null;
        }

        private void TimerFinaliseTick(object sender, ElapsedEventArgs e)
        {
            timerFinalise.Enabled = false;
            //finalize();
            if (Globals.timerOnOff)
            {
                Globals.timerOnOff = false;
                finalize();
                Globals.timerOnOff = true;
            }

            timerFinalise.Enabled = true;
        }

        private void finalize()
        {
            try
            {
                var coupon = new Coupons();
               var ds =  coupon.getAllUNfinalisedMid();
                if (ds.Tables.Count>0)
                {
                    if (ds.Tables[0].Rows.Count>0)
                    {
                        foreach (var mid in ds.Tables[0].Rows)
                        {
                            coupon.MatchFinalize(mid.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }

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
                        //var co = new Coupons();
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

        private void AliveTimer_Tick(object sender, ElapsedEventArgs e)
        {
            //var cou = new SharedLibrary.Coupons();
            //AliveTimer.Stop();
            //cou.WorkAlive();
            //AliveTimer.Start();
        }

        private void timerRedisChannel_Tick(object sender, ElapsedEventArgs e)
        {
            //timerRedisChannel.Enabled = false;
            //if (Globals.redisTimerOnOff)
            //{
            //    Globals.redisTimerOnOff = false;
            //    RedisQueue.SendRedisChannelFromQueue();
            //}

            //timerRedisChannel.Enabled = true;

        }


        [Conditional("DEBUG_SERVICE")]
        private static void DebugMode()
        {
            Debugger.Break();
        }

        private void finalize_coupon_temp()
        {
            //try
            //{
            //    var bet = RedisQueue.BetClear_Dequeue();
            //    if (bet != null)
            //    {
            //        Logg.logger.Debug(bet.MatchId.ToString() + " : " + bet.OddsId.ToString());
            //    }
            //    else
            //    {
            //        Logg.logger.Info("No queue elements found!");
            //    }
            //}
            //catch (Exception ex)
            //{

            //    Logg.logger.Fatal(ex.Message);
            //}

        }


    }
}