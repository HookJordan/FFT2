using Common;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace fft_2
{
    public partial class frmConnectionLogger : Form
    {
        public frmConnectionLogger()
        {
            InitializeComponent();

            rtxtLog.TextChanged += RtxtLog_TextChanged;
            FormClosing += FrmConnectionLogger_FormClosing;

            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Text = $"FFT - Logs";

            // Handle log events
            Logger.Get.WriteLineEvent += HandleLogs;
        }

        private void FrmConnectionLogger_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void RtxtLog_TextChanged(object sender, EventArgs e)
        {
            PrettyPrint();
        }

        private void HandleLogs(Logger logger, string log)
        {
            WriteText(log);
        }

        private void PrettyPrint()
        {
            // Log highlighting
            foreach (Match match in Regex.Matches(rtxtLog.Text, @"ERROR"))
            {
                SetColor(match, Color.Red);
            }

            foreach (Match match in Regex.Matches(rtxtLog.Text, @"INFO"))
            {
                SetColor(match, Color.Green);
            }

            foreach (Match match in Regex.Matches(rtxtLog.Text, @"DEBUG"))
            {
                SetColor(match, Color.Blue);
            }

            rtxtLog.SelectionStart = rtxtLog.Text.Length;
            rtxtLog.ScrollToCaret();
        }

        private void SetColor(Match match, Color color)
        {
            rtxtLog.Select(match.Index, match.Length);
            rtxtLog.SelectionColor = color;
        }

        public void WriteText(string text)
        {
            // Invoke is required as we will be updating from a different thread
            if (rtxtLog.InvokeRequired)
            {
                Action safeWrite = delegate { WriteText(text); };
                rtxtLog?.BeginInvoke(safeWrite);
            }
            else
            {
                rtxtLog.AppendText(text + Environment.NewLine);
            }
        }

        private void frmConnectionLogger_Load(object sender, EventArgs e)
        {

        }

        private void openLogFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", Logger.Get.GetLogsFolder());
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = ".txt|*.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(sfd.FileName, rtxtLog.Text);

                    MessageBox.Show($"Your logs have been saved to the file:\n\n{sfd.FileName}", "Export Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxtLog.Clear();
        }
    }
}
