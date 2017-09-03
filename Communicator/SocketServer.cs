using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;
using StackExchange.Redis;

namespace Communicator
{
    class SocketServer
    {
        private static ConnectionMultiplexer Rconnect = RedisConnectorHelper.RedisConn;
        private static ISubscriber sub = Rconnect.GetSubscriber();
        public  async void ReceivedString(IAsyncResult asyncResult)
        {
            Connection connection = (Connection)asyncResult.AsyncState;
            try
            {
                int bytesReceived = connection.Socket.EndReceive(asyncResult);

                if (bytesReceived > 0)
                {
                    int length = BitConverter.ToInt32(connection.Buffer, 0);

                    byte[] buffer;
                    if (length > connection.Buffer.Length)
                        buffer = new byte[length];
                    else
                        buffer = connection.Buffer;

                    int index = 0;
                    int remainingLength = length;
                    do
                    {
                        bytesReceived = connection.Socket.Receive(buffer, index, remainingLength, SocketFlags.None);
                        index += bytesReceived;
                        remainingLength -= bytesReceived;
                    }
                    while (bytesReceived > 0 && remainingLength > 0);

                    if (remainingLength > 0)
                    {
                        Logg.logger.Fatal("Connection was closed before entire string could be received");
                    }
                    else
                    {
                         
                        Task.Run(() =>  SendToredis(connection.Encoding.GetString(buffer, 0, length))).ConfigureAwait(false);
                        //Task.Factory.StartNew(()=> SendToredis(connection.Encoding.GetString(buffer, 0, length)));
                    }

                    connection.WaitForNextString(ReceivedString);
                }
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal("ERROR: " + ex.Message);
            }
        }
        public  async Task SendToredis(string message)
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

  
    }

    class Recievers
    {
        public void Recieve()
        {
            var server = new SocketServer();
            try
            {
                Connection connection;
                Console.WriteLine("The beginning");
                using (Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    Console.WriteLine("listener");
                    listener.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6767));
                    listener.Listen(1);
                    connection = new Connection(listener.Accept());
                    Console.WriteLine("Connection established.");
                }

                connection.WaitForNextString(server.ReceivedString);
            }
            catch (Exception ex)
            {
                Logg.logger.Fatal("ERROR: " + ex.Message);
            }
        }
    }

    class Connection
    {
        public Socket Socket { get; private set; }
        public byte[] Buffer { get; private set; }
        public Encoding Encoding { get; private set; }

        public Connection(Socket socket)
        {
            this.Socket = socket;
            this.Buffer = new byte[2048];
            this.Encoding = Encoding.UTF8;
        }

        public void WaitForNextString(AsyncCallback callback)
        {
            this.Socket.BeginReceive(this.Buffer, 0, 4, SocketFlags.None, callback, this);
        }
    }





}
