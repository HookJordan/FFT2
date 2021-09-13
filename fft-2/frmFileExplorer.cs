using Common;
using Common.IO;
using Common.Networking;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace fft_2
{
    public partial class frmFileExplorer : Form
    {
        private bool _drives = true;
        private string currentPath = "";
        private readonly Client _client;
        private Client _transfer;
        private FileTransferManager _fileTransferManager;

        public frmFileExplorer(Client client)
        {
            InitializeComponent();

            _client = client;
            _client.Disconnected += _client_Disconnected;
            _client.PacketReceived += _client_PacketReceived;
        }

        private void _client_PacketReceived(Client client, Packet packet)
        {
            using (MemoryStream ms = new MemoryStream(packet.Payload))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        switch (packet.PacketHeader)
                        {
                            case PacketHeader.DrivesGet:
                                SetDriveColumns();

                                int drives = br.ReadInt32();
                                for(int i = 0; i < drives; i++)
                                {
                                    string[] columns = new string[]
                                    {
                                        br.ReadString(),    // name
                                        br.ReadString(),    // label
                                        br.ReadString(),    // total
                                        br.ReadString(),    // used 
                                        br.ReadString(),    // free
                                        br.ReadString(),    // format
                                        br.ReadString()     // type
                                    };
                                    var item = new ListViewItem(columns, 0);
                                    item.Tag = "drive";

                                    if (br.ReadString() == true.ToString()) { item.ImageIndex = 1; }

                                    lstFiles.Items.Add(item);
                                }

                                lstFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                                lstFiles.Enabled = true;
                                break;
                            case PacketHeader.DirectoryGet:
                                bool resize = _drives;
                                SetFileColumns();
                                lstFiles.BeginUpdate();

                                // Folders
                                int rows = br.ReadInt32();
                                for (int i = 0; i < rows; i++)
                                {
                                    string[] columns = new string[]
                                    {
                                        br.ReadString(),    // Name
                                        br.ReadString(),    // Full path
                                        br.ReadString(),    // File count
                                        br.ReadString(),    // Created On
                                        br.ReadString()     // Last Modified
                                    };

                                    var item = new ListViewItem(columns);
                                    item.ImageIndex = 3;
                                    item.Tag = "folder";

                                    if (item.SubItems[0].Text.Contains("*"))
                                    {
                                        item.SubItems[0].Text = item.SubItems[0].Text.Replace("*", "");
                                        item.ForeColor = Color.Red;
                                    }

                                    lstFiles.Items.Add(item);
                                }

                                // Files
                                rows = br.ReadInt32();
                                for (int i = 0; i < rows; i++)
                                {
                                    string[] columns = new string[]
                                    {
                                        br.ReadString(),    // Name
                                        br.ReadString(),    // Full path
                                        br.ReadString(),    // size
                                        br.ReadString(),    // Created On
                                        br.ReadString(),     // Last Modified
                                    };
                                    var len = long.Parse(br.ReadString());
                                    var item = new ListViewItem(columns);
                                    item.Tag = "file";
                                    item.SubItems[2].Tag = len; // full file size on subitem item tag

                                    string ex = Path.GetExtension(columns[1]);
                                    if (ex == "") { item.ImageIndex = 2; }
                                    else
                                    {
                                        if (lstIcons.Images.ContainsKey(ex))
                                            item.ImageIndex = lstIcons.Images.IndexOfKey(ex);
                                        else
                                        {
                                            Icon icon = IconReader.GetFileIcon(ex, IconReader.IconSize.Small, false);
                                            if (icon != null)
                                            {
                                                Logger.Debug($"Adding icon for ext={ex}");
                                                lstIcons.Images.Add(ex, icon);
                                                item.ImageIndex = lstIcons.Images.Count - 1;
                                            }
                                        }
                                    }

                                    lstFiles.Items.Add(item);
                                }
                                if (resize) lstFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                                lstFiles.Enabled = true;
                                lstFiles.EndUpdate();
                                break;
                            case PacketHeader.DirectoryCreate:
                            case PacketHeader.DirectoryDelete:
                            case PacketHeader.DirectoryCompress:
                            case PacketHeader.DirectoryMove:
                            case PacketHeader.FileCompress:
                            case PacketHeader.FileDelete:
                            case PacketHeader.FileMove:
                                lstFiles.Enabled = false;
                                client.Transmit(new Packet(PacketHeader.DirectoryGet, currentPath));
                                break;
                            case PacketHeader.Checksum:
                                string p = br.ReadString(), h = br.ReadString();

                                MessageBox.Show($"File Path:\n{p}\n\nSHA256:\n{h}", "SHA256 Checksum", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            case PacketHeader.Exception:
                                MessageBox.Show(packet.PayloadAsString(), "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                lstFiles.Enabled = true;
                                break;
                            default: // Unhandled packet
                                break;
                        }

                    });
                }
            }
        }

        private void _client_Disconnected(Client client)
        {
            client.Dispose();

            Close();
        }

        private void frmFileExplorer_Load(object sender, EventArgs e)
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Text = $"FFT - File Explorer - {_client.IP}:{_client.Port}";

            // Set list to display drive information by default
            SetDriveColumns(true);

            // Load default images
            lstIcons.Images.Add(IconReader.GetFileIcon("", IconReader.IconSize.Small, false));
            lstIcons.Images.Add(IconReader.GetFileIcon("dummy", IconReader.IconSize.Small, false));
            lstIcons.Images.Add(IconReader.GetFolderIcon(IconReader.IconSize.Small, IconReader.FolderType.Open));


            // Send request for list of drives
            _client.Transmit(new Packet(PacketHeader.DrivesGet));
            lstFiles.Enabled = false;

            // Setup Event handlers for form controls
            lstFiles.DoubleClick += LstFiles_DoubleClick;
            FormClosing += FrmFileExplorer_FormClosing;
            mnuRefresh.Click += MnuRefresh_Click;
            lstTransfers.ColumnWidthChanged += LstFiles_ColumnWidthChanged;

            #region mnuEvents
            // Directory buttons
            mnuDirectoryCreate.Click += MnuDirectoryCreate_Click;
            mnuDirectoryMove.Click += MnuDirectoryMove_Click;
            mnuDirectoryDelete.Click += MnuDirectoryDelete_Click;
            mnuDirectoryCompress.Click += MnuDirectoryCompress_Click;

            // File Buttons
            mnuFileCompress.Click += MnuFileCompress_Click;
            mnuFileDelete.Click += MnuFileDelete_Click;
            mnuFileMove.Click += MnuFileMove_Click;
            mnuChecksum.Click += MnuChecksum_Click;

            // Transfer Buttons
            mnuDownload.Click += MnuDownload_Click;
            mnuUpload.Click += MnuUpload_Click;
            mnuTransferPause.Click += MnuTransferPause_Click;
            mnuTransferCancel.Click += MnuTransferCancel_Click;
            #endregion

            // Create transfer socket 
            Visible = false;

            _transfer = new Client(Protocol.FFTST, _client.IP, _client.Port, _client.Password, _client.CryptoServiceAlgorithm);
            _transfer.Ready += _transfer_Ready;
            _transfer.Connect(Program.Configuration.MaxBufferSize);
        }

        private void MnuChecksum_Click(object sender, EventArgs e)
        {
            var item = lstFiles.SelectedItems[0];

            _client.Transmit(new Packet(PacketHeader.Checksum, item.SubItems[1].Text));
        }

        private void LstFiles_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            FixProgressBars();
        }

        private void MnuTransferCancel_Click(object sender, EventArgs e)
        {
            var transfer = lstTransfers.SelectedItems[0];
            if (transfer.SubItems[1].Text != "Completed")
            {
                string msg = "Are you sure you want to cancel the following file transfer?\n\n";
                msg += "Local path:\n" + transfer.SubItems[2].Text;
                msg += "\n\nRemote path:\n" + transfer.SubItems[3].Text;

                if (MessageBox.Show(msg, "Cancel " + transfer.SubItems[0].Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _transfer.Transmit(new Packet(PacketHeader.FileTransferCancel, (string)transfer.Tag));
                    CancelTransferItem(_fileTransferManager.FileTransfer[(string)transfer.Tag]);
                }
            }
        }

        private void MnuTransferPause_Click(object sender, EventArgs e)
        {
            var item = lstTransfers.SelectedItems[0];
            var transfer = _fileTransferManager.FileTransfer[(string)item.Tag];

            transfer.TogglePause();

            // Resume the transfer
            if (!transfer.Paused)
            {
                item.SubItems[1].Text = "In-Progress";

                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write(transfer.TransferId);
                        bw.Write(0);
                    }

                    _transfer.Transmit(new Packet(PacketHeader.FileTransferChunk, ms.ToArray()));
                }

            }
            else
            {
                item.SubItems[1].Text = "Suspended";
                item.SubItems[item.SubItems.Count - 1].Text = "-";
            }
        }

        private void MnuUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string local = ofd.FileName;
                    string remote = Path.Combine(currentPath, Path.GetFileName(local));

                    // Create session 
                    FileTransfer transfer = new FileTransfer(local, remote, Program.Configuration.MaxBufferSize);
                    _fileTransferManager.FileTransfer.Add(transfer.TransferId, transfer);

                    // UI
                    AddTransferItem(transfer);

                    // Send packet to start
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (BinaryWriter bw = new BinaryWriter(ms))
                        {
                            bw.Write(remote);
                            bw.Write(local);
                            bw.Write(transfer.TransferId);
                            bw.Write(transfer.FileLength.ToString());
                        }

                        _transfer.Transmit(new Packet(PacketHeader.FileTransferUpload, ms.ToArray()));
                    }
                }
            }
        }

        private void MnuDownload_Click(object sender, EventArgs e)
        {
            var item = lstFiles.SelectedItems[0];

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                // Determine download type based on extension
                string ext = Path.GetExtension(item.SubItems[1].Text);
                sfd.Filter = $"(*{ext}|*{ext}";
                sfd.FileName = item.Text;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string localPath = sfd.FileName;
                    string remote = item.SubItems[1].Text;

                    // Add file to transfer manager 
                    FileTransfer transfer = new FileTransfer(localPath, remote, (long)item.SubItems[2].Tag, Program.Configuration.MaxBufferSize);
                    _fileTransferManager.FileTransfer.Add(transfer.TransferId, transfer);

                    AddTransferItem(transfer);

                    // Send request to start to transfer socket? 
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (BinaryWriter bw = new BinaryWriter(ms))
                        {
                            bw.Write(remote);
                            bw.Write(localPath);
                            bw.Write(transfer.TransferId);
                        }

                        _transfer.Transmit(new Packet(PacketHeader.FileTransferDownload, ms.ToArray()));
                    }
                }
            }
        }

        private void AddTransferItem(FileTransfer fileTransfer)
        {
            ListViewItem item = new ListViewItem(new string[]
            {
                fileTransfer.TransferType.ToString(),
                "In-Progress",
                fileTransfer.LocalFilePath,
                fileTransfer.RemoteFilePath,
                Explorer.GetSize(fileTransfer.FileLength),
                "-",
                "-",
                ""
            });

            item.Tag = fileTransfer.TransferId;
            lstTransfers.Items.Add(item);

            ProgressBar progressBar = new ProgressBar();
            progressBar.Maximum = 100;
            progressBar.Minimum = 0;
            progressBar.Value = 0;
            progressBar.Tag = fileTransfer.TransferId;
            progressBar.Parent = lstTransfers;
            progressBar.Visible = true;

            Rectangle r = item.SubItems[item.SubItems.Count - 1].Bounds;
            progressBar.SetBounds(r.X, r.Y, r.Width, r.Height);

            lstTransfers.Controls.Add(progressBar);
        }

        private void CancelTransferItem(FileTransfer fileTransfer)
        {
            ListViewItem item = lstTransfers.Items.Cast<ListViewItem>().FirstOrDefault(i => (string)i.Tag == fileTransfer.TransferId);
            ProgressBar progressBar = lstTransfers.Controls.OfType<ProgressBar>().FirstOrDefault(i => (string)i.Tag == fileTransfer.TransferId);
            progressBar.Visible = false;

            item.SubItems[item.SubItems.Count - 3].Text = "-";
            item.SubItems[item.SubItems.Count - 2].Text = "-";
            item.SubItems[1].Text = "Cancelled";
            item.ForeColor = Color.Red;
        }

        private void _transfer_Ready(Client client)
        {
            _fileTransferManager = new FileTransferManager(client, Program.Configuration.MaxBufferSize);
            _fileTransferManager.UpdateTransfer += _fileTransferManager_UpdateTransfer;
            Text += $" - Encryption: {client.CryptoServiceAlgorithm}";
            Visible = true;
        }

        private void FixProgressBars()
        {
            foreach (ListViewItem item in lstTransfers.Items)
            {
                Rectangle bounds = item.SubItems[item.SubItems.Count - 1].Bounds;
                ProgressBar progressBar = lstTransfers.Controls.OfType<ProgressBar>().FirstOrDefault(i => (string)i.Tag == (string)item.Tag);

                progressBar.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                progressBar.Visible = bounds.Y > 10;
            }
        }

        private void _fileTransferManager_UpdateTransfer(FileTransferManager sender, FileTransfer fileTransfer)
        {
            Invoke((MethodInvoker)delegate
            {
                ListViewItem item = lstTransfers.Items.Cast<ListViewItem>().FirstOrDefault(i => (string)i.Tag == fileTransfer.TransferId);
                ProgressBar progress = lstTransfers.Controls.OfType<ProgressBar>().FirstOrDefault(p => (string)p.Tag == fileTransfer.TransferId);

                progress.Value = fileTransfer.CalculatePercentage();
                item.SubItems[item.SubItems.Count - 3].Text = Explorer.GetSize(fileTransfer.Transferred());
                item.SubItems[item.SubItems.Count - 2].Text = Explorer.GetSize(fileTransfer.BytesPerSecond);

                if (!fileTransfer.Transfering)
                {
                    item.SubItems[1].Text = "Completed";
                    item.ForeColor = Color.Green;
                }
            });
        }

        private void MnuFileMove_Click(object sender, EventArgs e)
        {
            var item = lstFiles.SelectedItems[0];
            string name = Prompt.InputBox($"Please enter the new file path\n{item.SubItems[1].Text}", "Move File", item.SubItems[1].Text);

            if (name != "" && name != item.SubItems[1].Text)
            {
                byte[] data = ToByteArray(
                    item.SubItems[1].Text,
                     name
                );

                lstFiles.Enabled = false;
                _client.Transmit(new Packet(PacketHeader.FileMove, data));
            }
        }

        private void MnuFileDelete_Click(object sender, EventArgs e)
        {
            var item = lstFiles.SelectedItems[0];
            if (MessageBox.Show("Are you sure you want to the following file and all of it's contents?\n" + item.SubItems[1].Text, " Delete File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                lstFiles.Enabled = false;
                _client.Transmit(new Packet(PacketHeader.FileDelete, item.SubItems[1].Text));
            }
        }

        private void MnuFileCompress_Click(object sender, EventArgs e)
        {
            var item = lstFiles.SelectedItems[0];
            lstFiles.Enabled = false;

            _client.Transmit(new Packet(PacketHeader.FileCompress, item.SubItems[1].Text));
        }

        private void MnuDirectoryCompress_Click(object sender, EventArgs e)
        {
            var item = lstFiles.SelectedItems[0];
            lstFiles.Enabled = false;

            _client.Transmit(new Packet(PacketHeader.DirectoryCompress, item.SubItems[1].Text));
        }

        private void MnuDirectoryDelete_Click(object sender, EventArgs e)
        {
            var item = lstFiles.SelectedItems[0];
            if (MessageBox.Show("Are you sure you want to the following directory and all of it's contents?\n" + item.SubItems[1].Text, " Delete Directory", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                lstFiles.Enabled = false;
                _client.Transmit(new Packet(PacketHeader.DirectoryDelete, item.SubItems[1].Text));
            }
        }

        private void MnuDirectoryMove_Click(object sender, EventArgs e)
        {
            var item = lstFiles.SelectedItems[0];
            string name = Prompt.InputBox($"Please enter the new directory path\n{item.SubItems[1].Text}", "Move Directory", item.SubItems[1].Text);

            if (name != "" && name != item.SubItems[1].Text)
            {
                byte[] data = ToByteArray(
                    item.SubItems[1].Text,
                     name
                );

                lstFiles.Enabled = false;
                _client.Transmit(new Packet(PacketHeader.DirectoryMove, data));
            }
        }

        public byte[] ToByteArray(params object[] obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(obj.Length);

                    foreach (var o in obj)
                    {
                        bw.Write(o.ToString());
                    }
                }

                return ms.ToArray();
            }
        }

        private void MnuDirectoryCreate_Click(object sender, EventArgs e)
        {
            string name = Prompt.InputBox("Please enter the name of the new directory", "Create Directory");

            if (name != "")
            {
                lstFiles.Enabled = false;
                _client.Transmit(new Packet(PacketHeader.DirectoryCreate, Path.Combine(currentPath, name)));
            }
        }

        private void MnuRefresh_Click(object sender, EventArgs e)
        {
            lstFiles.Enabled = false;

            if (currentPath.Length == 0)
                _client.Transmit(new Packet(PacketHeader.DrivesGet));
            else
                _client.Transmit(new Packet(PacketHeader.DirectoryGet, currentPath));
        }

        private void FrmFileExplorer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_client.Connected)
            {
                if (MessageBox.Show("By closing this window you will be disconnecting your session which will cancel any pending file transfers. Are you sure you want to close the session?", "Close Session", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _client.Disconnect();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void LstFiles_DoubleClick(object sender, EventArgs e)
        {
            if ((string)lstFiles.SelectedItems[0].Tag != "file")
            {
                lstFiles.Enabled = false;
                GetDirectory(lstFiles.SelectedItems[0]);
            }
        }

        private void GetDirectory(ListViewItem item)
        {
            string last = currentPath;
            string type = (string)item.Tag;

            if (type == "drive")
                currentPath = item.SubItems[0].Text;
            else if (type == "folder")
                currentPath = item.SubItems[1].Text;
            else
                return; // file type 

            txtNav.Text = currentPath;

            if (last == currentPath || currentPath == "")
            {
                currentPath = "";
                txtNav.Text = "";
                _client.Transmit(new Packet(PacketHeader.DrivesGet));
            }
            else
            {
                _client.Transmit(new Packet(PacketHeader.DirectoryGet, currentPath));
            }
        }


        private void SetDriveColumns(bool over = false)
        {
            if (_drives && !over)
            {
                lstFiles.Items.Clear();
                return;
            }

            _drives = true;
            lstFiles.Clear();

            lstFiles.Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader() { Text = "Drive" },
                new ColumnHeader() { Text = "Label" },
                new ColumnHeader() { Text = "Total Space" },
                new ColumnHeader() { Text = "Used Space" },
                new ColumnHeader() { Text = "Free Space" },
                new ColumnHeader() { Text = "Format" },
                new ColumnHeader() { Text = "Type" }
            });
        }

        private void SetFileColumns()
        {
            if (!_drives)
            {
                lstFiles.Items.Clear();
                return;
            }

            lstFiles.Clear();
            _drives = false;
            lstFiles.Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader() { Text = "Name" },
                new ColumnHeader() { Text = "Full Path" },
                new ColumnHeader() { Text = "Size" },
                new ColumnHeader() { Text = "Created On" },
                new ColumnHeader() { Text = "Last Modified" }
            });
        }

        private void mnuTransfers_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lstTransfers.SelectedItems.Count == 0)
            {
                e.Cancel = true;
            }
        }

        private void mnuExplore_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Don't open when on drives
            if (currentPath == "")
                e.Cancel = true;
            else
            {
                if (lstFiles.SelectedItems.Count == 0)
                {
                    mnuDirectoryMove.Enabled = false;
                    mnuFileMove.Enabled = false;
                    mnuDirectoryDelete.Enabled = false;
                    mnuFileDelete.Enabled = false;
                    mnuDirectoryCompress.Enabled = false;
                    mnuFileCompress.Enabled = false;
                    mnuDownload.Enabled = false;
                    mnuUpload.Enabled = true;
                    mnuChecksum.Enabled = false;
                }
                else
                {
                    var type = (string)lstFiles.SelectedItems[0].Tag;

                    if (type == "folder")
                    {
                        mnuDirectoryMove.Enabled = true;
                        mnuDirectoryDelete.Enabled = true;
                        mnuDirectoryCompress.Enabled = true;

                        mnuFileMove.Enabled = false;
                        mnuFileDelete.Enabled = false;
                        mnuFileCompress.Enabled = false;

                        mnuUpload.Enabled = false;
                        mnuDownload.Enabled = false;
                    }
                    else
                    {
                        mnuDirectoryMove.Enabled = false;
                        mnuDirectoryDelete.Enabled = false;
                        mnuDirectoryCompress.Enabled = false;

                        mnuFileMove.Enabled = true;
                        mnuFileDelete.Enabled = true;
                        mnuFileCompress.Enabled = true;

                        mnuUpload.Enabled = true;
                        mnuDownload.Enabled = true;
                        mnuChecksum.Enabled = true;
                    }
                }
            }
        }
    }
}
