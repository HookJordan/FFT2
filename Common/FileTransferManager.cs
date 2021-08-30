using Common.IO;
using Common.Networking;
using System;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    public class FileTransferManager
    {
        public Dictionary<string, FileTransfer> FileTransfer { get; private set; }
        private Client _transferSocket;
        private long _bufferSize;

        public FileTransferManager(Client transferSocket, long maxBufferSize)
        {
            FileTransfer = new Dictionary<string, FileTransfer>();
            _transferSocket = transferSocket;
            _bufferSize = 1024 * 1024; // 1 MB 
            _transferSocket.PacketReceived += _transferSocket_PacketReceived;
        }

        private void _transferSocket_PacketReceived(Client client, Packet packet)
        {
            try
            {
                switch (packet.PacketHeader)
                {
                    // Download and Upload are reversed when receiving a request
                    // Upload = Download 
                    // Download = Upload 
                    case PacketHeader.FileTransferDownload:
                        StartUpload(packet);
                        break;
                    case PacketHeader.FileTransferUpload:
                        StartDownload(packet);
                        break;
                    case PacketHeader.FileTransferChunk:
                        HandleFileChunk(packet);
                        break;
                    case PacketHeader.FileTransferCancel:
                    case PacketHeader.FileTransferCancelConfirm:
                        CancelTransfer(packet);
                        break;
                    default: // Unhandled packet 
                        break;
                }
            } 
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private void CancelTransfer(Packet packet)
        {
            string transferId = packet.PayloadAsString();

            if (FileTransfer.ContainsKey(transferId))
            {
                FileTransfer transfer = FileTransfer[transferId];
                transfer.Cancel();

                if (packet.PacketHeader == PacketHeader.FileTransferCancel)
                {
                    packet.PacketHeader = PacketHeader.FileTransferCancelConfirm;
                    _transferSocket.Transmit(packet);
                }
            }
            else
            {
                Logger.Error($"Could not find file transfer with id {transferId}");
            }
        }

        private void StartDownload(Packet packet)
        {
            using MemoryStream ms = new MemoryStream(packet.Payload);
            using (BinaryReader br = new BinaryReader(ms))
            {
                // Create Session
                string localPath = br.ReadString();
                string remotePath = br.ReadString();
                string id = br.ReadString();
                long length = long.Parse(br.ReadString());

                FileTransfer ft = new FileTransfer(localPath, remotePath, length, 0);
                FileTransfer.Add(id, ft);

                // Start Transfer
                SendChunk(id, new byte[0]);

                // Log
                Logger.Info(ft.ToString());
            }
        }

        private void StartUpload(Packet packet)
        {
            using (MemoryStream ms = new MemoryStream(packet.Payload))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    // Create Session
                    string localPath = br.ReadString();
                    string remotePath = br.ReadString();
                    string id = br.ReadString();

                    FileTransfer ft = new FileTransfer(localPath, remotePath, _bufferSize);
                    FileTransfer.Add(id, ft);

                    // Start transfer
                    SendChunk(id, new byte[0]);

                    // Log
                    Logger.Info(ft.ToString());
                }
            }
        }

        private void HandleFileChunk(Packet p)
        {
            using (MemoryStream ms = new MemoryStream(p.Payload))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    string id = br.ReadString();

                    if (FileTransfer.ContainsKey(id))
                    {
                        var ft = FileTransfer[id];

                        if (ft.TransferType == TransferType.Upload)
                        {
                            SendChunk(id, ft.NextChunk());
                        }
                        else
                        {
                            int size = br.ReadInt32();
                            if (size > 0)
                            {
                                byte[] chunk = br.ReadBytes(size);
                                ft.WriteChunk(chunk);
                            }
                            if (ft.Transfering)
                            {
                                // Request next chunk
                                SendChunk(id, new byte[0]);
                            }
                        }

                        UpdateTransfer?.Invoke(this, ft);
                    }
                }
            }
        }

        private void SendChunk(string id, byte[] chunk)
        {
            if (!FileTransfer[id].Paused)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write(id);
                        bw.Write(chunk.Length);
                        bw.Write(chunk);
                    }

                    _transferSocket.Transmit(new Packet(PacketHeader.FileTransferChunk, ms.ToArray()));
                }
            }
        }

        public void CancelTransfer(string id)
        {
            if (FileTransfer.ContainsKey(id))
            {
                FileTransfer[id].Cancel();
                Logger.Info($"Cancelled {FileTransfer[id]}");
            }
        }

        public void CancelAllActiveTransfers()
        {
            foreach (var transfer in FileTransfer.Values)
            {
                transfer.Cancel();
            }
        }

        public delegate void UpdateTransferHandler(FileTransferManager sender, FileTransfer fileTransfer);
        public event UpdateTransferHandler UpdateTransfer;
    }
}
