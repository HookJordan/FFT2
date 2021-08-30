using Common.IO;
using Common.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
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
            else if (cbEncryptionMode.Text == "DES")
                return CryptoServiceAlgorithm.DES;
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
                Random randomizer = new Random();
                Stopwatch sw = new Stopwatch();
                byte[] buffer = new byte[bufferSize];
                long dataProcessed = 0;

                while (sw.ElapsedMilliseconds < 1000)
                {
                    randomizer.NextBytes(buffer);

                    sw.Start();
                    buffer = cryptoService.Encrypt(buffer);
                    sw.Stop();

                    dataProcessed += buffer.Length;
                }

                Invoke((MethodInvoker)delegate
                {
                    lblEncryption.Text = $"Encryption: {Explorer.GetSize((double)dataProcessed)}/S";
                });

                sw.Reset();
                dataProcessed = 0;

                while (sw.ElapsedMilliseconds < 1000)
                {
                    randomizer.NextBytes(buffer);
                    buffer = cryptoService.Encrypt(buffer);

                    sw.Start();
                    buffer = cryptoService.Decrypt(buffer);
                    sw.Stop();

                    dataProcessed += buffer.Length;
                }

                Invoke((MethodInvoker)delegate
                {
                    lblDec.Text = $"Decryption: {Explorer.GetSize((double)dataProcessed)}/S";
                    btnBenchmark.Enabled = true;
                    numBuffer.Enabled = true;
                    cbEncryptionMode.Enabled = true;
                });
            }).Start();
        }

    }
}
