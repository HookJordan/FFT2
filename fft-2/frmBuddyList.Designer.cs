namespace fft_2
{
    partial class frmBuddyList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lstBuddy = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.tmrBuddyCheck = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lstBuddy
            // 
            this.lstBuddy.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstBuddy.FullRowSelect = true;
            this.lstBuddy.HideSelection = false;
            this.lstBuddy.Location = new System.Drawing.Point(12, 12);
            this.lstBuddy.MultiSelect = false;
            this.lstBuddy.Name = "lstBuddy";
            this.lstBuddy.Size = new System.Drawing.Size(320, 224);
            this.lstBuddy.TabIndex = 0;
            this.lstBuddy.UseCompatibleStateImageBehavior = false;
            this.lstBuddy.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "#";
            this.columnHeader1.Width = 30;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Nickname";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Status";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 120;
            // 
            // tmrBuddyCheck
            // 
            this.tmrBuddyCheck.Enabled = true;
            this.tmrBuddyCheck.Interval = 60000;
            this.tmrBuddyCheck.Tick += new System.EventHandler(this.tmrBuddyCheck_Tick);
            // 
            // frmBuddyList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 248);
            this.Controls.Add(this.lstBuddy);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBuddyList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBuddyList";
            this.Load += new System.EventHandler(this.frmBuddyList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstBuddy;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Timer tmrBuddyCheck;
    }
}