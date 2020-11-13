using Common;
using System;
using System.Windows.Forms;

namespace fft_2
{
    static class Program
    {
        public static Configuration Configuration = Configuration.FromFile("config.json", true);
        public static string APP_VERSION = "2.0.0";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logger.SetLogLevel(LogLevel.Debug, true);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
