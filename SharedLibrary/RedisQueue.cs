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
    public interface IRedisChannelObject
    {
        string Channel { get; set; }
        string Data { get; set; }
        string username { get; set; }
        string password { get; set; }
        string node { get; set; }
        string external_content_id { get; set; }
        string bet_event { get; set; }
        void SendRedisChannelFromZyan(RedisChannelObject obj);
    }
    [Serializable]
    public class RedisChannelObject : IRedisChannelObject
    {
        public string Channel { get; set; }
        public string Data { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string node { get; set; }
        public string external_content_id { get; set; }
        public string bet_event { get; set; }
        private static ConnectionMultiplexer Rconnect = RedisConnectorHelper.RedisConn;
        private static ISubscriber sub = Rconnect.GetSubscriber();
        public  void SendRedisChannelFromZyan(RedisChannelObject r_channel_object)
        {
            try
            {
                    var address = Core.config.AppSettings.Get("RedisCommandChannel");
                    var data = new JObject();
                    if (!string.IsNullOrEmpty(r_channel_object.node))
                    {
                        //  data["external_content_name"] = node;

                    }
                    if (!string.IsNullOrEmpty(r_channel_object.external_content_id))
                    {
                        // data["external_content_id"] = external_content_id;
                    }
                    data["auth"] = new JObject();
                    data["event"] = "data.upsert";
                    data["data"] = new JObject();
                    data["auth"]["username"] = Core.config.AppSettings.Get("HybridgeClientUserName");
                    data["auth"]["password"] = Core.config.AppSettings.Get("HybridgeClientUserPassword");
                    data["data"]["channel"] = r_channel_object.Channel;
                    data["data"]["event"] = r_channel_object.bet_event;
                    data["data"]["payload"] = r_channel_object.Data;

                    if (sub.IsConnected(address))
                    {
                        sub.Publish(address, data.ToString());
                    }
                    else
                    {
                        sub = Rconnect.GetSubscriber();
                        sub.Publish(address, data.ToString());
                    }
                    data.RemoveAll();
                    data = null;
                    r_channel_object = null;
                    GC.Collect();
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }
      
    }

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
                data.RemoveAll();
                data = null;
                GC.Collect();
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
