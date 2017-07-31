using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using NetMQ.Sockets;
using SharedLibrary;

namespace Communicator
{
    static class Program
    {
        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
           
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new BetradarComService()
            };
            ServiceBase.Run(ServicesToRun);

            if (Environment.UserInteractive && System.Diagnostics.Debugger.IsAttached)
            {
                BetradarComService service1;
                Task.Factory.StartNew(() => service1 = new BetradarComService(args));

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
