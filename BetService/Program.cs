using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using BetService.Classes.DbInsert;
using BetService.DbInsert.Betradar;
using SharedLibrary;
using Sportradar.SDK.FeedProviders.Lcoo;
using Sportradar.SDK.FeedProviders.LiveOdds.Common;

namespace BetService
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Queue<AliveEventArgs> AliveQueue = new Queue<AliveEventArgs>();


        static void Main(string[] args)
        {
            var coup = new Coupons();
            Globals.MerchantsDs = coup.selectAllMerchants(null);

            //var common = new Common();
            //var ds = common.selectotherOutcomesMarket(12016834, 20, "12016834|20|3|0000|0000|0000", "");
          


            //var entity = new BetResultEntity_test();
            // common.insertCpLcooBetclearOdds_test(entity, 11537497, "11537497|10|1|0000|0000|0000");
            // Console.WriteLine(entity.OddsType);
            // Console.ReadLine();
            //common.insertCpLcooBetclearOdds(, , )

            //var rpc_server = new RPCServer();
            // var server_message = rpc_server.StartServer();
            //var co = new Common();
            //co.LiveOddsMoveToArch(DateTime.Now.AddDays(-20));
            //var hy = new HybridgeClient();
            //await hy.SendDataSocketPhoenix("any@KLASTEST", "nothing", "klas_test", "12345");
            //hy.SendDataSocketPhoenix("any@KLASTEST","nothing", "klas_test", "12345");
            //sendToMerchantServices()


            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new CouponFinalize(),new Betradar()
            };


            if (Environment.UserInteractive && System.Diagnostics.Debugger.IsAttached)
            {
                CouponFinalize service1;
                Betradar service2;
                Task.Factory.StartNew(() => service1 = new CouponFinalize(args));
                Task.Factory.StartNew(() => service2 = new Betradar(args));

                //Console.WriteLine("Press any key to stop program");
                Console.Read();

            }
            else
            {
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
