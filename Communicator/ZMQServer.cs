using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json.Linq;
using SharedLibrary;
using StackExchange.Redis;

namespace Communicator
{
    public static class ZMQServer
    {
        private static NetMQContext m_context;
        private static Poller m_poller;
        private static NetMQTimer m_timeoutTimer;

        public static void Server()
        {
            try
            {
                using (var subSocket = new SubscriberSocket())
                {
                    subSocket.Options.ReceiveHighWatermark = 0;
                    subSocket.Connect("tcp://127.0.0.1:10001");
                    subSocket.Options.Backlog = 100000000;
                    subSocket.Options.Linger = TimeSpan.FromSeconds(1);
                    subSocket.Options.MaxMsgSize = -1;
                    // subSocket.Poll();
                    subSocket.Subscribe("Betradar");
                    Logg.logger.Fatal("Subscriber socket connecting... at time UTC: "+DateTime.UtcNow.ToString());
                    while (true)
                    {
                        //string messageReceived = subSocket.ReceiveFrameString();
                        //string messageTopicReceived = subSocket.ReceiveFrameString();
                        
                        //string messageReceived = subSocket.ReceiveFrameString();
                        //var msg = subSocket.ReceiveMultipartMessage();
                        //string messageReceived = msg[msg.Count()-1].ToString();
                        string messageTopicReceived = subSocket.ReceiveFrameString();
                        string messageReceived = subSocket.ReceiveFrameString();
                        if (!string.IsNullOrEmpty(messageReceived))
                        {
                            Logg.logger.Fatal(messageReceived);
                            BetradarComService.redisQueue.Enqueue(messageReceived);
                            Logg.logger.Fatal(BetradarComService.redisQueue.Count.ToString());
                        }
                        //Console.WriteLine(messageTopicReceived + " TOPIC " + messageReceived);
                        //Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal("ERROR: "+ex.Message);
            }
        }

      
    }
}
