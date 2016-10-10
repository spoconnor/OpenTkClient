using Sean.Shared.Comms;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OpenTkClient
{
    public class Comms
    {
        private static Thread thread;

        public static void Run()
        {
            thread = new Thread(new ThreadStart(Start));
            thread.Start();
        }

        private static void Start()
        {
            Console.WriteLine ("Hello World!");

            try
            {
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8084);

                TcpClient client = new TcpClient();
                Console.WriteLine ("Connecting...");
                client.Connect(remoteEP);

                var connection = ClientConnection.CreateClientConnection(client, ProcessMessage);
                connection.StartClient();

                ClientConnection.BroadcastMessage(new Message()
                {
                    Ping = new PingMessage()
                    {
                        Message = "Hi"
                    }
                });

                ClientConnection.BroadcastMessage(new Message()
                {
                    MapRequest = new MapRequestMessage()
                    {
                        Coords = new Sean.Shared.ChunkCoords(100, 100)
                    }
                });

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught in ServerSocketListener - {0}", e.ToString());
            }
        }

        public static void ProcessMessage(Guid clientId, Message msg)
        {
            Console.WriteLine($"Processing response...");
        }

    }

}
