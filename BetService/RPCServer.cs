using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;
using SharedLibrary.RPC;
using Zyan.Communication;

namespace BetService
{
   public class RPCServer:Core
    {
        
       public string StartServer()
       {
            try
           {
                var host = new ZyanComponentHost(config.AppSettings.Get("RCP_CommonHostName"), Convert.ToInt32(config.AppSettings.Get("RCP_Port")));
                host.EnableDiscovery(Convert.ToInt32(config.AppSettings.Get("RCP_Port")));
                host.DiscoverableUrl = "tcp://"+config.AppSettings.Get("RCP_Server") +":"+config.AppSettings.Get("RCP_Port") +"/"+config.AppSettings.Get("RCP_CommonHostName");
                host.RegisterComponent<IBetClearingQueue,BetClearingQueue>();

                //var hostLive = new ZyanComponentHost(config.AppSettings.Get("RCP_CommonHostName"), Convert.ToInt32(config.AppSettings.Get("RCP_Port")));
                //hostLive.EnableDiscovery(Convert.ToInt32(config.AppSettings.Get("RCP_Port")));
                //hostLive.DiscoverableUrl = "tcp://" + config.AppSettings.Get("RCP_Server") + ":" + config.AppSettings.Get("RCP_Port") + "/" + config.AppSettings.Get("RCP_CommonHostName")+"_Live";
                //hostLive.RegisterComponent<IBetClearingQueueLive, BetClearingQueueLive>();
                //var x = new Server();
                //x.Start(host);
                return "Server Started!";
           }
           catch (Exception ex)
           {
               Logg.logger.Fatal(ex.Message);
               return "Server did not Start!";
           }
       }

    }

}
