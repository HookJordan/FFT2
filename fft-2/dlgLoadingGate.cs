using System;
using System.Drawing;
using System.Windows.Forms;

namespace fft_2
{
    public partial class dlgLoadingGate : Form
    {
        public bool KeepOpen { get; private set; }

        public dlgLoadingGate(string title, string message)
        {
            InitializeComponent();

            Text = title;
            lblMessage.Text = message;
            KeepOpen = true;
        }

        private void dlgLoadingGate_Load(object sender, EventArgs e)
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            FormClosing += DlgLoadingGate_FormClosing;
        }

        private void DlgLoadingGate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = KeepOpen;
        }

        public void AllowClose()
        {
            KeepOpen = false;
        }

        public void CloseDialog()
        {
            AllowClose();
            Close();
        }
    }
}
