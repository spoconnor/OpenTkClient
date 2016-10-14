using Sean.Shared.Comms;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Sean.Shared;

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

				int CHUNK_SIZE = 32; // TODO - move
				Position lookingAt = new Position(2000, 100, 1000);  // TODO - duplicate put this somewhere
				int x = lookingAt.X / CHUNK_SIZE;
				int z = lookingAt.Z / CHUNK_SIZE;

                ClientConnection.BroadcastMessage(new Message()
                {
                    MapRequest = new MapRequestMessage()
                    {
							Coords = new Sean.Shared.ChunkCoords(x,z)
                    }
                });
                System.Threading.Thread.Sleep (10000);
                ClientConnection.BroadcastMessage(new Message()
                {
                    MapRequest = new MapRequestMessage()
                    {
							Coords = new Sean.Shared.ChunkCoords(x+1,z)
                    }
                });
                System.Threading.Thread.Sleep (10000);
                ClientConnection.BroadcastMessage(new Message()
                {
                    MapRequest = new MapRequestMessage()
                    {
							Coords = new Sean.Shared.ChunkCoords(x,z+1)
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
            Console.WriteLine($"Processing response... {msg.ToString()}");
            if (msg.Map != null)
            {
                MapManager.AddChunk (msg);
            }
        }

    }

}
