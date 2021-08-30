using Common;
using Common.IO;
using Common.Networking;
using System.Threading;

namespace fft_server
{
    class Program
    {
        private static Configuration config;
        static void Main(string[] args)
        {
            config = Configuration.FromFile("config.json");
            // Full system logging
            Logger.SetLogLevel(config.LogLevel, true);

            // Port, Password
            using (Server server = new Server(config.Port, config.Password, config.Password))
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
            if (client.Protocol == Protocol.FFTSI)
            {
                Explorer explorer = new Explorer(client, config.ProtectedDirectories.ToArray());
                client.PacketReceived += Client_PacketReceived;
                client.Connect(config.MaxBufferSize);
            }
            else
            {
                FileTransferManager ftm = new FileTransferManager(client, config.MaxBufferSize);
                client.Connect(config.MaxBufferSize);
            }
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
