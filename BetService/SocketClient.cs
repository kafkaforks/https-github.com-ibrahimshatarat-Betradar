using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;
using StackExchange.Redis;

namespace BetService
{
    class SocketClient : Core
    {
        // public static Socket connector = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static ConnectionMultiplexer Rconnect = RedisConnectorHelper.RedisConn;
        private static ISubscriber sub = Rconnect.GetSubscriber();
        static async Task SendString(string value)
        {
            var encoding = Encoding.UTF8;
            try
            {
                Socket connector = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                connector.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6767));
                connector.Send(BitConverter.GetBytes(encoding.GetByteCount(value)));
                connector.Send(encoding.GetBytes(value));
            }
            catch (Exception ex)
            {
                SharedLibrary.Logg.logger.Fatal(ex.Message);
            }
        }
        private async void SendRedisChannelSocket(string Channel, string Data, string username, string password, string node, string external_content_id, string bet_event)
        {
            try
            {
                //var address = Core.config.AppSettings.Get("RedisCommandChannel");
                //var data = new JObject();
                //if (!String.IsNullOrEmpty(node))
                //{
                //    //  data["external_content_name"] = node;
                //}
                //if (!String.IsNullOrEmpty(external_content_id))
                //{
                //    // data["external_content_id"] = external_content_id;
                //}
                //data["auth"] = new JObject();
                //data["event"] = "data.upsert";
                //data["data"] = new JObject();
                //data["auth"]["username"] = Core.config.AppSettings.Get("HybridgeClientUserName");
                //data["auth"]["password"] = Core.config.AppSettings.Get("HybridgeClientUserPassword");
                //data["data"]["channel"] = Channel;
                //data["data"]["event"] = bet_event;
                //data["data"]["payload"] = Data;
                //Task.Factory.StartNew(()=>SendString(data.ToString()));
                var build = new StringBuilder(); //Channel+'|'+Data + '|' +username + '|' +password + '|' +node + '|' +external_content_id + '|' +bet_event

                build.Append(Channel);
                build.Append('|');
                build.Append(Data);
                build.Append('|');
                build.Append(username);
                build.Append('|');
                build.Append(password);
                build.Append('|');
                build.Append(node);
                build.Append('|');
                build.Append(external_content_id);
                build.Append('|');
                build.Append(bet_event);

                SendToredis(build.ToString());
                build = null;
                // var command = new NpgsqlCommand("insert_cp_send");
                // command.Parameters.AddWithValue("p_message", NpgsqlDbType.Text, data.ToString());
                // await insert(command);
                //Globals.LiveOddsQueue.Enqueue(data.ToString());
                //SenMQueue(data.ToString());
                //data = null;
                //GC.Collect();
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public void SendToredis(string message)
        {
            try
            {
                var address = Core.config.AppSettings.Get("RedisCommandChannel");

                if (sub.IsConnected(address))
                {
                    var res =  sub.Publish(address, message);
                }
                else
                {
                    sub = Rconnect.GetSubscriber();
                    var res =  sub.Publish(address, message);
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal("ERROR: " + ex.Message);
            }
        }

        public async Task SendToHybridgeSocket(long match_id, long odd_id, int? odd_eventoddsfield_typeid, string odd_name, string odd_special_odds_value, EventOddsField odd_in, string channel, string msg_event)
        {
            var oddUnique = new BetClearQueueElementLive();

            try
            {
                oddUnique.MatchId = match_id;
                oddUnique.OddId = odd_id;
                if (odd_eventoddsfield_typeid != null)
                {
                    oddUnique.TypeId = odd_eventoddsfield_typeid;
                }
                else
                {
                    oddUnique.TypeId = 0;
                }

                var oid = EncodeUnifiedBetClearQueueElementLive(oddUnique);
                var odd = new SocketOdd();
                odd.odd_eventoddsfield_active = odd_in.Active;
                odd.odd_name = odd_name;
                odd.odd_odd = odd_in.Value.ToString();
                odd.odd_special_odds_value = odd_special_odds_value;
                //TODO REDISCALL
                //TODO ASian handicap block
                if (odd_in.TypeId != 51 && odd_in.TypeId != 7)
                {
                    //Task.Factory.StartNew(()=>RedisQueue.Send_Redis_Channel(channel,
                    // "[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) + "]",
                    // "home@HYBRIDGE", "12345", "Odd", oid, msg_event));
                    if (oid != null && odd != null && msg_event != null)
                    {
                        Task.Run(() =>
                        SendRedisChannelSocket(channel, "[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) + "]", "home@HYBRIDGE", "12345", "Odd", oid.Result, msg_event)
                        ).ConfigureAwait(false);
                        //ZMQ = null;
                    }


                    // var channelQueueObject = new RedisChannelObject();
                    //channelQueueObject.Channel = channel;
                    //channelQueueObject.Data = "[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) + "]";
                    //channelQueueObject.username = config.AppSettings.Get("RedisUserName");
                    //channelQueueObject.password = config.AppSettings.Get("RedisPassword");
                    //channelQueueObject.node = "Odd";
                    //channelQueueObject.external_content_id = oid;
                    //channelQueueObject.bet_event = msg_event;
                    //Globals.Queue_RedisChannelSend.Enqueue(channelQueueObject);
                }

                //Task.Factory.StartNew(()=>HybridgeClient.SendDataSocketPhoenix(channel, "[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) + "]", "home@HYBRIDGE", "12345","Odd",oid,msg_event));
                //clientSock.SendData(channel,"[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) + "]");
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public async void SendToHybridgeSocketNewOdd(long match_id, long odd_id, int? odd_eventoddsfield_typeid, string odd_name, string odd_special_odds_value, EventOddsField odd_in, string channel, string odd_event)
        {
            var oddUnique = new BetClearQueueElementLive();
            try
            {
                oddUnique.MatchId = match_id;
                oddUnique.OddId = odd_id;
                if (odd_eventoddsfield_typeid != null)
                {
                    oddUnique.TypeId = odd_eventoddsfield_typeid;
                }
                else
                {
                    oddUnique.TypeId = 0;
                }
                var oid = EncodeUnifiedBetClearQueueElementLive(oddUnique);
                var odd = new SocketOdd();
                odd.odd_eventoddsfield_active = odd_in.Active;
                odd.odd_name = odd_name;
                odd.odd_odd = odd_in.Value.ToString();
                odd.odd_special_odds_value = odd_special_odds_value;
                if (odd_in.Probability != null)
                {
                    odd.odd_probability = odd_in.Probability.ToString();
                }
                odd.last_update = DateTime.UtcNow;

                //TODO Aian handicap block
                if (odd_in.TypeId != 51 && odd_in.TypeId != 7)
                {
                    if (oid != null && odd != null && odd_event != null)
                    {
                        Task.Run(() =>
                            SendRedisChannelSocket(channel,
                                "[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) +
                                "]", "home@HYBRIDGE", "12345", "Odd_New", oid.Result, odd_event)).ConfigureAwait(false);
                        // ZMQ = null;
                    }
                }

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public async Task SendToHybridgeSocketMessages(string message, string channel, string odd_event)
        {
            try
            {
                //TODO REDISSEND
                if (channel != null && message != null)
                {
                    Task.Run(() => SendRedisChannelSocket(channel, "[{\"message\": \"" + message + "\"}]]", "home@HYBRIDGE", "12345", "Message", null, odd_event)).ConfigureAwait(false);
                    // zmq = null;
                }

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }
        public async Task<string> EncodeUnifiedBetClearQueueElementLive(BetClearQueueElementLive UnifiedBetObject)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                if (UnifiedBetObject.MatchId != 0)
                {
                    builder.Append((UnifiedBetObject.MatchId.ToString() ?? "0000"));
                }
                else
                {
                    builder.Append("0000");
                }
                if (UnifiedBetObject.OddId != 0)
                {
                    builder.Append('|' + (UnifiedBetObject.OddId.ToString() ?? "0000"));
                }
                else
                {
                    builder.Append('|' + "0000");
                }
                if (UnifiedBetObject.TypeId != 0)
                {
                    builder.Append('|' + (UnifiedBetObject.TypeId.ToString() ?? "0000"));
                }
                else
                {
                    builder.Append('|' + "0000");
                }


                return builder.ToString();
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public async Task<string> CreateLiveOddsChannelName(long match_id, string language, string last_prefix)
        {
            try
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
#if DEBUG
                    var prefix = config.AppSettings.Get("ChannelsSecretPrefix_test");
                    // var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_test");
                    var secret = config.AppSettings.Get("ChannelsSecretKey_test");
#else
                    var prefix = Core.config.AppSettings.Get("ChannelsSecretPrefix_real");
                    // var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real");
                    var secret = Core.config.AppSettings.Get("ChannelsSecretKey_real");
#endif
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(secret + "betradar_live_odds_" + language + "_" + match_id.ToString());
                    return prefix + await ToHex(sha1.ComputeHash(bytes), false) + last_prefix; ;
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }


        public static async Task<string> ToHex(byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }

    }

    public class RedisConnectorHelper : Core
    {
        public static ConnectionMultiplexer RedisConn;
        static RedisConnectorHelper()
        {
            try
            {

                ConfigurationOptions configs = new ConfigurationOptions
                {
                    EndPoints =
                    {
                        {Core.config.AppSettings.Get("RedisServer"), int.Parse(Core.config.AppSettings.Get("RedisPort"))}
                    },
                    KeepAlive = 100,
                    AbortOnConnectFail = false,
                    ResponseTimeout = 100,
                    ConnectTimeout = 100,
                    SyncTimeout = 200,
                    Password = config.AppSettings.Get("RedisServerPassword")
                };

                RedisConn = ConnectionMultiplexer.Connect(configs);

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }
    }
}
