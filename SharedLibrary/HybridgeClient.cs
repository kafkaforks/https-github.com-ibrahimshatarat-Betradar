using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;
using PhoenixSocket;

namespace SharedLibrary
{
    public static class HybridgeClient
    {
        //Channel example:   #betsoft_jackpots
        //Data example   :   {\"data\": \"Hello\"}
        private static Channel channel;



      
      
        public static void JoinChannel()
        {
            try
            {
                System.Func<int, int> reconnect = i => 10000;
                //Action<string, string, object> act = (s, s1, arg3) => Logg.logger.Fatal(reconnect);
                Dictionary<string, string> JJ = new Dictionary<string, string>();
                JJ.Add("auth[username]", Core.config.AppSettings.Get("HybridgeClientUserName"));
                JJ.Add("auth[password]", Core.config.AppSettings.Get("HybridgeClientUserPassword"));
                Socket socket = new Socket(Core.config.AppSettings.Get("HybridgeClientServer_socket"), 3000, 30000, reconnect, null, JJ);
                JObject channelOpts = new JObject();
                string ch_name = "merchant:API";
                channel = socket.Channel(ch_name, channelOpts);
                socket.Connect();
                channel.Join()
                    .Receive("ok", (jo) => Logg.logger.Fatal("ok"))
                    .Receive("error", (jo) => Logg.logger.Fatal("error"))
                    .Receive("timeout", (jo) => Logg.logger.Fatal("timeout"));
                Logg.logger.Fatal(":::::::::::::::::::::::::::: CONNECT AND JOIN ::::::::::::::::::::::::::::");
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public static void SendDataSocketPhoenix(string Channel, string Data, string username, string password, string node, string external_content_id, string bet_event)
        {
            try
            {
                var data = new JObject();
                data["identifier_slug"] = "odds";
                if (!string.IsNullOrEmpty(node))
                {
                    data["external_content_name"] = node;
                }
                if (!string.IsNullOrEmpty(external_content_id))
                {
                    data["external_content_id"] = external_content_id;
                }
               
                
                data["data"] = new JObject();
                data["data"]["channel"] = Channel;
                data["data"]["event"] = bet_event;
                data["data"]["identifier_slug"] = "odds";
                data["data"]["expires_after_ms"] = 3000;
                data["data"]["payload"] = Data;
                channel.Push("data.upsert", data);
                //.Receive("ok",
                //    (jo) => Logg.logger.Info(":::::::::::::::: Phoenix OK ::::::::::::::::"))
                //.Receive("error", (jo) =>
                //{
                //    channel.Push("data.upsert", data);
                //})
                //.Receive("timeout", (jo) =>
                //{
                //    channel.Push("data.upsert", data);
                //});
                data.RemoveAll();
                data = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public static void SendData(string channel, string data)
        {
            try
            {
                RestClient client = new RestClient(Core.config.AppSettings.Get("HybridgeClientServer_Rest"));
                RestRequest request = new RestRequest(Method.POST);
                request.Timeout = 3000;
                request.AddHeader("token", "Live Odds Test");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__api_key\"\r\n\r\nDU?JG`s:OQNrV]%ruhgLl}dpFr;z_No=uHXE{/j&[_wQBS;J)yNOBMj>7VP<\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"event\"\r\n\r\nshout\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"channel\"\r\n\r\n" + channel + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"data\"\r\n\r\n" + data + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
                client.Execute(request);
                request = null;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                //return null;
            }
            //return null;
        }



        //public IRestResponse SendData(string channel, string data)
        //{
        //    try
        //    {
        //        request.Resource = config.AppSettings.Get("HybridgeClientServer");
        //        request.AddHeader("token", "Live Odds Test");
        //        request.AddHeader("cache-control", "no-cache");
        //        request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
        //        request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"channel\"\r\n\r\n" + channel + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"data\"\r\n\r\n" + data + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__api_key\"\r\n\r\nDU?JG`s:OQNrV]%ruhgLl}dpFr;z_No=uHXE{/j&[_wQBS;J)yNOBMj>7VP<\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"event\"\r\n\r\nshout\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"\"\r\n\r\n\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
        //        IRestResponse response = client.Execute(request);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logg.logger.Fatal(ex.Message);
        //        return null;
        //    }
        //    return null;
        //}

    }


}
