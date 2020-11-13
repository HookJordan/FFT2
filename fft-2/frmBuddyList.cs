using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace fft_2
{

    public partial class frmBuddyList : Form
    {
        public frmBuddyList()
        {
            InitializeComponent();

            FormClosing += FrmBuddyList_FormClosing;
        }

        private void FrmBuddyList_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void frmBuddyList_Load(object sender, EventArgs e)
        {
            Text = $"FFT - Buddy List";
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            
        }

        public void Display()
        {
            Show();
        }

        private void tmrBuddyCheck_Tick(object sender, EventArgs e)
        {
            foreach (var item in lstBuddy.Items)
            {

            }
        }
    }
}
