using System;
using NLog.Internal;
using StackExchange.Redis;

namespace SharedLibrary
{
    public static class RedisConnectorHelper
    {
        public static ConfigurationManager config = new ConfigurationManager();
        public static ConnectionMultiplexer RedisConn;
        static RedisConnectorHelper()
        {
            ConfigurationOptions configs = new ConfigurationOptions
            {
                EndPoints =
                    {
                        {config.AppSettings.Get("RedisServer"), 6379}
                    },
                KeepAlive = 180,
                //DefaultVersion = new Version(2, 8, 8),
                AbortOnConnectFail = false,
                Password = config.AppSettings.Get("RedisServerPassword")
            };

            RedisConn = ConnectionMultiplexer.Connect(configs);
        }

        static void RedisConnect()
        {
            ConnectionMultiplexer.Connect("");
        }

        private static ConnectionMultiplexer multiConnection;
        public static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
