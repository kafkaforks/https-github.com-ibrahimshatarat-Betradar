using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using NetMQ;
using Npgsql;
using SharedLibrary;
using Timer = System.Timers.Timer;

namespace Communicator
{
    partial class BetradarComService : ServiceBase
    {
        public static Timer timerRedisChannel = null;
        public static Queue<string> redisQueue;
        public static string BetQueueName = ".\\private$\\BetQueue";
        public BetradarComService()
        {
            InitializeComponent();
        }
        public BetradarComService(string[] args)
        {
            InitializeComponent();
#if DEBUG
            TestStartupAndStop(args);
#endif
        }

        protected override void OnStart(string[] args)
        {
            Globals.redisTimerOnOff = true;
            Logg.logger.Fatal("Connector Start at UTC: " + DateTime.UtcNow.ToString());
            timerRedisChannel = new Timer();
            timerRedisChannel.Interval = 1000;
            timerRedisChannel.Elapsed += timerRedisChannel_Tick;
            timerRedisChannel.Enabled = true;
            timerRedisChannel.Start();
            // TODO: Add code here to start your service.

        }

        private void timerRedisChannel_Tick(object sender, ElapsedEventArgs e)
        {
            timerRedisChannel.Enabled = false;
            try
            {
                if (Globals.redisTimerOnOff)
                {
                    Globals.redisTimerOnOff = false;
                    BetradarComService.timerRedisChannel.Stop();
                    var command = new NpgsqlCommand("select_to_send");
                    DataSet ds = select(command);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                // Task.Factory.StartNew(()=> ConnectorGlobal.SendRedisChannel(ds.Tables[0].Rows[i][1].ToString()));
                                ConnectorGlobal.SendRedisChannel(ds.Tables[0].Rows[i][1].ToString());
                            }
                        }
                    }
                    Globals.redisTimerOnOff = true;
                }
                BetradarComService.timerRedisChannel.Start();

            }
            catch (Exception ex)
            {
                Logg.logger.Fatal("ERROR: " + ex.Message);
            }
            timerRedisChannel.Enabled = true;
        }

        //private void timerRedisChannel_Tick(object sender, ElapsedEventArgs e)
        //{
        //    try
        //    {

        //        Task.Factory.StartNew(() =>
        //        {
        //            try
        //            {

        //                if (MessageQueue.Exists(BetQueueName))
        //                {
        //                    MessageQueue queue = new MessageQueue(BetQueueName);
        //                    while (queue.CanRead)
        //                    {
        //                        var msg = queue.Receive(TimeSpan.FromMilliseconds(1000));
        //                        if (msg != null)
        //                        {
        //                            msg.Formatter = new BinaryMessageFormatter();
        //                            ConnectorGlobal.SendRedisChannel(msg.Body.ToString());
        //                        }

        //                    }
        //                }
        //            }

        //            catch (Exception ex)
        //            {
        //                if (ex.Message != "Timeout for the requested operation has expired.")
        //                {
        //                    Logg.logger.Fatal("ERROR: " + ex.Message);
        //                }
        //            }
        //        });
        //    }

        //    catch (Exception ex)
        //    {
        //        Logg.logger.Fatal("ERROR: " + ex.Message);
        //    }
        //}

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }
        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            timerRedisChannel.Stop();
            timerRedisChannel.Dispose();
            timerRedisChannel = null;
        }
        public DataSet select(NpgsqlCommand objCommand)
        {
            objCommand.Connection = connection();
            objCommand.Connection.Open();
            objCommand.CommandType = CommandType.StoredProcedure;
            var ds = new DataSet();
            try
            {
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(objCommand);
                da.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal(ex.Message);
                return null;
            }
            finally
            {
                objCommand.Connection.Close();
            }
        }
        public NpgsqlConnection connection()
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
    }
}
