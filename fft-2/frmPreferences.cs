using Common.Security.Cryptography;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace fft_2
{
    public partial class frmPreferences : Form
    {
        public frmPreferences()
        {
            InitializeComponent();
        }

        private void frmPreferences_Load(object sender, EventArgs e)
        {
            Text = $"FFT - Preferences";
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            // Load default settings into UI
            numPort.Value = Program.Configuration.Port;
            numBuffer.Value = Program.Configuration.MaxBufferSize / 1024;
            txtPassword.Text = Program.Configuration.Password;
            SetCryptoServiceAlgorithm(Program.Configuration.CryptoServiceAlgorithm);
            cbDebug.Checked = Program.Configuration.LogLevel == Common.LogLevel.Debug;
            lstProtected.Items.AddRange(Program.Configuration.ProtectedDirectories.ToArray());
            cbWindows.Checked = Program.Configuration.StartWithWindows;

            // Events
            FormClosing += FrmPreferences_FormClosing;
            mnuAdd.Click += MnuAdd_Click;
            mnuRemove.Click += MnuRemove_Click;
        }

        private void MnuRemove_Click(object sender, EventArgs e)
        {
            if (lstProtected.SelectedItems.Count > 0)
            {
                var dir = (string)lstProtected.SelectedItem;
                if (MessageBox.Show($"By removing this protected folder, remotely connected users will be able to access and modify it's contents. Are you sure you wish to remove:\n'{dir}'?", "Remove Protected Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    lstProtected.Items.Remove(lstProtected.SelectedItem);
                }
            }
        }

        private void MnuAdd_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK && !lstProtected.Items.Contains(fbd.SelectedPath))
                {
                    lstProtected.Items.Add(fbd.SelectedPath);
                }
            }
        }

        private void FrmPreferences_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                // Copy values into Program.Configuration
                Program.Configuration.MaxBufferSize = (long)numBuffer.Value * 1024;
                Program.Configuration.Port = (int)numPort.Value;
                Program.Configuration.Password = txtPassword.Text;
                Program.Configuration.CryptoServiceAlgorithm = GetCryptoServiceAlgorithm();
                Program.Configuration.LogLevel = cbDebug.Checked ? Common.LogLevel.Debug : Common.LogLevel.Info;
                Program.Configuration.ProtectedDirectories = lstProtected.Items.Cast<string>().ToList();

                if (Program.Configuration.StartWithWindows != cbWindows.Checked)
                {
                    using (var install = new Installation.Startup("FastFileTransfer", Application.ExecutablePath))
                    {
                        if (cbWindows.Checked)
                        {
                            install.Install();
                        }
                        else
                        {
                            install.Uninstall();
                        }
                    }
                }

                // Persist
                Program.Configuration.Save("config.json", true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '\0';
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
        }

        private CryptoServiceAlgorithm GetCryptoServiceAlgorithm()
        {
            if (radEncAES.Checked)
                return CryptoServiceAlgorithm.AES;
            else if (radEncRC4.Checked)
                return CryptoServiceAlgorithm.RC4;
            else if (radEncXOR.Checked)
                return CryptoServiceAlgorithm.XOR;
            else
                return CryptoServiceAlgorithm.Disabled;
        }

        private void SetCryptoServiceAlgorithm(CryptoServiceAlgorithm alg)
        {
            if (alg == CryptoServiceAlgorithm.AES)
                radEncAES.Checked = true;
            else if (alg == CryptoServiceAlgorithm.RC4)
                radEncRC4.Checked = true;
            else if (alg == CryptoServiceAlgorithm.XOR)
                radEncXOR.Checked = true;
            else
                radEncNone.Checked = true;
        }
    }
}
