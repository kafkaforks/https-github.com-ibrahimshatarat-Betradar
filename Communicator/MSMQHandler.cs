using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Communicator
{
  public  class MSMQHandler
    {
        
      public void GetMessages(string BetQueueName)
      {
          try
          {
                if (MessageQueue.Exists(BetQueueName))
                {
                    MessageQueue queue = new MessageQueue(BetQueueName);
                    while (queue.CanRead)
                    {
                        var msg = queue.Receive(TimeSpan.FromMilliseconds(10000));
                        if (msg != null)
                        {
                            msg.Formatter = new BinaryMessageFormatter();
                            ConnectorGlobal.SendRedisChannel(msg.Body.ToString());
                        }

                    }
                }
            }
          catch (Exception)
          {
                
              throw;
          }
      }
    }
}
