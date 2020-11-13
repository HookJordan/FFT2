using System;
using System.Net;
using System.Text.Json;

namespace Common.Updates
{
    public class Updater : IDisposable
    {
        private const string UPDATE_CHECK_URL = "https://jordanhook.com/index.php?&controller=api&view=updater&appId=15";

        public bool IsDisposed { get; private set; }
        private WebClient _webClient;

        public Updater()
        {
            IsDisposed = false;
            _webClient = new WebClient();
        }

        public UpdatePackage UpdateCheck()
        {
            try
            {
                Logger.Info($"Checking for updates");
                string raw = _webClient.DownloadString(UPDATE_CHECK_URL);

                return JsonSerializer.Deserialize<UpdatePackage>(raw);
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to to connect to update server.");
                Logger.Debug(ex.Message);

                return null;
            }
        }

        public bool IsNewVersion(string currentVersion, string targetVersion)
        {
            int current = int.Parse(currentVersion.Replace(".", ""));
            int target = int.Parse(targetVersion.Replace(".", ""));

            return target > current;
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
