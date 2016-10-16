﻿using Sean.Shared.Comms;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Sean.Shared;
using System.Collections.Generic;

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

		        Thread.Sleep (2000);
                ClientConnection.BroadcastMessage(new Message()
                {
                    WorldMapRequest = new WorldMapRequestMessage()
                });
        
				List<string> sent = new List<string>();
				while(true)
				{
					int x = Global.LookingAt.X / Global.CHUNK_SIZE;
					int z = Global.LookingAt.Z / Global.CHUNK_SIZE;
					string hash = $"{x},{z}";
					if (!sent.Contains(hash))
					{
						sent.Add(hash);
						ClientConnection.BroadcastMessage(new Message()
						{
							MapRequest = new MapRequestMessage()
							{
								Coords = new ChunkCoords(x,z)
							}
						});
						}
					Thread.Sleep (2000);
				}
			
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
            if (msg.WorldMapResponse != null)
            {
                MapManager.SetWorldMap(msg);
            }
        }

    }

}
