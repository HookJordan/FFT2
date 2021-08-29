using Common;
using Common.Networking;
using Common.Security;
using Common.Security.Cryptography;
using System;
using System.IO;

namespace fft_client
{
    class Program
    {
        const string APP_VERSION = "1.0.0";

        static int Main(string[] args)
        {
            Console.Title = "FFT2 CLI";
            // Set debugging info
            Logger.SetLogLevel(LogLevel.Debug, true);

            PrintStartup();

            if (args.Length != 0)
            {
                // CLI arguments provided
                return ParseArgs(args);
            }
            else
            {
                // No cli provided - print usage (instructions on how to use the app)
                PrintUsage();
                return -1;
            }
        }

        static void PrintUsage()
        {
            Console.WriteLine("Invalid arguements provided.");
            Console.WriteLine("\nUsage:\tfft-client {IP_ADDRESS|DNS}:{PORT} {PASSWORD} {AES|RC4|XOR}");
            Console.WriteLine("\nex: \tfft-client -c 127.0.0.1:15056 \"SECRET_PASSWORD\"");
        }


        static void PrintStartup()
        {
            Console.Clear();
            Console.WriteLine("FFT2 - Command Line Interface");
            Console.WriteLine($"Version: {APP_VERSION}\n");
        }

        static int ParseArgs(string[] args)
        {
            if (args.Length !=2 && args.Length != 3)
            {
                PrintUsage();
                return -1;
            }
            else
            {
                string endpoint = args[0].ToLower();
                string password = args[1];
                string encryptionMode = args.Length == 3 ? args[2].ToLower() : "";

                if (encryptionMode != "aes" && encryptionMode != "rc4" && encryptionMode != "xor")
                {
                    Console.WriteLine($"Invalid encryption mode provided: '{encryptionMode}'.\nValid options: AES | RC4 | XOR.");
                    return -1;
                }


                try
                {
                    // Finish parsing information
                    string[] epInfo = endpoint.Split(":");
                    string dns = epInfo[0];
                    int port = int.Parse(epInfo[1]);
                    CryptoServiceAlgorithm alg = CryptoServiceAlgorithm.Disabled;

                    // Set crypto mode
                    if (encryptionMode == "aes")
                        alg = CryptoServiceAlgorithm.AES;
                    else if (encryptionMode == "rc4")
                        alg = CryptoServiceAlgorithm.RC4;
                    else if (encryptionMode == "xor")
                        alg = CryptoServiceAlgorithm.XOR;

                    // Create new client
                    Client client = new Client(Protocol.FFTSI, dns, port, Hashing.SHA(password), alg);

                    // Listen to events
                    client.Disconnected += Client_Disconnected;
                    client.PacketReceived += Client_PacketReceived;
                    client.Ready += Client_Ready;

                    // Connect
                    client.Connect(1024 * 1024); // TODO: Make buffer size an optional parameter

                    if (client.Connected)
                    {
                        System.Diagnostics.Process.GetCurrentProcess().WaitForExit();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    return -1;
                }
            }

            return 0;
        }

        private static void Client_Ready(Client client)
        {
            Console.Title = $"FFT2 CLI - Connected: {client.IP}:{client.Port}:{client.CryptoServiceAlgorithm}";
            Logger.Info($"Connection established: {client}");
            // Get drive listing
            client.Transmit(new Packet(PacketHeader.DrivesGet));
        }

        private static void Client_PacketReceived(Client client, Packet packet)
        {
            using (MemoryStream ms = new MemoryStream(packet.Payload))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    switch (packet.PacketHeader)
                    {
                        case PacketHeader.DrivesGet:
                            HandleDriveList(packet, br);
                            break;
                        default:
                            Logger.Error($"Unsupport CLI packet received: {packet}");
                            break;
                    }
                }
            }
        }

        private static void Client_Disconnected(Client client)
        {
            client.Dispose();
        }

        private static void HandleDriveList(Packet packet, BinaryReader reader)
        {
            // if (clear) Console.Clear();

            // How many drives
            int drives = reader.ReadInt32();

            Logger.Info("#\tNAME\tFORMAT\tTYPE");
            for (int i = 0; i < drives; i++)
            {
                string name = reader.ReadString(), 
                    label = reader.ReadString(),
                    totalSize = reader.ReadString(),
                    usedSpace = reader.ReadString(),
                    freeSpace = reader.ReadString(),
                    format = reader.ReadString(),
                    driveType = reader.ReadString();

                reader.ReadString(); // is primary drive?

                Logger.Info($"[{i + 1}]\t{name}\t{format}\t{driveType}");
            }

            Console.WriteLine("\n\nPlease select a drive to enter: ");
            Console.ReadKey().KeyChar.ToString();

            // Console.WriteLine("ENTER DRIVE: " + option);
        }
    }
}
