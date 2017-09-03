using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;
using StackExchange.Redis;
using System.Messaging;

namespace Communicator
{
    public static class ConnectorGlobal
    {
        private static ConnectionMultiplexer Rconnect = RedisConnectorHelper.RedisConn;
        private static ISubscriber sub = Rconnect.GetSubscriber();


        public static  void SendRedisChannel(string message)
        {
            try
            {
                var address = Core.config.AppSettings.Get("RedisCommandChannel");

                if (sub.IsConnected(address))
                {
                    sub.Publish(address, message);
                }
                else
                {
                    sub = Rconnect.GetSubscriber();
                    sub.Publish(address, message);
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal("ERROR: " + ex.Message);
            }
        }
    }
}
