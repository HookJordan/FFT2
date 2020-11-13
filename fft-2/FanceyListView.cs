using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace fft_2
{
    public class FanceyListView : ListView
    {
        [DllImport("uxtheme", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string textSubAppName, string textSubIdList);
        public FanceyListView()
        {
            DoubleBuffered = true;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            // Using windows explorer style listview 
            // It looks nicer
            SetWindowTheme(Handle, "explorer", null);
            base.OnHandleCreated(e);
        }

    }
}
