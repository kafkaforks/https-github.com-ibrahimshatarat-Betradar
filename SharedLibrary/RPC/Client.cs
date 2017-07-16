using System;
using Zyan.Communication;

namespace SharedLibrary.RPC
{
    public class Client : Core
    {
        public IBetClearingQueue Serverproxy()
        {
            try
            {
                //var connection = new ZyanConnection("tcp://127.0.0.1:8080/Bet");
                var connectionString = "tcp://" + config.AppSettings.Get("RCP_Server") + ":" +
                                       config.AppSettings.Get("RCP_Port") + "/" +
                                       config.AppSettings.Get("RCP_CommonHostName");
                var connection = new ZyanConnection(connectionString);
                return connection.CreateProxy<IBetClearingQueue>();
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }


        }
        //public IBetClearingQueueLive ServerproxyLive()
        //{
        //    try
        //    {
        //        //var connection = new ZyanConnection("tcp://127.0.0.1:8080/Bet");
        //        var connectionString = "tcp://" + config.AppSettings.Get("RCP_Server") + ":" +
        //                               config.AppSettings.Get("RCP_Port") + "/" +
        //                               config.AppSettings.Get("RCP_CommonHostName") + "_Live";
        //        var connection = new ZyanConnection(connectionString);
        //        return connection.CreateProxy<IBetClearingQueueLive>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logg.logger.Fatal(ex.Message);
        //        return null;
        //    }

        //}
    }
}
