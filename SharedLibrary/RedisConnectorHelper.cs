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
            try
            {

            ConfigurationOptions configs = new ConfigurationOptions
            {
                EndPoints =
                    {
                        {config.AppSettings.Get("RedisServer"), int.Parse(config.AppSettings.Get("RedisPort"))}
                    },
                KeepAlive = 180,
                AbortOnConnectFail = false,
                ResponseTimeout = 1000,
                ConnectTimeout = 1000,
                SyncTimeout = 500,
                Password = config.AppSettings.Get("RedisServerPassword")
            };

            RedisConn = ConnectionMultiplexer.Connect(configs);

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }
    }
}
