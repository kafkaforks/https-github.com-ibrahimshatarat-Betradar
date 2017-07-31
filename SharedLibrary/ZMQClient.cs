using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json.Linq;
using SharedLibrary;
using StackExchange.Redis;

namespace SharedLibrary
{
    public class ZMQClient
    {
        private static ConnectionMultiplexer Rconnect = RedisConnectorHelper.RedisConn;
        private static ISubscriber sub = Rconnect.GetSubscriber();
        public static ZMQClient Instance = new ZMQClient();

        public ZMQClient()
        {
        }

        public  void SendOut(string Channel, string Data, string username, string password, string node,
            string external_content_id, string bet_event)
        {

            SendRedisChannelZmq(Channel, Data, username, password, node, external_content_id, bet_event);

        }

        public void SendOutQueue(string message)
        {
            SenMQueue(message);
        }

        private  async void SendRedisChannelZmq(string Channel, string Data, string username, string password, string node, string external_content_id, string bet_event)
        {
            try
            {
                var address = Core.config.AppSettings.Get("RedisCommandChannel");
                var data = new JObject();
                if (!String.IsNullOrEmpty(node))
                {
                    //  data["external_content_name"] = node;
                }
                if (!String.IsNullOrEmpty(external_content_id))
                {
                    // data["external_content_id"] = external_content_id;
                }
                data["auth"] = new JObject();
                data["event"] = "data.upsert";
                data["data"] = new JObject();
                data["auth"]["username"] = Core.config.AppSettings.Get("HybridgeClientUserName");
                data["auth"]["password"] = Core.config.AppSettings.Get("HybridgeClientUserPassword");
                data["data"]["channel"] = Channel;
                data["data"]["event"] = bet_event;
                data["data"]["payload"] = Data;
                SendToredis(data.ToString());
                //Globals.LiveOddsQueue.Enqueue(data.ToString());
                //SenMQueue(data.ToString());
                data = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }
        private async void SendToredis(string message)
        {
            try
            {
                var address = Core.config.AppSettings.Get("RedisCommandChannel");

                if (sub.IsConnected(address))
                {
                    var res = await sub.PublishAsync(address, message);
                }
                else
                {
                    sub = Rconnect.GetSubscriber();
                    var res = await sub.PublishAsync(address, message);
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal("ERROR: " + ex.Message);
            }
        }

        private void SenMQueue(string message)
        {
            try
            {
                MessageQueue mq;
                if (!MessageQueue.Exists(Globals.BetQueueName))
                {
                    mq = MessageQueue.Create(Globals.BetQueueName);
                }
                else
                {
                    mq = new MessageQueue(Globals.BetQueueName);
                }
                Message msg = new Message
                {
                    Formatter = new BinaryMessageFormatter(),
                    Body = message,
                    Label = "QueueMessage"
                };
                mq.Send(msg);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

    }
}
