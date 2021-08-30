using System;
using System.Drawing;
using System.Windows.Forms;

namespace fft_2
{
    partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                return "FFT - FastFileTransfer";
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Program.APP_VERSION;
            }
        }

        public string AssemblyDescription
        {
            get
            {
                return "FastFileTransfer is a .NET Core 3.0 application that implements it's own file transfer protocols to provide file management over a network connection; WAN or LAN. The purpose of this application was to further develop my skills such as networking, data streams and file manipulation. The application utilizes a TCP connection between the file host (server) and the client in order to provide directory listings, directory management and even the ability to download files.";
            }
        }

        public string AssemblyProduct
        {
            get
            {
                return AssemblyTitle;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                return "© Jordan Hook 2020";
            }
        }

        public string AssemblyCompany
        {
            get
            {
                return "https://jordanhook.com";
            }
        }
        #endregion

        private void frmAbout_Load(object sender, EventArgs e)
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }
    }
}
