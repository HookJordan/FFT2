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
                                                lstIcons.Images.Add(icon);
                                                item.ImageIndex = lstIcons.Images.Count - 1;
                                            }
                                        }
                                    }

                                    lstFiles.Items.Add(item);
                                }
                                if (resize) lstFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

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
    }
}
