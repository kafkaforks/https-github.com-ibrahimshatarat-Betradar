using System;
using System.Messaging;

namespace SharedLibrary
{
   public class MSMQ_Handler
    {
        private MessageQueue iQueue;
        private Message iMessage;

        public MSMQ_Handler()
        {
            iMessage = new Message();
            iMessage.Recoverable = true;
        }

        public Boolean queueExists(string fifo)
        {
            if (MessageQueue.Exists(@".\Private$\" + fifo + ""))
            {
                return true;// there is a queue like that
            }
            return false; // there is no queue like that in msmq server
        }

        public void createQueue(string fifo)
        {
            if (!queueExists(fifo))
            {
                iQueue = MessageQueue.Create(@".\Private$\" + fifo + "");
                iQueue.DefaultPropertiesToSend.Recoverable = true;
            }
            else
            {
                Console.WriteLine("" + fifo + " exists!");
            }
        }

        public MessageQueue searchForMessageQueue(string fifo)
        {
            string withPrefix = "private$\\";
            withPrefix += fifo;
            MessageQueue[] QueueList =
                MessageQueue.GetPrivateQueuesByMachine(".");
            foreach (MessageQueue queueItem in QueueList)
            {
                if (queueItem.QueueName.Equals(withPrefix))
                    iQueue = queueItem;
            }
            return iQueue;
        }

        public void EnqueueMessageToFifo<T>(string fifo, T datas)
        {
            try
            {
                iQueue = searchForMessageQueue(fifo);
                iMessage.Formatter = new BinaryMessageFormatter();
                iMessage.Body = datas;
                iMessage.Label = "Message from " + iQueue + "";
                iQueue.Send(iMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public T DequeueMessageFromFifo<T>(string fifo)
        {
            try
            {
                iQueue = searchForMessageQueue(fifo);
                iMessage = iQueue.Receive();
                iMessage.Formatter = new BinaryMessageFormatter();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (T)Convert.ChangeType(iMessage.Body, typeof(T));
        }

        public object DequeueMessageFromFifo(string fifo)
        {
            try
            {
                iQueue = searchForMessageQueue(fifo);
                iMessage = iQueue.Receive();
                iMessage.Formatter = new BinaryMessageFormatter();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return iMessage.Body;
        }

        public BetQueue<T> insertObjectsToMemoryQueue<T>(string fifo, BetQueue<T> memoryQueue)
        {
            iQueue = searchForMessageQueue(fifo);
            Message[] msgs = iQueue.GetAllMessages();

            foreach (Message msg in msgs)
            {

                msg.Formatter = new BinaryMessageFormatter();
                memoryQueue.Enqueue((T)Convert.ChangeType(msg.Body, typeof(T)));
            }

            return memoryQueue;
        }

        public int getQueueElementNumber(string fifo)
        {
            iQueue = searchForMessageQueue(fifo);
            Message[] msgs = iQueue.GetAllMessages();
            return msgs.Length;
        }




    }
}

