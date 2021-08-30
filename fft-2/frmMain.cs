using Common;
using Common.IO;
using Common.Networking;
using Common.Security;
using Common.Updates;
using System;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace fft_2
{
    public partial class frmMain : Form
    {
        private frmBuddyList _frmBuddyList;
        private Server _server;
        private bool _closeToTray;
        private Updater _updater;
        private frmConnectionLogger _logger;

        public frmMain()
        {
            InitializeComponent();

            _frmBuddyList = new frmBuddyList();
            _closeToTray = true;
            _updater = new Updater();
            _logger = new frmConnectionLogger();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Text = $"FFT - FastFileTransfer {Program.APP_VERSION}";
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            // Setup UI
            ResetPassword();
            SetIpAddress();
            txtMyPort.Text = Program.Configuration.Port.ToString();
            txtPartnerIp.Text = "127.0.0.1";
            txtPartnerPassword.Text = "";

            // Create new instance of a server
            _server = new Server(int.Parse(txtMyPort.Text), txtMyPassword.Text, Program.Configuration.Password);
            _server.NewClient += Server_NewClient;

            // Start running server
            try
            { 
                _server.Listen();
                UpdateStatus("Awaiting new connections...");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Server socket in use, unable to accept new clients");
            }

            // Setup menu button handlers 
            mnuQuit.Click += MnuQuit_Click;
            mnuPreferences.Click += MnuPreferences_Click;
            mnuBuddyList.Visible = false;
            mnuBuddyList.Click += MnuBuddyList_Click;
            mnuLogger.Click += MnuLogger_Click;
            mnuBenchmark.Click += MnuBenchmark_Click;

            // Icon buttons
            mnuIconQuit.Click += MnuQuit_Click;
            mnuIconPreferences.Click += MnuPreferences_Click;
            mnuIconOpen.Click += MnuIconOpen_Click;
            icnMain.DoubleClick += IcnMain_DoubleClick;
            mnuIconCheckForUpdates.Click += MnuIconCheckForUpdates_Click;

            // Help buttons
            mnuAbout.Click += MnuAbout_Click;
            mnuUpdates.Click += MnuUpdates_Click;

            // Initial launch update check
            UpdateCheck(false);
        }

        private void MnuBenchmark_Click(object sender, EventArgs e)
        {
            using (frmBenchmark bench = new frmBenchmark())
            {
                bench.ShowDialog();
            }
        }

        private void MnuLogger_Click(object sender, EventArgs e)
        {
            _logger.Show();
            _logger.BringToFront();
        }

        private void MnuIconCheckForUpdates_Click(object sender, EventArgs e)
        {
            UpdateCheck(true);
        }

        private void MnuUpdates_Click(object sender, EventArgs e)
        {
            mnuIconCheckForUpdates.PerformClick();
        }

        private void MnuAbout_Click(object sender, EventArgs e)
        {
            using (frmAbout about = new frmAbout())
            {
                about.ShowDialog();
            }
        }

        private void IcnMain_DoubleClick(object sender, EventArgs e)
        {
            Show();
            BringToFront();
        }

        private void MnuIconOpen_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void MnuBuddyList_Click(object sender, EventArgs e)
        {
            _frmBuddyList.Display();
        }

        private void MnuPreferences_Click(object sender, EventArgs e)
        {
            using (frmPreferences pref = new frmPreferences())
            {
                pref.ShowDialog();

                if (Program.Configuration.Port != _server.Port)
                {
                    _server.SetPort(Program.Configuration.Port);
                    txtMyPort.Text = Program.Configuration.Port.ToString();
                }

                if (Program.Configuration.Password != _server.Password)
                {
                    _server.SetPersonalPassword(Program.Configuration.Password);
                }
            }
        }

        private void MnuQuit_Click(object sender, EventArgs e)
        {
            _closeToTray = false;
            _frmBuddyList.Dispose();
            _logger.Dispose();

            // TODO: If session in progress -- warn about ending session
            Application.Exit();
        }

        private void Server_NewClient(Server server, Client client)
        {
            new Thread(() =>
            { 
                if (client.Protocol == Protocol.FFTSI)
                {
                    // Handle events on the new client
                    client.Disconnected += Client_Disconnected1;
                    client.PacketReceived += Client_PacketReceived1;

                    // Create file explorer session
                    Explorer explorer = new Explorer(client, Program.Configuration.ProtectedDirectories.ToArray());

                    // Start receiving data
                    client.Connect(Program.Configuration.MaxBufferSize);

                    UpdateStatus($"Connected - {client}");

                    icnMain.ShowBalloonTip(5000, "FastFileTransfer", $"New inbound connection from {client.IP}:{client.Port}", ToolTipIcon.Info);
                }
                else
                {
                    // Transfer socket connection
                    FileTransferManager ftm = new FileTransferManager(client, Program.Configuration.MaxBufferSize);
                    client.Connect(Program.Configuration.MaxBufferSize);
                }
            }).Start();
        }

        private void UpdateStatus(string status)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { UpdateStatus(status); }); 
            }
            else
            {
                lblStatus.Text = status;
            }
        }

        private void Client_PacketReceived1(Client client, Packet packet)
        {
            if (packet.PacketHeader == PacketHeader.Ping)
            {
                packet.PacketHeader = PacketHeader.Pong;
                client.Transmit(packet);
            }
        }

        private void Client_Disconnected1(Client client)
        {
            UpdateStatus("Awaiting new connections...");
            client.Dispose();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetPassword();
        }

        private void ResetPassword()
        {
            string password = Guid.NewGuid().ToString().Replace("-", "").ToUpper().Substring(0, 8);
            txtMyPassword.Text = password;

            // Update server configuration as well
            if (_server != null && _server.Running)
            {
                _server.SetPassword(password);
            }
        }

        private void SetIpAddress()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Proxy = null;
                    txtMyIp.Text = wc.DownloadString("https://api.ipify.org");
                }
            }
            catch (Exception e)
            {
                Logger.Error("Failed to obtain ip address from api.ipify.org");
                Logger.Debug(e.Message);

                txtMyIp.Text = "Failed to obtain...";
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (txtPartnerPassword.TextLength == 0)
                MessageBox.Show("A partner password must be set in the connect to section in order to establish a new connection", "Invalid Password",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                btnConnect.Enabled = false;
                Client client = new Client(Protocol.FFTSI, txtPartnerIp.Text, (int)numPartnerPort.Value, Hashing.SHA(txtPartnerPassword.Text), Program.Configuration.CryptoServiceAlgorithm);

                // Setup events for client
                client.Disconnected += Client_Disconnected;
                client.PacketReceived += Client_PacketReceived;
                client.Ready += Client_Ready;

                frmFileExplorer frmFileExplorer = new frmFileExplorer(client);

                try 
                {
                    client.Connect(Program.Configuration.MaxBufferSize);
                    frmFileExplorer.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to establish a secure connection. Please try agian.", "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Error(ex.Message);
                    EnableButton();
                    frmFileExplorer.Dispose();
                }
            }
        }

        private void Client_Ready(Client client)
        {
            client.Transmit(new Packet(PacketHeader.Ping, Environment.TickCount.ToString()));
        }

        private void Client_PacketReceived(Client client, Packet packet)
        {
            if (packet.PacketHeader == PacketHeader.Pong)
            {
                long now = Environment.TickCount, before = long.Parse(packet.PayloadAsString());
                Logger.Info($"Ping completed  RTT={now - before}MS");
            }
        }

        private void Client_Disconnected(Client client)
        {
            client.Dispose();
            EnableButton();
        }

        public void EnableButton()
        {
            if (btnConnect.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    EnableButton();
                });
            }
            else
            {
                btnConnect.Enabled = true;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_closeToTray)
            {
                e.Cancel = _closeToTray;
                Hide();

                icnMain.ShowBalloonTip(5000, "FastFileTransfer", "FastFileTransfer is still running in the background.", ToolTipIcon.Info);
            }
        }

        private void UpdateCheck(bool isManualCheck = false)
        {
            UpdatePackage update = _updater.UpdateCheck();

            // Determine is target version is > current version
            if (_updater.IsNewVersion(Program.APP_VERSION, update.version))
            {
                if (MessageBox.Show($"A newer version of FastFileTransfer has been found. Would you like to download version: {update.version}?", "Checking For Updates", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // SHOW GATE UNTIL DOWNLOAD COMPELTED
                    string targetFile = System.IO.Path.Combine(Application.StartupPath, update.package.Replace("apps/", ""));
                    string message = $"Current Version: {Program.APP_VERSION}\nTarget Version: {update.version}\n\nDestination:\n{targetFile}";

                    using (dlgLoadingGate dlg = new dlgLoadingGate("Downloading Update", message))
                    {
                        using (UpdateDownloader ud = new UpdateDownloader(targetFile, update))
                        {
                            dlg.Show();
                            ud.Download();
                            while (!ud.DownloadComplete)
                            {
                                Application.DoEvents();
                            }

                            dlg.CloseDialog();

                            // TODO: It would be nice to extract and auto install the updates
                            // However, we will need to write or utilize an installation wizard
                            MessageBox.Show($"The updated package has been downloaded to:\n\n{targetFile}\n\nPlease extract it to complete the update!", "Update Downloaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else if (isManualCheck)
            {
                MessageBox.Show($"FastFileTransfer is up to date with version {Program.APP_VERSION}.", "Checking For Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
