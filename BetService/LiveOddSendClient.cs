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
    class LiveOddSendClient : Core
    {
        // public static Socket connector = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static ConnectionMultiplexer Rconnect = RedisConnectorHelper.RedisConn;

        public static ISubscriber sub = Rconnect.GetSubscriber();

        static  void SendString(string value)
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

        private void SendRedisChannelSocket(string Channel, string Data, string username, string password, string node, string external_content_id, string bet_event)
        {
            try
            {
                var build = new StringBuilder(); //Channel+'|'+Data + '|' +username + '|' +password + '|' +node + '|' +external_content_id + '|' +bet_event

                build.Append(Channel);
                build.Append('|');
                build.Append(username);
                build.Append('|');
                build.Append(password);
                build.Append('|');
                build.Append(external_content_id.Replace('|', '_'));
                build.Append('|');
                build.Append(bet_event);
                build.Append('|');
                build.Append(Data);
                SendToredis(build.ToString());
                build = null;
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
                sub.PublishAsync(address, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal("ERROR: " + ex.Message);
            }
        }

        public void SendToHybridgeLiveMenue(string Channel,  string status, string match_time, string bet_status, string match_id, string home_team_id, string match_name, string away_team_name, string match_start_date, string away_team_id, string score, string home_team_name, string tournament_name, string country_id, string country_iso, string country_name, string tournament_id,string sport_id,string sport_name)
        {
            try
            {
                //"matches": [
                var sb = new StringBuilder();
                //sb.Append("{");
                sb.Append("{");
                ////////////////////////
                sb.Append("\"");
                sb.Append("matches");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("[");
                sb.Append("{");
                /////////////////////////
                sb.Append("\"");
                sb.Append("status");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(status);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("match_time");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(match_time);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("bet_status");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(bet_status);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("match_id");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(match_id);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("home_team_id");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(home_team_id);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("match_name");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(match_name);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("away_team_name");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(away_team_name);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("match_start_date");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(match_start_date);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("away_team_id");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(away_team_id);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("score");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(score);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("home_team_name");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(home_team_name);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("tournament_name");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(tournament_name);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("country_id");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(country_id);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("country_iso");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(country_iso);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                sb.Append("\"");
                sb.Append("country_name");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(country_name);
                sb.Append("\"");
                sb.Append(",");
                ///////////////////////// 
                 sb.Append("\"");
                sb.Append("sport_id");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(sport_id);
                sb.Append("\"");
                sb.Append(",");
                /////////////////////////
                sb.Append("\"");
                sb.Append("sport_name");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(sport_name);
                sb.Append("\"");
                sb.Append(",");
                /////////////////////////
                sb.Append("\"");
                sb.Append("tournament_id");
                sb.Append("\"");
                sb.Append(":");
                sb.Append("\"");
                sb.Append(tournament_id);
                sb.Append("\"");
                ///////////////////////// 
                sb.Append("}]}");
                SendRedisChannelSocket(Channel, sb.ToString(), config.AppSettings.Get("RedisUserName"), config.AppSettings.Get("RedisPassword"), "Match","MATCH|"+match_id,"LIVEMENUEUPDATES");
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }
      
        public void SendToHybridgeSocket(long match_id, long odd_id, int? odd_eventoddsfield_typeid, string odd_name, string odd_special_odds_value, EventOddsField odd_in, string channel, string msg_event)
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
                    if (oid != null && odd != null && msg_event != null)
                    {
                        var sb = new StringBuilder();
                        //sb.Append("{");
                        sb.Append("\"");
                        sb.Append("last_update");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.last_update);
                        sb.Append("\"");
                        sb.Append(",");
                        sb.Append("\"");
                        sb.Append("odd_eventoddsfield_active");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.odd_eventoddsfield_active);
                        sb.Append("\"");
                        sb.Append(",");
                        sb.Append("\"");
                        sb.Append("odd_name");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.odd_name);
                        sb.Append("\"");
                        sb.Append(",");
                        sb.Append("\"");
                        sb.Append("odd_odd");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.odd_odd);
                        sb.Append("\"");
                        sb.Append(",");
                        sb.Append("\"");
                        sb.Append("odd_probability");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.odd_probability);
                        sb.Append("\"");
                        sb.Append(",");
                        sb.Append("\"");
                        sb.Append("odd_special_odds_value");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.odd_special_odds_value);
                        sb.Append("\"");

                        //sb.Append("}");
                        Task.Factory.StartNew(() =>
                        SendRedisChannelSocket(channel, "[{\"mid_otid_ocid_sid\": \"" + oid + "\"},{" + sb + "}]", config.AppSettings.Get("RedisUserName"), config.AppSettings.Get("RedisPassword"), "Odd", oid, msg_event)
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public void SendToHybridgeSocketNewOdd(long match_id, long odd_id, int? odd_eventoddsfield_typeid, string odd_name, string odd_special_odds_value, EventOddsField odd_in, string channel, string odd_event)
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
                        var sb = new StringBuilder();
                        //sb.Append("{");
                        sb.Append("\"");
                        sb.Append("last_update");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.last_update);
                        sb.Append("\"");
                        sb.Append(",");
                        sb.Append("\"");
                        sb.Append("odd_eventoddsfield_active");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.odd_eventoddsfield_active);
                        sb.Append("\"");
                        sb.Append(",");
                        sb.Append("\"");
                        sb.Append("odd_name");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.odd_name);
                        sb.Append("\"");
                        sb.Append(",");
                        sb.Append("\"");
                        sb.Append("odd_odd");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.odd_odd);
                        sb.Append("\"");
                        sb.Append(",");
                        sb.Append("\"");
                        sb.Append("odd_probability");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.odd_probability);
                        sb.Append("\"");
                        sb.Append(",");
                        sb.Append("\"");
                        sb.Append("odd_special_odds_value");
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(odd.odd_special_odds_value);
                        sb.Append("\"");
                        // sb.Append("}");
                        Task.Factory.StartNew(() =>
                            SendRedisChannelSocket(channel,
                                "[{\"mid_otid_ocid_sid\": \"" + oid + "\"},{" + sb +
                                "}]", config.AppSettings.Get("RedisUserName"), config.AppSettings.Get("RedisPassword"), "Odd_New", oid, odd_event)).ConfigureAwait(false);
                    }
                }

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public void SendToHybridgeSocketMessages(string message, string channel, string odd_event)
        {
            try
            {
                //TODO REDISSEND
                if (channel != null && message != null)
                {
                    Task.Factory.StartNew(() => SendRedisChannelSocket(channel, "[{\"message\": \"" + message + "\"}]]", config.AppSettings.Get("RedisUserName"), config.AppSettings.Get("RedisPassword"), "Message", "|", odd_event));
                    // zmq = null;
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public string EncodeUnifiedBetClearQueueElementLive(BetClearQueueElementLive UnifiedBetObject)
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

        public string CreateLiveOddsChannelName(long match_id, string language, string last_prefix)
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
                    return prefix +  ToHex(sha1.ComputeHash(bytes), false) + last_prefix; ;
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public static string ToHex(byte[] bytes, bool upperCase)
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
                    KeepAlive = 1000,
                    AbortOnConnectFail = false,
                    ResponseTimeout = 1000,
                    ConnectTimeout = 5000,
                    SyncTimeout = 1000,
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
