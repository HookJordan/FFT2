using System;
using System.Net;
using System.Net.Sockets;

namespace Common.Networking
{
    public enum PingerResponseCodes : byte
    {
        Online = 1,
        Offline = 2,
        Unknown
    }

    public class Pinger
    {
        public static PingerResponseCodes CheckServerStatus(string ip, int port)
        {
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    var connect = socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), null, null);

                    bool success = connect.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(30), true);

                    if (success)
                    {
                        // Let the server know it was just a status check
                        byte[] payload = new byte[] { 255 };
                        socket.Send(payload);

                        Logger.Debug($"{ip}:{port} is online");

                        return PingerResponseCodes.Online;
                    }
                    else
                    {
                        Logger.Debug($"{ip}:{port} is offline");
                        return PingerResponseCodes.Offline;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to determine status of {ip}:{port}");
                Logger.Debug(ex.Message);

                return PingerResponseCodes.Unknown;
            }
        }
    }
}
