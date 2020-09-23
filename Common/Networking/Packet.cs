using System;
using System.Linq;
using System.Text;

namespace Common.Networking
{
    public enum PacketHeader : byte
    {
        Goodbye = 1,
        Ping,
        Pong,
        DrivesGet,
        DirectoryGet,
        Exception
    }

    public class Packet : IDisposable
    {
        public PacketHeader PacketHeader { get; set; }
        public byte[] Payload { get; set; }

        public Packet(PacketHeader header)
        {
            PacketHeader = header;
            Payload = new byte[0];
        }

        public Packet(PacketHeader header, byte[] payload)
        {
            PacketHeader = header;
            Payload = payload;
        }

        public Packet(PacketHeader header, string payload)
        {
            PacketHeader = header;
            Payload = Encoding.Unicode.GetBytes(payload);
        }

        public Packet(byte[] raw)
        {
            PacketHeader = (PacketHeader)raw[0];
            Payload = raw.Skip(1).ToArray();
        }

        public string PayloadAsString()
        {
            return Encoding.Unicode.GetString(Payload);
        }

        public byte[] GetRaw()
        {
            byte[] raw = new byte[Payload.Length + 1];
            raw[0] = (byte)PacketHeader;
            Payload.CopyTo(raw, 1);

            return raw;
        }

        public void Dispose()
        {
            Payload = null;
        }
    }
}
