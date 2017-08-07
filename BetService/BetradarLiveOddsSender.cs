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
    partial class BetradarLiveOddsSender : ServiceBase
    {
        private Timer timer1 = null;
        public BetradarLiveOddsSender()
        {
            InitializeComponent();
        }

        public BetradarLiveOddsSender(string[] args)
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
            timer1.Interval = 11000;
            timer1.Elapsed += timer1_Tick;
            timer1.Enabled = true;
            timer1.Start();
        }

        protected override void OnStop()
        {
            timer1.Stop();
            timer1.Enabled = false;
        }
        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                timer1.Enabled = false;
                var address = Core.config.AppSettings.Get("RedisCommandChannel");

                if (!LiveOddSendClient.sub.IsConnected(address))
                {
                    LiveOddSendClient.sub = LiveOddSendClient.Rconnect.GetSubscriber();
                }
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }
    }
}
