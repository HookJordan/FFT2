using Common.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Common.IO
{
    public class Explorer
    {
        private Client _client;
        private string directorySplit = "";
        public Explorer(Client client)
        {
            _client = client;

            _client.PacketReceived += _client_PacketReceived;
            _client.Disconnected += _client_Disconnected;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                directorySplit = "\\";
            }
            else
            {
                directorySplit = "/";
            }
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
                    default: // Unhandled packet
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                SendException(ex);
            }
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
            return false;
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
                                    di.CreationTime.ToString("dd/MM/yyyy HH:ss"),
                                    di.LastAccessTime.ToString("dd/MM/yyyy HH:ss")
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
                                    fi.CreationTime.ToString("dd/MM/yyyy HH:ss"),
                                    fi.LastAccessTime.ToString("dd/MM/yyyy HH:ss"),
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

        public static string GetSize(double size)
        {
            string rtn;
            rtn = string.Format("{0:0} bytes", size);
            if (size > 100)
            {
                size /= 1024; //kb
                rtn = string.Format("{0:0.00} KB", size);
            }
            if (size > 100)
            {
                size /= 1024; //mb
                rtn = string.Format("{0:0.00} MB", size);
            }
            if (size > 1000)
            {
                size /= 1024; // gb
                rtn = string.Format("{0:0.00} GB", size);
            }
            return rtn;
        }
    }
}
