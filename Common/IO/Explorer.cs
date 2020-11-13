using Common.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;

namespace Common.IO
{
    public class Explorer
    {
        private Client _client;
        private string[] _protectedDirectories;
        private static char directorySplit;
        private static char oppositeSplit;
        public Explorer(Client client, string[] protectedDirectories)
        {
            _client = client;

            _client.PacketReceived += _client_PacketReceived;
            _client.Disconnected += _client_Disconnected;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                directorySplit = '\\';
                oppositeSplit = '/';
            }
            else
            {
                directorySplit = '/';
                oppositeSplit = '\\';
            }

            _protectedDirectories = protectedDirectories;
        }

        private void _client_Disconnected(Client client)
        {
            // TODO: Cancel all transfers?
        }

        private void _client_PacketReceived(Client client, Packet packet)
        {
            try
            {
                switch (packet.PacketHeader)
                {
                    case PacketHeader.DrivesGet:
                        GetDrives();
                        break;
                    case PacketHeader.DirectoryGet:
                        GetDirectory(packet);
                        break;
                    case PacketHeader.DirectoryCreate:
                        CreateDirectory(packet);
                        break;
                    case PacketHeader.DirectoryMove:
                        MoveDirectory(packet);
                        break;
                    case PacketHeader.DirectoryDelete:
                        DeleteDirectory(packet);
                        break;
                    case PacketHeader.DirectoryCompress:
                        CompressDirectory(packet);
                        break;
                    case PacketHeader.FileDelete:
                        DeleteFile(packet);
                        break;
                    case PacketHeader.FileCompress:
                        CompressFile(packet);
                        break;
                    case PacketHeader.FileMove:
                        MoveFile(packet);
                        break;
                    default: // Unhandled packet
                        break;
                }

                // For generic actions, respond with same packet to confirm success
                if (packet.PacketHeader != PacketHeader.DirectoryGet
                    && packet.PacketHeader != PacketHeader.DrivesGet)
                {
                    client.Transmit(packet);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                SendException(ex);
            }
        }

        public static string FixPath(string path)
        {
            string fix = path.Replace(oppositeSplit, directorySplit);
            Logger.Debug($"Fixing path: orig={path}, fix={fix}");
            return fix;
        }

        private void SendException(Exception ex)
        {
            using (Packet p = new Packet(PacketHeader.Exception, ex.Message))
            {
                _client.Transmit(p);
            }
        }

        private bool IsProtected(string path, bool throwException = true)
        {
            var find = _protectedDirectories.FirstOrDefault(d => path.ToLower().StartsWith(d.ToLower()));

            if (find != null && throwException)
            {
                throw new Exception($"Access denied! '{path}' is a protected directory.");
            }

            return find != null;
        }

        private void CreateDirectory(Packet packet)
        {
            string fullPath = FixPath(packet.PayloadAsString());

            if (!IsProtected(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                Logger.Info($"Created new directory: {fullPath}");
            }
        }

        private void MoveDirectory(Packet packet)
        {
            string[] data = listFromArray(packet.Payload);

            if (!IsProtected(data[0]) && !IsProtected(data[1]))
            {
                Logger.Info($"Moving directory src={data[0]}, dst={data[1]}");
                Directory.Move(data[0], data[1]);
            }
        }

        private void DeleteDirectory(Packet packet)
        {
            string fullPath = FixPath(packet.PayloadAsString());

            if (!IsProtected(fullPath) && Directory.Exists(fullPath))
            {
                Logger.Info($"Deleting directory: {fullPath}");
                Directory.Delete(fullPath, true);
            }
        }

        private void CompressDirectory(Packet packet)
        {
            string fullPath = FixPath(packet.PayloadAsString());

            if (!IsProtected(fullPath) && Directory.Exists(fullPath))
            {
                try
                {
                    Logger.Info($"Compressing directory src={fullPath}, dst={fullPath}.zip");
                    ZipFile.CreateFromDirectory(fullPath, $"{fullPath}.zip", CompressionLevel.Fastest, true);
                }
                catch (Exception ex)
                {
                    Logger.Error($"An error occured while compressing {fullPath}");
                    Logger.Debug(ex.Message);

                    if (File.Exists($"{fullPath}.zip"))
                        File.Delete($"{fullPath}.zip");
                    throw ex;
                }
            }
        }

        private void DeleteFile(Packet packet)
        {
            string fullPath = FixPath(packet.PayloadAsString());

            if (!IsProtected(fullPath) && File.Exists(fullPath))
            {
                Logger.Info($"Deleting file: {fullPath}");
                File.Delete(fullPath);
            }
        }

        private void MoveFile(Packet packet)
        {
            string[] data = listFromArray(packet.Payload);

            if (!IsProtected(data[0]) && !IsProtected(data[1]))
            {
                Logger.Info($"Moving file src={data[0]}, dst={data[1]}");
                Directory.Move(data[0], data[1]);
            }
        }

        private void CompressFile(Packet packet)
        {
            string fullPath = FixPath(packet.PayloadAsString());

            if (!IsProtected(fullPath))
            {
                try
                {
                    Logger.Info($"Compressing file src={fullPath}, dst={fullPath}.zip");
                    using (FileStream fs = new FileStream($"{fullPath}.zip", FileMode.Create))
                    {
                        using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Create))
                        {
                            zip.CreateEntryFromFile(fullPath, Path.GetFileName(fullPath));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"An error occured while compressing {fullPath}");
                    Logger.Debug(ex.Message);

                    if (File.Exists($"{fullPath}.zip"))
                        File.Delete($"{fullPath}.zip");
                    throw ex;
                }
            }
        }

        private void GetDrives()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    var readyDrives = DriveInfo.GetDrives().Where(d => d.IsReady).ToList();
                    List<string[]> drives = new List<string[]>();

                    foreach (var drive in readyDrives)
                    {
                        try
                        {
                            drives.Add(new string[]
                            {
                                drive.Name,
                                drive.VolumeLabel,
                                GetSize(drive.TotalSize),
                                GetSize(drive.TotalSize - drive.AvailableFreeSpace),
                                GetSize(drive.TotalFreeSpace),
                                drive.DriveFormat.ToString(),
                                drive.DriveType.ToString(),
                                Environment.GetFolderPath(Environment.SpecialFolder.System).ToLower().Contains(drive.RootDirectory.Name.ToLower()).ToString()
                            });
                        }
                        catch (Exception e)
                        {
                            Logger.Error("Failed to read a drives information");
                            Logger.Debug(e.Message);
                        }
                    }

                    bw.Write(drives.Count);
                    Logger.Debug($"Sending {drives.Count} of {readyDrives.Count} drives");
                    // Package drive information into packet
                    foreach (string[] drive in drives)
                    {
                        for (int i = 0; i < drive.Length; i++)
                        {
                            bw.Write(drive[i]);
                        }
                    }
                }

                using (Packet p = new Packet(PacketHeader.DrivesGet, ms.ToArray()))
                {
                    _client.Transmit(p);
                }
            }
        }

        private void GetDirectory(Packet packet)
        {
            string dir = packet.PayloadAsString();

            if (!IsProtected(dir))
            {
                Logger.Info($"Accessing directory: {dir}");

                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        string root = dir.Remove(dir.LastIndexOf(directorySplit));
                        if (root.Length == 2)
                            root += directorySplit;

                        var directories = Directory.EnumerateDirectories(dir);
                        var files = Directory.EnumerateFiles(dir);

                        // This will serve as a place holder for the user to go "up" a directory
                        List<string[]> temp = new List<string[]>();
                        temp.Add(new string[]
                        {
                            "..",
                            root,
                            " ",
                            " ",
                            " "
                        });

                        // TODO: This can be refactored down to 2 loops instead of 4
                        // We can iterate the directory and write to the memory stream
                        // at the same time.
                        foreach (var d in directories)
                        {
                            try
                            {
                                DirectoryInfo di = new DirectoryInfo(d);
                                var isProt = IsProtected(di.FullName, false) ? "*" : "";
                                temp.Add(new string[]
                                {
                                    di.Name+ isProt,
                                    di.FullName,
                                    di.GetFiles().Length + " FILES" ,
                                    di.CreationTime.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                    di.LastAccessTime.ToString("dd/MM/yyyy hh:mm:ss tt")
                                });
                            }
                            catch (Exception e)
                            {
                                Logger.Error($"Failed to access directory: {d}");
                                Logger.Debug(e.Message);
                            }
                        }

                        bw.Write(temp.Count());
                        foreach (string[] t in temp)
                        {
                            for (int i = 0; i < t.Length; i++)
                            {
                                bw.Write(t[i]);
                            }
                        }


                        temp = new List<string[]>();
                        foreach (var f in files)
                        {
                            try
                            {

                                FileInfo fi = new FileInfo(f);
                                temp.Add(new string[]
                                {
                                    fi.Name,
                                    fi.FullName,
                                    GetSize((double)fi.Length),
                                    fi.CreationTime.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                    fi.LastAccessTime.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                    fi.Length.ToString()
                                });
                            }
                            catch (Exception e)
                            {
                                Logger.Error($"Failed to access file: {f}");
                                Logger.Debug(e.Message);
                            }
                        }

                        bw.Write(temp.Count());
                        foreach (string[] t in temp)
                        {
                            for (int i = 0; i < t.Length; i++)
                            {
                                bw.Write(t[i]);
                            }
                        }
                    }

                    using (Packet p = new Packet(PacketHeader.DirectoryGet, ms.ToArray()))
                    {
                        _client.Transmit(p);
                    }
                }
            }
        }

        public string[] listFromArray(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int len = br.ReadInt32();
                    string[] rtn = new string[len];

                    for (int i = 0; i < len; i++)
                    {
                        rtn[i] = br.ReadString();
                        rtn[i] = FixPath(rtn[i]);
                    }

                    return rtn;
                }
            }
        }

        public static string GetSize(double size)
        {
            string rtn;
            rtn = string.Format("{0:0} bytes", size);
            if (size > 1000)
            {
                size /= 1024; // KB
                rtn = string.Format("{0:0.00} KB", size);
            }
            if (size > 1000)
            {
                size /= 1024; // MB
                rtn = string.Format("{0:0.00} MB", size);
            }
            if (size > 1000)
            {
                size /= 1024; // GB
                rtn = string.Format("{0:0.00} GB", size);
            }
            if (size > 1000)
            {
                size /= 1024; // TB
                rtn = string.Format("{0:0.00} TB", size);
            }
            return rtn;
        }
    }
}
