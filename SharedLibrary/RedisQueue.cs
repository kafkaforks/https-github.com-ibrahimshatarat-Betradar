using System;
using System.Collections.Generic;
using NLog.Internal;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using PhoenixSocket;
using ServiceStack.Messaging;
using StackExchange.Redis;

namespace SharedLibrary
{
    public static class RedisQueue
    {
        public static ConfigurationManager config = new ConfigurationManager();
        public static Queue<BetClearQueueElement> QueueBet;
        // static IDatabase cache = RedisConnectorHelper.Connection.GetDatabase();
        private static ConnectionMultiplexer Rconnect = RedisConnectorHelper.RedisConn;
        private static ISubscriber sub = Rconnect.GetSubscriber();
        public static void BetClear_Enqueue(BetClearQueueElement bet)
        {
            try
            {
                //QueueBet.Enqueue(bet);
                //cache.ListLeftPush(config.AppSettings.Get("RedisBetClearQueue"), new JavaScriptSerializer().Serialize(bet));
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }


        }

        public static void Api_Send_Enqueue()
        {
            //cache.ListLeftPush(config.AppSettings.Get("RedisSendApiQueue"), new JavaScriptSerializer().Serialize("{gogog}"));
        }

        public static void Send_Redis_Channel(string Channel, string Data, string username, string password, string node, string external_content_id, string bet_event)
        {
            try
            {
                var address = config.AppSettings.Get("RedisCommandChannel");
                var data = new JObject();
                if (!string.IsNullOrEmpty(node))
                {
                  //  data["external_content_name"] = node;

                }
                if (!string.IsNullOrEmpty(external_content_id))
                {
                   // data["external_content_id"] = external_content_id;
                }
                data["auth"] = new JObject();
               // data["data.upsert"] = new JObject();
                data["event"] = "data.upsert";
                data["data"] = new JObject();
                data["auth"]["username"] = Core.config.AppSettings.Get("HybridgeClientUserName");
                data["auth"]["password"] = Core.config.AppSettings.Get("HybridgeClientUserPassword");
                data["data"]["channel"] = Channel;
                data["data"]["event"] = bet_event;
                data["data"]["payload"] = Data;
                if (sub.IsConnected(address))
                {
                    sub.Publish(address, data.ToString());
                }
                else
                {
                    sub = Rconnect.GetSubscriber();
                    sub.Publish(address, data.ToString());
                }
                data = null;

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public static BetClearQueueElement BetClear_Dequeue()
        {

            try
            {
                //QueueBet.Dequeue();
                // return new JavaScriptSerializer().Deserialize<BetClearQueueElement>(/*cache.ListRightPop(config.AppSettings.Get("RedisBetClearQueue")*/));
                return null;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }

        }
    }
}
