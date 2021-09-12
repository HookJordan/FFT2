using Common.IO;
using Common.Security.Cryptography;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace fft_2
{
    public partial class frmBenchmark : Form
    {
        public frmBenchmark()
        {
            InitializeComponent();
        }

        private void frmBenchmark_Load(object sender, EventArgs e)
        {
            Text = $"FFT - Benchmark";
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            cbEncryptionMode.SelectedIndex = 0;
        }

        private void btnBenchmark_Click(object sender, EventArgs e)
        {
            btnBenchmark.Enabled = false;
            cbEncryptionMode.Enabled = false;
            numBuffer.Enabled = false;


            RunBenchmarks();
        }
        
        private CryptoServiceAlgorithm GetAlgorithm()
        {
            if (cbEncryptionMode.Text == "AES")
                return CryptoServiceAlgorithm.AES;
            else if (cbEncryptionMode.Text == "XOR")
                return CryptoServiceAlgorithm.XOR;
            else if (cbEncryptionMode.Text == "TripleDES")
                return CryptoServiceAlgorithm.TripleDES;
            else if (cbEncryptionMode.Text == "RC4")
                return CryptoServiceAlgorithm.RC4;
            else
                return CryptoServiceAlgorithm.Disabled;
        }

        private void RunBenchmarks()
        {
            int bufferSize = (int)(1024 * numBuffer.Value);
            CryptoServiceAlgorithm algorithm = GetAlgorithm();

            new Thread(() =>
            {
                CryptoService cryptoService = new CryptoService(algorithm, Guid.NewGuid().ToString());
                Random random = new Random();
                Stopwatch sw = new Stopwatch();
                byte[] buffer = new byte[bufferSize];
                long dataProcessed = 0;
                long duration = 3; // 1 second

                random.NextBytes(buffer);

                sw.Start();
                while (sw.ElapsedMilliseconds < duration * 1000)
                {
                    cryptoService.Encrypt(buffer);
                    dataProcessed += buffer.Length;
                }
                sw.Stop();

                buffer = cryptoService.Encrypt(buffer);

                Invoke((MethodInvoker)delegate
                {
                    lblEncryption.Text = $"Encryption: {Explorer.GetSize((double)dataProcessed / duration)}/S";
                });

                sw.Restart();

                while(sw.ElapsedMilliseconds < duration * 1000)
                {
                    cryptoService.Decrypt(buffer);
                    dataProcessed += buffer.Length;
                }

                sw.Stop();

                Invoke((MethodInvoker)delegate
                {
                    lblDec.Text = $"Decryption: {Explorer.GetSize((double)dataProcessed / duration)}/S";
                    btnBenchmark.Enabled = true;
                    numBuffer.Enabled = true;
                    cbEncryptionMode.Enabled = true;
                });

                buffer = null;
            }).Start();
        }
    }
}
