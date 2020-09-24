using Common;
using Common.Networking;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace fft_2
{
    public partial class frmFileExplorer : Form
    {
        private bool _drives = true;
        private string currentPath = "";
        private Client _client;

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

            #region mnuEvents
            mnuDirectoryCreate.Click += MnuDirectoryCreate_Click;
            mnuDirectoryMove.Click += MnuDirectoryMove_Click;
            mnuDirectoryDelete.Click += MnuDirectoryDelete_Click;
            #endregion
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
            string name = Prompt.InputBox($"Enter the new directory path\n{item.SubItems[1].Text}", "Move Directory", item.SubItems[1].Text);

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
            string name = Prompt.InputBox("Enter name of new directory", "Create Directory");

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
            _client.Disconnect();
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
                    }
                    else
                    {
                        mnuDirectoryMove.Enabled = false;
                        mnuDirectoryDelete.Enabled = false;
                        mnuDirectoryCompress.Enabled = false;

                        mnuFileMove.Enabled = true;
                        mnuFileDelete.Enabled = true;
                        mnuFileCompress.Enabled = true;
                    }
                }
            }
        }
    }
}
