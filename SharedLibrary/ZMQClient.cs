using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using SharedLibrary;
using StackExchange.Redis;

namespace SharedLibrary
{
    public class ZMQClient
    {
        private static ConnectionMultiplexer Rconnect = RedisConnectorHelper.RedisConn;
        private static ISubscriber sub = Rconnect.GetSubscriber();
        public static ZMQClient Instance = new ZMQClient();

        public async Task SendOut(string Channel, string Data, string username, string password, string node,
            string external_content_id, string bet_event)
        {
  
            await SendRedisChannelZmq(Channel, Data, username, password, node, external_content_id, bet_event);

        }

        public async Task SendOutQueue(string message)
        {
            await SenMQueue(message);
        }

        private async Task SendRedisChannelZmq(string Channel, string Data, string username, string password, string node, string external_content_id, string bet_event)
        {
            try
            {
                var address = Core.config.AppSettings.Get("RedisCommandChannel");
                var data = new JObject();
                if (!String.IsNullOrEmpty(node))
                {
                    //  data["external_content_name"] = node;
                }
                if (!String.IsNullOrEmpty(external_content_id))
                {
                    // data["external_content_id"] = external_content_id;
                }
                data["auth"] = new JObject();
                data["event"] = "data.upsert";
                data["data"] = new JObject();
                data["auth"]["username"] = Core.config.AppSettings.Get("HybridgeClientUserName");
                data["auth"]["password"] = Core.config.AppSettings.Get("HybridgeClientUserPassword");
                data["data"]["channel"] = Channel;
                data["data"]["event"] = bet_event;
                data["data"]["payload"] = Data;
                
                
                
                //await SendToredis(data.ToString());

                // var command = new NpgsqlCommand("insert_cp_send");
                // command.Parameters.AddWithValue("p_message", NpgsqlDbType.Text, data.ToString());
                // await insert(command);
                //Globals.LiveOddsQueue.Enqueue(data.ToString());
                //SenMQueue(data.ToString());
                data = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }
        private async Task SendToredis(string message)
        {
            try
            {
                var address = Core.config.AppSettings.Get("RedisCommandChannel");

                if (sub.IsConnected(address))
                {
                    var res = await sub.PublishAsync(address, message);
                }
                else
                {
                    sub = Rconnect.GetSubscriber();
                    var res = await sub.PublishAsync(address, message);
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal("ERROR: " + ex.Message);
            }
        }
        delegate string MethodDelegate(int iCallTime, out int iExecThread);
        private async Task SenMQueue(string message)
        {
            try
            {
                MessageQueue mq;
                if (!MessageQueue.Exists(Globals.BetQueueName))
                {
                    mq = MessageQueue.Create(Globals.BetQueueName);
                }
                else
                {
                    mq = new MessageQueue(Globals.BetQueueName);
                }
                Message msg = new Message
                {
                    Formatter = new BinaryMessageFormatter(),
                    Body = message,
                    Label = "QueueMessage"
                };

                await Task.Run(() => mq.Send(msg));
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
            }
        }
        public async Task<NpgsqlConnection> connection()
        {
            var con = new NpgsqlConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                {

                    var connectionBuilder = new NpgsqlConnectionStringBuilder();
                    connectionBuilder.Host = Core.config.AppSettings.Get("DB_Host");
                    connectionBuilder.Port = int.Parse(Core.config.AppSettings.Get("DB_Port"));
                    connectionBuilder.Database = Core.config.AppSettings.Get("DB_Database");
                    connectionBuilder.Username = Core.config.AppSettings.Get("DB_Username");
                    connectionBuilder.Password = Core.config.AppSettings.Get("DB_Password");
                    connectionBuilder.Timeout = 300;
                    connectionBuilder.Pooling = true;
                    connectionBuilder.CommandTimeout = 300;
                    con = new NpgsqlConnection(connectionBuilder.ConnectionString);
                    return con;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }

        }
        public async Task<long> insert(NpgsqlCommand objCommand)
        {
            long errorNumber = -1;
            long result = -20;
            object one;
            objCommand.CommandType = CommandType.StoredProcedure;
            try
            {

                objCommand.Connection = await connection();
                //objCommand.CommandTimeout = 5;
                if (objCommand.Connection != null) await objCommand.Connection.OpenAsync();
                one = await objCommand.ExecuteScalarAsync();
                bool successfullyParsed = long.TryParse(one.ToString(), out result);
                long val = 0;
                if (successfullyParsed)
                {
                    if (result != null && long.TryParse(result.ToString(), out val))
                    {
                        if (val > 0)
                        {
                            return val;
                        }
                        else
                        {
                            errorNumber = val;
                            throw new DataException();
                        }
                    }
                }

                return -1;
            }
            catch (Exception ex)
            {
                StackTrace trace = new StackTrace(true);
                var frame = trace.GetFrame(1);
                var altMessage = "  Error#: " + errorNumber.ToString() + "  METHOD: " + frame.GetMethod().Name + "  LINE:  " + frame.GetFileLineNumber();
                Logg.logger.Fatal(ex.Message + altMessage);
                // Task.Factory.StartNew(() => Globals.Queue_Errors.Enqueue(objCommand));
                return -1;
            }
            finally
            {
                objCommand.Connection.Close();
            }
        }

    }
}
