using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Common.Updates
{
    public class UpdateDownloader : IDisposable
    {
        private const string ENDPOINT = "https://jordanhook.com/index.php?&controller=api&view=download&appId=15";

        public string Destination { get; private set; }
        public UpdatePackage UpdatePackage { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool DownloadComplete { get; private set; }
     
        private WebClient _webClient;

        public UpdateDownloader(string dst, UpdatePackage updatePackage)
        {
            // Setup download client
            _webClient = new WebClient();
            _webClient.DownloadFileCompleted += _webClient_DownloadFileCompleted;

            // Setup source and destination
            Destination = dst;
            UpdatePackage = updatePackage;
            IsDisposed = false;
            DownloadComplete = false;
        }

        private void _webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            new System.Threading.Thread(() =>
            {
                System.Threading.Thread.Sleep(1500);
                DownloadComplete = true;
            }).Start();
        }

        public void Download()
        {
            _webClient.DownloadFileAsync(new Uri(ENDPOINT), Destination);
        }


        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                _webClient.Dispose();
            }
        }
    }
}
