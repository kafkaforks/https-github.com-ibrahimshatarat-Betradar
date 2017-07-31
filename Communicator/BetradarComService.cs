using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using NetMQ;
using SharedLibrary;
using Timer = System.Timers.Timer;

namespace Communicator
{
    partial class BetradarComService : ServiceBase
    {
        public static Timer timerRedisChannel = null;
        public static Queue<string> redisQueue;
        public static string BetQueueName = ".\\private$\\BetQueue";
        public BetradarComService()
        {
            InitializeComponent();
        }
        public BetradarComService(string[] args)
        {
            InitializeComponent();
#if DEBUG
            TestStartupAndStop(args);
#endif
        }

        protected override void OnStart(string[] args)
        {
            Logg.logger.Fatal("Connector Start at UTC: " + DateTime.UtcNow.ToString());
            timerRedisChannel = new Timer();
            timerRedisChannel.Interval = 1000;
            timerRedisChannel.Elapsed += timerRedisChannel_Tick;
            timerRedisChannel.Enabled = true;
            timerRedisChannel.Start();
            // TODO: Add code here to start your service.

        }

        private void timerRedisChannel_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {

                        if (MessageQueue.Exists(BetQueueName))
                        {
                            MessageQueue queue = new MessageQueue(BetQueueName);
                            while (queue.CanRead)
                            {
                                var msg = queue.Receive(TimeSpan.FromMilliseconds(1000));
                                if (msg != null)
                                {
                                    msg.Formatter = new BinaryMessageFormatter();
                                    ConnectorGlobal.SendRedisChannel(msg.Body.ToString());
                                }

                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        if (ex.Message != "Timeout for the requested operation has expired.")
                        {
                            Logg.logger.Fatal("ERROR: " + ex.Message);
                        }
                    }
                });
            }

            catch (Exception ex)
            {
                Logg.logger.Fatal("ERROR: " + ex.Message);
            }
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }
        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            timerRedisChannel.Stop();
            timerRedisChannel.Dispose();
            timerRedisChannel = null;
        }
    }
}
