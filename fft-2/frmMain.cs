using Common;
using Common.IO;
using Common.Networking;
using Common.Security;
using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace fft_2
{
    public partial class frmMain : Form
    {
        private Server Server;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Text = $"FFT - FastFileTransfer {Program.APP_VERSION}";
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            // TODO: Load from config? 
            ResetPassword();
            SetIpAddress();
            txtMyPort.Text = "15056";
            txtPartnerIp.Text = "127.0.0.1";
            txtPartnerPassword.Text = "";

            // Create new instance of a server
            Server = new Server(int.Parse(txtMyPort.Text), txtMyPassword.Text);
            Server.NewClient += Server_NewClient;

            // Start running server
            try
            {

                Server.Listen();
                lblServerStatus.Text = "Awaiting new connections...";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblServerStatus.Text = "Server socket in use, unable to accept new clients";
            }

            // Setup menu button handlers 
            mnuQuit.Click += MnuQuit_Click;
        }

        private void MnuQuit_Click(object sender, EventArgs e)
        {
            // TODO: If session in progress -- warn about ending session
            Application.Exit();
        }

        private void Server_NewClient(Server server, Client client)
        {
            lblServerStatus.Text = $"Connected - {client.ToString().Split("Password")[0]}Password=...}}";

            // Handle events on the new client
            client.Disconnected += Client_Disconnected1;
            client.PacketReceived += Client_PacketReceived1;

            // Create file explorer session
            Explorer explorer = new Explorer(client);

            // Start receiving data
            client.Connect();
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
            lblServerStatus.Text = "Awaiting new connections...";
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
            if (Server != null && Server.Running)
            {
                Server.SetPassword(password);
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
                Client client = new Client(Protocol.FFTSI, txtPartnerIp.Text, (int)numPartnerPort.Value, Hashing.SHA(txtPartnerPassword.Text), Common.Security.Cryptography.CryptoServiceAlgorithm.AES);

                // Setup events for client
                client.Disconnected += Client_Disconnected;
                client.PacketReceived += Client_PacketReceived;
                client.Ready += Client_Ready;


                frmFileExplorer frmFileExplorer = new frmFileExplorer(client);

                try 
                {
                    client.Connect();
                    frmFileExplorer.Show();
                }
                catch (Exception ex)
                {
                    btnConnect.Enabled = true;
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
                Logger.Info($"Ping pong completed in {now - before}MS");
            }
        }

        private void Client_Disconnected(Client client)
        {
            client.Dispose();
            btnConnect.Enabled = true;
        }
    }
}
