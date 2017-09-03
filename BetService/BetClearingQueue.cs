using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;
using SharedLibrary.RPC;

namespace BetService
{

    //public class BetClearingQueueLive : IBetClearingQueueLive
    //{
    //    public static QueueBetClear<BetClearQueueElementLive> ServiceQueueLive = new QueueBetClear<BetClearQueueElementLive>();
    //    public static QueueBetClear<string> StringQueueLive = new QueueBetClear<string>();
    //    public void AddQueueLive(BetClearQueueElementLive element)
    //    {

    //        ServiceQueueLive.Enqueue(element);
    //    }

    //    public ErrorReturn AddStringQueueLive(string element)
    //    {
    //        var err = new ErrorReturn();
    //        try
    //        {
    //            StringQueueLive.Enqueue(element);
    //            err.success = true;
    //            err.message = "";
    //            return err;
    //        }
    //        catch (Exception ex)
    //        {
    //            err.success = false;
    //            err.message = ex.Message;
    //            SharedLibrary.Logg.logger.Fatal(ex.Message);
    //            return err;
    //        }
           
    //    }
    //    public string GetStringQueueLive()
    //    {
    //        return StringQueueLive.Dequeue();
    //    }

    //    public BetClearQueueElementLive GetQueueLive()
    //    {
    //        return ServiceQueueLive.Dequeue();
    //    }
    //}
    public class BetClearingQueue 
    {
        public static QueueBetClear<BetClearQueueElement> ServiceQueue = new QueueBetClear<BetClearQueueElement>();
        public static QueueBetClear<string> StringQueue = new QueueBetClear<string>();
        public void AddQueue(BetClearQueueElement element)
        {

            ServiceQueue.Enqueue(element);
        }

        public void AddStringQueue(string element)
        {
            StringQueue.Enqueue(element);
        }
        public string GetStringQueue()
        {
            return StringQueue.Dequeue();
        }

        public BetClearQueueElement GetQueue()
        {
            return ServiceQueue.Dequeue();
        }
    }
}
