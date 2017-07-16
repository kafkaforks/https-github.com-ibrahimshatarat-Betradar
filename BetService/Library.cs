using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using System.Web.Script.Serialization;
using SharedLibrary;

namespace BetService
{
    [Serializable]
    public class Library
    {
        public void get_pending_coupons()
        {

        }

        public void zeroMQ()
        {
            try
            {
                ReadMessages();
               
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

        public void ReadMessages()
        {
            try
            {
                BetClearQueueElement bet_recieve = new BetClearQueueElement();
        var msg_server = new NetMQMessage();
                msg_server = Commons.server.ReceiveMultipartMessage();
                
                foreach (var frame in msg_server)
                    bet_recieve = (BetClearQueueElement)new JavaScriptSerializer().Deserialize<BetClearQueueElement>(frame.ConvertToString());
                Console.WriteLine("Client received {0} frames", bet_recieve.MatchId);
                Commons.BetClearQueue.Enqueue(bet_recieve);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }

    }
   

    public static class Commons
    {
        public static Queue<BetClearQueueElement> BetClearQueue = new Queue<BetClearQueueElement>();
        public static ResponseSocket server = new ResponseSocket("@tcp://127.0.0.1:5556");

        static Commons()
        {
        }
    }
}
