using Common;
using Common.IO;
using Common.Networking;
using System;
using System.Threading;

namespace fft_server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Full system logging
            Logger.SetLogLevel(LogLevel.Debug, true);

            // Port, Password
            using (Server server = new Server(15056, "https://jordanhook.ca"))
            {
                string passPreview = server.Password.Substring(0, 8);
                Logger.Info($"Set server password={passPreview}...");

                // Setup events
                server.NewClient += Server_NewClient;

                server.Listen();

                while(true)
                {
                    Thread.Sleep(1);
                }
            }
        }

        private static void Server_NewClient(Server server, Client client)
        {
            Explorer explorer = new Explorer(client);
            client.PacketReceived += Client_PacketReceived;
            client.Connect();
        }

        private static void Client_PacketReceived(Client client, Packet packet)
        {
            if (packet.PacketHeader == PacketHeader.Ping)
            {
                packet.PacketHeader = PacketHeader.Pong;
                client.Transmit(packet);
            }
        }

    }
}
