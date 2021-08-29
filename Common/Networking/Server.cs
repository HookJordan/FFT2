using Common.Security;
using Common.Security.Cryptography;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common.Networking
{
    public class Server : IDisposable
    {
        public bool Running { get; private set; }
        public int Port { get; private set; }
        public string Password { get; private set; }
        public string PersonalPassword { get; private set; }

        private Socket _socket;
        
        private Server() { }

        public Server(int port, string password, string personal)
        {
            Port = port;
            SetPassword(password);
            SetPersonalPassword(personal);
        }

        public void SetPassword(string password)
        {
            if (password.Length != 16)
            {
                password = Hashing.SHA(password);
            }

            Password = password;
        }

        public void SetPersonalPassword(string password)
        {
            if (password.Length != 16)
            {
                password = Hashing.SHA(password);
            }

            PersonalPassword = password;
        }

        public void Listen()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Bind(new IPEndPoint(IPAddress.Any, Port));

                if (_socket.IsBound)
                {
                    Logger.Info($"Bound to host machine on port {Port}");
                    _socket.Listen(1);

                    Running = true;
                    _socket.BeginAccept(AcceptConnection, null);
                }
            } 
            catch (Exception e)
            {
                Logger.Error($"Unable to start server socket on port {Port}.");
                Logger.Debug(e.Message);

                throw new Exception($"Unable to start server socket on port {Port}");
            }
        }

        private void AcceptConnection(IAsyncResult ar)
        {
            if (Running)
            {
                try
                {
                    Socket socket = _socket.EndAccept(ar);

                    // Application Layer Handshake
                    Handshake(socket);

                    // Wait for next connection
                    _socket.BeginAccept(AcceptConnection, null);

                }
                catch (Exception e)
                {
                    Logger.Error("An error occurred while accepting a connection");
                    Logger.Debug(e.Message);
                }
            }
        }

        private void Handshake(Socket socket)
        {
            // 1 byte protocol 
            // 1 byte encryption mode
            // 1 byte compression mode
            // 64 bytes sha256 hash password
            // = 67 bytes
            byte[] req = new byte[67];
            int rec = socket.Receive(req);
            // Ensure the correct amount of bytes are received
            if (rec == req.Length)
            {
                byte protocol = req[0], enc = req[1], comp = req[2];
                string password = Encoding.ASCII.GetString(req.Skip(3).ToArray());

                if (password == Password || password == PersonalPassword)
                {
                    Client c = new Client((Protocol)protocol, socket, password, (CryptoServiceAlgorithm)enc);
                    SendResponse(socket, 1);
                    NewClient?.Invoke(this, c);
                    Logger.Info($"New {c}");
                }
                else
                {
                    SendResponse(socket, 0);
                    socket.Dispose();
                    Logger.Info($"Rejected socket for incorrect password {socket.RemoteEndPoint}");
                }
            }
            else if (rec == 1 && req[0] == 255)
            {
                // Check if client is online
                Logger.Info($"Updated server status for {socket.RemoteEndPoint}");
            }
            else
            {
                Logger.Info($"Rejected socket for incorrect password {socket.RemoteEndPoint}");
                SendResponse(socket, 0);
                socket.Dispose();
            }
        }

        private void SendResponse(Socket client, byte response)
        {
            client.Send(new byte[] { response });
        }

        public void SetPort(int port)
        {
            if (port != Port)
            {
                Logger.Info($"Setting server port ${port}");

                Stop();
                Port = port;
                Listen();
            }
        }

        public void Stop()
        {
            if (Running)
            {
                Running = false;
                _socket.Dispose();
            }
        }

        public void Dispose()
        {
            Stop();
        }

        public delegate void NewClientHandler(Server server, Client client);
        public event NewClientHandler NewClient;
    }
}
