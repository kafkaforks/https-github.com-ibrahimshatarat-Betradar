using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.RPC;
using Zyan.Communication;
using Zyan.Communication.Protocols.Tcp;
using Zyan.Communication.Security;

namespace BetService
{
   public class Server
    {
        private readonly int _tcpPortNumber;
        private readonly string _zyanHostName;
        public Server()
        {
            //this._locator = locator;
            //var port = "8080";
            //if (!int.TryParse(port, out this._tcpPortNumber)) throw new InvalidCastException("tcpPort not int32");
            //this._zyanHostName = "127.0.0.1";

        }
        public void Start(ZyanComponentHost hostMain)
        {
            var mProtocol = new TcpDuplexServerProtocolSetup(this._tcpPortNumber, new NullAuthenticationProvider());
            mProtocol.Encryption = false;
            mProtocol.TcpKeepAliveEnabled = true;

            var asml = Assembly.GetAssembly(typeof(IBetClearingQueue));
            var types = asml.GetExportedTypes().Where(x => x.IsSerializable).ToList();

            types.AddRange(asml.GetExportedTypes().Where(x => x.IsSerializable));

            //using (var mHost = new ZyanComponentHost(this._zyanHostName, mProtocol))
            //{
            var serializationHandler = new MsgPackSerializer();
            // register types for custom serialization
            foreach (var type in types)
            {
                hostMain.SerializationHandling.RegisterSerializationHandler(type, serializationHandler);

            }
            //hostMain.DisableDiscovery();
            hostMain.ClientHeartbeatReceived += this.MHostOnClientHeartbeatReceived;
            hostMain.ClientLoggedOn += this.MHostOnClientLoggedOn;
            hostMain.ClientLoggedOff += this.MHostOnClientLoggedOff;
            hostMain.InvokeCanceled += this.MHostOnInvokeCanceled;
            //hostMain.RegisterComponent<IHelloWorldService>(() => new HelloWorldService(), ActivationType.Singleton);
            Console.Write("Server Started On : {0} - {1}", hostMain.Name, mProtocol.TcpPort, mProtocol);

            Console.ReadLine();
            //}
        }
        private void MHostOnInvokeCanceled(object sender, InvokeCanceledEventArgs invokeCanceledEventArgs)
        {
            Console.Write("Request Completed : Id : {0} ", invokeCanceledEventArgs.TrackingID);
        }

        private void MHostOnClientLoggedOff(object sender, LoginEventArgs loginEventArgs)
        {
            Console.Write("Logoff Received : {0} - {1}", loginEventArgs.ClientAddress, loginEventArgs.Timestamp);
        }

        private void MHostOnClientLoggedOn(object sender, LoginEventArgs loginEventArgs)
        {
            Console.Write("Logon Received : {0} - {1}", loginEventArgs.ClientAddress, loginEventArgs.Timestamp);
        }

        private void MHostOnClientHeartbeatReceived(object sender, ClientHeartbeatEventArgs clientHeartbeatEventArgs)
        {
            Console.Write("Heartbeart Received : {0} - {1}", clientHeartbeatEventArgs.SessionID,
                clientHeartbeatEventArgs.HeartbeatReceiveTime);
        }

    }
}
