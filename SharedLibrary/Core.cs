using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using NLog.Internal;
using RestSharp;
using SharedLibrary.RPC;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;

namespace SharedLibrary
{
    public class Core
    {
        // static Socket client = new Socket(SocketType.Stream, ProtocolType.IP);
        public static ConfigurationManager config = new ConfigurationManager();
        public static ManualResetEvent ManualReset = new ManualResetEvent(false);
        public static long InCount;
        public static long OutCount;
        private static ClientWebSocket pusher = new ClientWebSocket();
        public static ClientWebSocket client;

       

        public Core()
        {
            try
            {


                //if (client.Connected == false)
                //    client.Connect("172.16.18.8", 22824);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public void sendToPusher(string jsonObject)
        {
            try
            {


                var client = new RestClient("http://195.244.59.34/api/messages/");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "ef4f0501-62e3-d06b-3730-047006f9686b");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"channel\"\r\n\r\n!f916dd57b852e938f6b32823cdf18a5b7bcad1c9@POKERKLAS\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"asdasd\"\r\n\r\n" + jsonObject + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__api_key\"\r\n\r\nDU?JG`s:OQNrV]%ruhgLl}dpFr;z_No=uHXE{/j&[_wQBS;J)yNOBMj>7VP<\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"event\"\r\n\r\nshout\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"message_type\"\r\n\r\nsuccess\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);


                //client.Send(jsonObject);
                //   byte[] outStream = System.Text.Encoding.ASCII.GetBytes(@"{\""channel\"": \""!574bd1b661ee473713174305b4e336ba91e8963b@POKERKLAS\"", \""ref\"": null, \""payload\"": {\""message\"": \""{\""data\"": \""Hello\""} "", \""event\"": \""shout\"", \""channel\"": \""!574bd1b661ee473713174305b4e336ba91e8963b @POKERKLAS\""}, \""event\"": \""shout\""}");
                ////client.Send(outStream);
                //var segment = new ArraySegment<byte>(outStream);
                //client.SendAsync(segment,WebSocketMessageType.Text, true,CancellationToken.None);
                //var client = new RestClient("http://195.244.59.34/api/messages/");
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("postman-token", "45ef11f5-1e1e-ac7b-94c2-371078b44a98");
                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                //request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"channel\"\r\n\r\n!574bd1b661ee473713174305b4e336ba91e8963b@POKERKLAS\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"message\"\r\n\r\nTest.\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__api_key\"\r\n\r\nDU?JG`s:OQNrV]%ruhgLl}dpFr;z_No=uHXE{/j&[_wQBS;J)yNOBMj>7VP<\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"event\"\r\n\r\nshout\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"message_type\"\r\n\r\nsuccess\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);




                //var client = new RestClient("http://195.244.59.34/api/messages/");
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("postman-token", "2a446b22-b285-4594-d70b-e2975cb7f933");
                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                //request.AddParameter(jsonObject, ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);





                //var encoded = Encoding.UTF8.GetBytes(jsonObject);
                //var uri = new Uri(host);
                //pusher..Connect(uri, CancellationToken.None);
                //var segment = new ArraySegment<byte>(encoded);
                //var result = pusher.Send(segment, WebSocketMessageType.Text, false, CancellationToken.None);


            }
            catch (Exception ex)
            {

                Logg.logger.Fatal(ex.Message);
                // client.Close();
            }

        }

        public void sendToWebSocket()
        {
            try
            {
                //foreach (var lang in NameDictionary)
                //{
                //    var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real");
                //    SendToHybridgeSocket(entity.EventHeader.Id, odd.Id, val.TypeId, "", odd.SpecialOddsValue, val, CreateLiveOddsChannelName(entity.EventHeader.Id, lang.Key, last_prefix));
                //}

                //foreach (var lang in NameDictionary)
                //{
                //    var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real2");
                //    SendToHybridgeSocket(entity.EventHeader.Id, odd.Id, val.TypeId, "", odd.SpecialOddsValue, val, CreateLiveOddsChannelName(entity.EventHeader.Id, lang.Key, last_prefix));
                //}
            }
            catch (Exception)
            {
                    
               
            }
        }

        public void sendToRpc(string mid)
        {
            try
            {
                var client = new Client();
                var proxy = client.Serverproxy();
                proxy.AddStringQueue(mid);
            }
            catch (Exception ex)
            {
              Logg.logger.Fatal(ex.Message);
            }
            
        }

        //public void sendToRpcLive(string mid)
        //{
        //    try
        //    {
        //        var client = new Client();
        //        var proxy = client.ServerproxyLive();
        //        var ret = proxy.AddStringQueueLive(mid);
        //        if (!ret.success)
        //        {
        //            Globals.Queue_BetClearQueueElementLive.Enqueue(DecodeUnifiedBetClearQueueElementLive(mid));
        //            SharedLibrary.Logg.logger.Fatal("SEND TO PROXY ERROR: " + ret.message);
        //        }
        //        else
        //        {
        //            SharedLibrary.Logg.logger.Debug("SEND "+ mid + " TO PROXY DONE !!!!!!!!!!!!!!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logg.logger.Fatal(ex.Message);
        //    }
        //}

        public static Queue<Globals.Rollback> CloneRollbackQueue(Queue<Globals.Rollback> ToQueue, Queue<Globals.Rollback> FromQueue)
        {
            var count = FromQueue.Count;
            for (int i = 0; i < count; i++)
            {
                ToQueue.Enqueue(FromQueue.Dequeue());
            }
            return ToQueue;
        }

        public static void pingNotipier()
        {
            while (true)
            {
                try
                {
                    byte[] outStream = Encoding.ASCII.GetBytes("ping\n");
                    //client.Send(outStream);
                    Thread.Sleep(5000);
                    Logg.logger.Fatal("=====================================================================================================");
                }
                catch (Exception)
                {

                }

            }
        }

        public Globals.Rollback SetRollback(long RowId, Globals.Tables Table, Globals.TransactionTypes Transaction)
        {
            var rollbackObject = new Globals.Rollback();
            try
            {
                rollbackObject.RowId = RowId;
                rollbackObject.TableId = (int)Table;
                rollbackObject.TransactionType = (int)Transaction;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
            return rollbackObject;
        }

        public BetClearQueueElement DecodeUnifiedBetClearQueueElement(string UnifiedBetCode)
        {
            try
            {
                var array = UnifiedBetCode.Split('|');
                var element = new BetClearQueueElement();
                for (int i = 0; i < array.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (array[i] != "0000")
                            {
                                long number;
                                bool result = Int64.TryParse(array[i], out number);
                                if (result)
                                {
                                    element.MatchId = number;
                                }
                            }
                            break;
                        case 1:
                            if (array[i] != "0000")
                            {
                                long number;
                                bool result = Int64.TryParse(array[i], out number);
                                if (result)
                                {
                                    element.OddsId = number;
                                }
                            }
                            break;
                        case 2:
                            if (array[i] != "0000")
                            {
                                int number;
                                bool result = Int32.TryParse(array[i], out number);
                                if (result)
                                {
                                    element.OutcomeId = number;
                                }
                            }
                            break;
                        case 3:
                            if (array[i] != "0000")
                            {
                                element.SpecialBetValue = array[i];
                            }
                            break;
                        case 4:
                            if (array[i] != "0000")
                            {
                                element.PlayerId = array[i];
                            }
                            break;
                        case 5:
                            if (array[i] != "0000")
                            {
                                element.TeamId = array[i];
                            }
                            break;
                    }
                }
                return element;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public string EncodeUnifiedBetClearQueueElement(BetClearQueueElement UnifiedBetObject)
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
                if (UnifiedBetObject.OddsId != 0)
                {
                    builder.Append('|' + (UnifiedBetObject.OddsId.ToString() ?? "0000"));
                }
                else
                {
                    builder.Append('|' + "0000");
                }
                if (UnifiedBetObject.OutcomeId != 0)
                {
                    builder.Append('|' + (UnifiedBetObject.OutcomeId.ToString() ?? "0000"));
                }
                else
                {
                    builder.Append('|' + "0000");
                }
                builder.Append('|' + (UnifiedBetObject.SpecialBetValue ?? "0000") + '|' +
                               (UnifiedBetObject.PlayerId ?? "0000") + '|' + (UnifiedBetObject.TeamId ?? "0000"));

                return builder.ToString();
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public void SendToHybridgeSocket(long match_id, long odd_id, int? odd_eventoddsfield_typeid, string odd_name, string odd_special_odds_value, EventOddsField odd_in,string channel,string msg_event)
        {
            var oddUnique = new BetClearQueueElementLive();
            
            try
            {
                oddUnique.MatchId = match_id;
                oddUnique.OddId = odd_id;
                oddUnique.TypeId = odd_eventoddsfield_typeid;
                var oid = EncodeUnifiedBetClearQueueElementLive(oddUnique);
                var odd = new SocketOdd();
                odd.odd_eventoddsfield_active = odd_in.Active;
                odd.odd_name = odd_name;
                odd.odd_odd = odd_in.Value.ToString();
                odd.odd_special_odds_value = odd_special_odds_value;
                //TODO UserName and Password must be dynamic for socket connection
                RedisQueue.Send_Redis_Channel(channel,
                    "[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) + "]",
                    "home@HYBRIDGE", "12345", "Odd", oid, msg_event);
                //Task.Factory.StartNew(()=>HybridgeClient.SendDataSocketPhoenix(channel, "[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) + "]", "home@HYBRIDGE", "12345","Odd",oid,msg_event));
                //clientSock.SendData(channel,"[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) + "]");
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public void SendToHybridgeSocketNewOdd(long match_id, long odd_id, int? odd_eventoddsfield_typeid, string odd_name, string odd_special_odds_value, EventOddsField odd_in, string channel,string odd_event)
        {
            var oddUnique = new BetClearQueueElementLive();
            try
            {
                oddUnique.MatchId = match_id;
                oddUnique.OddId = odd_id;
                oddUnique.TypeId = odd_eventoddsfield_typeid;
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

                //TODO 
                RedisQueue.Send_Redis_Channel(channel, "[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) + "]", "home@HYBRIDGE", "12345", "Odd_New", oid, odd_event);
                //Task.Factory.StartNew(() => HybridgeClient.SendDataSocketPhoenix(channel, "[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) + "]", "home@HYBRIDGE", "12345","Odd_New",oid,odd_event));
                //clientSock.SendData(channel,"[{\"mid_otid_ocid_sid\": \"" + oid + "\"}," + new JavaScriptSerializer().Serialize(odd) + "]");
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public void SendToHybridgeSocketMessages(string message, string channel,string odd_event)
        {
            try
            {
                //TODO
                RedisQueue.Send_Redis_Channel(channel, "[{\"message\": \"" + message + "\"}]]", "home@HYBRIDGE", "12345", "Message", null, odd_event);
                //Task.Factory.StartNew(() => HybridgeClient.SendDataSocketPhoenix(channel, "[{\"message\": \"" + message + "\"}]]", "home@HYBRIDGE", "12345","Message",null,odd_event));
                //clientSock.SendData(channel, "[{\"message\": \"" + message + "\"}]]");
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

        public BetClearQueueElementLive DecodeUnifiedBetClearQueueElementLive(string UnifiedBetCode)
        {
            try
            {
                var array = UnifiedBetCode.Split('|');
                var element = new BetClearQueueElementLive();
                for (int i = 0; i < array.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (array[i] != "0000")
                            {
                                long number;
                                bool result = Int64.TryParse(array[i], out number);
                                if (result)
                                {
                                    element.MatchId = number;
                                }
                            }
                            break;
                        case 1:
                            if (array[i] != "0000")
                            {
                                long number;
                                bool result = Int64.TryParse(array[i], out number);
                                if (result)
                                {
                                    element.OddId = number;
                                }
                            }
                            break;
                        case 2:
                            if (array[i] != "0000")
                            {
                                int number;
                                bool result = Int32.TryParse(array[i], out number);
                                if (result)
                                {
                                    element.TypeId = number;
                                }
                            }
                            break;
                    }
                }
                return element;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
        }

        public string CreateLiveOddsChannelName(long match_id, string language,string last_prefix)
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
                     var prefix = config.AppSettings.Get("ChannelsSecretPrefix_real");
                    // var last_prefix = config.AppSettings.Get("ChannelsSecretPrefixLast_real");
                    var secret = config.AppSettings.Get("ChannelsSecretKey_real");
#endif
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(secret+"betradar_live_odds_" + language + "_" + match_id.ToString());
                    return prefix + ToHex(sha1.ComputeHash(bytes), false)+last_prefix;
                }

            }
            catch (Exception ex)
            {
                return null;
                throw;
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
}
