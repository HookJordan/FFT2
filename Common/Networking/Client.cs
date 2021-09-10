using Common.Security;
using Common.Security.Cryptography;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common.Networking
{
    public enum Protocol : byte
    {
        FFTSI = 1, // FastFileTransfer Secure Information
        FFTST // FastFileTransfer Secure Transfer
    }

    public class Client : IDisposable
    {
        public Protocol Protocol { get; private set; }
        public string IP { get; private set; }
        public int Port { get; private set; }
        public string Password { get; private set; }
        public bool Connected => _socket.Connected;
        public CryptoServiceAlgorithm CryptoServiceAlgorithm => _cryptoService.Algorithm;
        public bool IsDisposed { get; private set; }

        private Socket _socket;
        private long _bufferSize = 1024 * 1024; // Buffer size for packets (1MB)?
        private CryptoService _cryptoService;

        public Client(Protocol protocol, Socket socket, string password, CryptoServiceAlgorithm algorithm)
        {
            Protocol = protocol;
            _socket = socket;
            Password = password;

            IP = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
            Port = ((IPEndPoint)socket.LocalEndPoint).Port;

            _cryptoService = new CryptoService(algorithm, password);
        }

        public Client(Protocol protocol, string dns, int port, string password, CryptoServiceAlgorithm algorithm)
        {
            Protocol = protocol;
            IP = dns;
            Port = port;
            Password = password;

            _cryptoService = new CryptoService(algorithm, password);
        }

        public void Connect(long maxBufferSize)
        {
            _bufferSize = maxBufferSize;
            try
            {
                // Outbound connections
                if (_socket == null)
                {
                    _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    var connect = _socket.BeginConnect(new IPEndPoint(ResolveDns(IP), Port), null, null);
                    
                    // 30 second timeout on new connection
                    bool success = connect.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(30), true);

                    if (success)
                    {
                        _socket.EndConnect(connect);
                    }
                    else
                    {
                        _socket.Close();
                        throw new SocketException(10060);
                    }

                    // Send protocol and other information to server
                    PerformHandshake();
                }

                // Begin waiting for data
                _socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, Receive, null);

                // Notify we are done handshake and good to go 
                Ready?.Invoke(this);
            }
            catch (Exception e)
            {
                Logger.Error("An error occured while establisting the following connection : " + ToString());
                Logger.Debug(e.Message);

                throw;
            }
        }

        private void PerformHandshake()
        {
            // Build out packet information
            byte[] info = new byte[67];
            byte[] pass = Encoding.ASCII.GetBytes(Password); // Hash the password -- should never transfer in plain text
            pass.CopyTo(info, 3);
            info[0] = (byte)Protocol;
            info[1] = (byte)_cryptoService.Algorithm; // Encryption mode
            info[2] = 0; // Compression mode

            _socket.Send(info);

            // Wait for server to confirm
            info = new byte[1];
            if (_socket.Receive(info) == 1)
            {
                if (info[0] == 1)
                {
                    return;
                }
            }

            throw new Exception("Failed to establish connection. Handshake failed.");
        }

        private IPAddress ResolveDns(string dns)
        {
            try
            {
                return IPAddress.Parse(dns);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return Dns.GetHostAddresses(dns)[0];
            }
        }

        private void Receive(IAsyncResult ir)
        {
            try
            {
                byte[] buffer = new byte[4];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read = 0, size = 0;

                    read = this._socket.Receive(buffer);

                    if (read != 4)
                    {
                        Logger.Error("Invalid packet size received.");
                        Disconnect();
                    }
                    else
                    {
                        size = BitConverter.ToInt32(buffer, 0);
                        buffer = new byte[_bufferSize];

                        // Loop until all of the data for the packet is received
                        while (size > 0)
                        {
                            read = this._socket.Receive(buffer, 0, size > buffer.Length ? buffer.Length : size, SocketFlags.None);
                            ms.Write(buffer, 0, read);
                            size -= read;
                        }
                    }

                    buffer = ms.ToArray();
                }

                // Decrypt data
                buffer = _cryptoService.Decrypt(buffer);

                Packet p = new Packet(buffer);
                PacketReceived?.Invoke(this, p);

                buffer = null;

                // Wait for next packet
               _socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, Receive, null);
            }
            catch (Exception ex)
            {
                Logger.Error($"Connection was lost for {ToString()}");
                Logger.Debug(ex.Message);

                Disconnect();
            }
        }

        public void Transmit(Packet packet)
        {
            Transmit(packet.GetRaw());
        }

        private void Transmit(byte[] raw)
        {
            try
            {
                // Encrypt data
                raw = _cryptoService.Encrypt(raw);

                _socket.Send(BitConverter.GetBytes(raw.Length));
                _socket.Send(raw);
            }
            catch (Exception e)
            {
                Logger.Error($"An error occured attempting to send a message to ${ToString()}");
                Logger.Debug(e.Message);

                Disconnect();
            }
        }

        public void Disconnect()
        {
            if (_socket != null && _socket.Connected)
            {
                _socket.Disconnect(false);
            }

            Disconnected?.Invoke(this);
        }


        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                _socket.Dispose();
            }
        }

        public override string ToString()
        {
            string protcol = $"Protocol={Enum.GetName(typeof(Protocol), Protocol)}";
            string ep = $"EndPoint={IP}:{Port}";
            string passw = $"Password={Password.Substring(0, 8)}...";
            string crypto = $"Encryption={Enum.GetName(typeof(CryptoServiceAlgorithm), _cryptoService.Algorithm)}";

            return $"Client {{{ep}, {protcol}, {crypto}, {passw} }}";
        }

        public delegate void ReadyHandler(Client client);
        public event ReadyHandler Ready;
        public delegate void PacketReceivedHandler(Client client, Packet packet);
        public event PacketReceivedHandler PacketReceived;
        public delegate void DisconnectedHandler(Client client);
        public event DisconnectedHandler Disconnected;
    }
}
