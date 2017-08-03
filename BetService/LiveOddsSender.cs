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
    partial class LiveOddsSender : ServiceBase
    {
        private Timer timer1 = null;
        public LiveOddsSender()
        {
            InitializeComponent();
        }

        public LiveOddsSender(string[] args)
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
            //timer1 = new Timer();
            //timer1.Interval = 11000;
            //timer1.Elapsed += timer1_Tick;
            //timer1.Enabled = true;
            //timer1.Start();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                timer1.Enabled = false;
                while (Globals.LiveOddsQueue.Count > 0)
                {
                    ZMQClient.Instance.SendOutQueue(Globals.LiveOddsQueue.Dequeue().ToString());
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
