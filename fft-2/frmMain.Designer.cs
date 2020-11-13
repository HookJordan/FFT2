namespace fft_2
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMyPassword = new System.Windows.Forms.TextBox();
            this.txtMyPort = new System.Windows.Forms.TextBox();
            this.txtMyIp = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBuddyList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numPartnerPort = new System.Windows.Forms.NumericUpDown();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtPartnerPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPartnerIp = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.icnMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.mnuIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuIconOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIconCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIconPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuIconQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPartnerPort)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.mnuIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtMyPassword);
            this.groupBox1.Controls.Add(this.txtMyPort);
            this.groupBox1.Controls.Add(this.txtMyIp);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 186);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Your Information";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(209, 154);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP Address:";
            // 
            // txtMyPassword
            // 
            this.txtMyPassword.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtMyPassword.Location = new System.Drawing.Point(6, 125);
            this.txtMyPassword.Name = "txtMyPassword";
            this.txtMyPassword.ReadOnly = true;
            this.txtMyPassword.Size = new System.Drawing.Size(278, 23);
            this.txtMyPassword.TabIndex = 0;
            this.txtMyPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMyPort
            // 
            this.txtMyPort.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtMyPort.Location = new System.Drawing.Point(6, 81);
            this.txtMyPort.Name = "txtMyPort";
            this.txtMyPort.ReadOnly = true;
            this.txtMyPort.Size = new System.Drawing.Size(278, 23);
            this.txtMyPort.TabIndex = 0;
            this.txtMyPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMyIp
            // 
            this.txtMyIp.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtMyIp.Location = new System.Drawing.Point(6, 37);
            this.txtMyIp.Name = "txtMyIp";
            this.txtMyIp.ReadOnly = true;
            this.txtMyIp.Size = new System.Drawing.Size(278, 23);
            this.txtMyIp.TabIndex = 0;
            this.txtMyIp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStrip,
            this.helpToolStrip});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStrip
            // 
            this.fileToolStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBuddyList,
            this.mnuPreferences,
            this.toolStripSeparator1,
            this.mnuQuit});
            this.fileToolStrip.Name = "fileToolStrip";
            this.fileToolStrip.Size = new System.Drawing.Size(37, 20);
            this.fileToolStrip.Text = "File";
            // 
            // mnuBuddyList
            // 
            this.mnuBuddyList.Name = "mnuBuddyList";
            this.mnuBuddyList.Size = new System.Drawing.Size(135, 22);
            this.mnuBuddyList.Text = "Buddy List";
            // 
            // mnuPreferences
            // 
            this.mnuPreferences.Name = "mnuPreferences";
            this.mnuPreferences.Size = new System.Drawing.Size(135, 22);
            this.mnuPreferences.Text = "Preferences";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(132, 6);
            // 
            // mnuQuit
            // 
            this.mnuQuit.Name = "mnuQuit";
            this.mnuQuit.Size = new System.Drawing.Size(135, 22);
            this.mnuQuit.Text = "Quit";
            // 
            // helpToolStrip
            // 
            this.helpToolStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout,
            this.mnuUpdates});
            this.helpToolStrip.Name = "helpToolStrip";
            this.helpToolStrip.Size = new System.Drawing.Size(44, 20);
            this.helpToolStrip.Text = "Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(170, 22);
            this.mnuAbout.Text = "About";
            // 
            // mnuUpdates
            // 
            this.mnuUpdates.Name = "mnuUpdates";
            this.mnuUpdates.Size = new System.Drawing.Size(170, 22);
            this.mnuUpdates.Text = "Check for updates";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numPartnerPort);
            this.groupBox2.Controls.Add(this.btnConnect);
            this.groupBox2.Controls.Add(this.txtPartnerPassword);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtPartnerIp);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(322, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(290, 186);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connect To";
            // 
            // numPartnerPort
            // 
            this.numPartnerPort.Location = new System.Drawing.Point(6, 77);
            this.numPartnerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPartnerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPartnerPort.Name = "numPartnerPort";
            this.numPartnerPort.Size = new System.Drawing.Size(278, 23);
            this.numPartnerPort.TabIndex = 3;
            this.numPartnerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numPartnerPort.Value = new decimal(new int[] {
            15056,
            0,
            0,
            0});
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(209, 154);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtPartnerPassword
            // 
            this.txtPartnerPassword.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtPartnerPassword.Location = new System.Drawing.Point(6, 125);
            this.txtPartnerPassword.Name = "txtPartnerPassword";
            this.txtPartnerPassword.Size = new System.Drawing.Size(278, 23);
            this.txtPartnerPassword.TabIndex = 0;
            this.txtPartnerPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Port:";
            // 
            // txtPartnerIp
            // 
            this.txtPartnerIp.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtPartnerIp.Location = new System.Drawing.Point(6, 37);
            this.txtPartnerIp.Name = "txtPartnerIp";
            this.txtPartnerIp.Size = new System.Drawing.Size(278, 23);
            this.txtPartnerIp.TabIndex = 0;
            this.txtPartnerIp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "IP Address:";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 226);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(624, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(118, 17);
            this.lblStatus.Text = "toolStripStatusLabel1";
            // 
            // icnMain
            // 
            this.icnMain.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.icnMain.ContextMenuStrip = this.mnuIcon;
            this.icnMain.Icon = ((System.Drawing.Icon)(resources.GetObject("icnMain.Icon")));
            this.icnMain.Text = "FFT - FastFileTransfer";
            this.icnMain.Visible = true;
            // 
            // mnuIcon
            // 
            this.mnuIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuIconOpen,
            this.mnuIconCheckForUpdates,
            this.mnuIconPreferences,
            this.toolStripSeparator2,
            this.mnuIconQuit});
            this.mnuIcon.Name = "mnuIcon";
            this.mnuIcon.Size = new System.Drawing.Size(171, 98);
            // 
            // mnuIconOpen
            // 
            this.mnuIconOpen.Name = "mnuIconOpen";
            this.mnuIconOpen.Size = new System.Drawing.Size(170, 22);
            this.mnuIconOpen.Text = "Open";
            // 
            // mnuIconCheckForUpdates
            // 
            this.mnuIconCheckForUpdates.Name = "mnuIconCheckForUpdates";
            this.mnuIconCheckForUpdates.Size = new System.Drawing.Size(170, 22);
            this.mnuIconCheckForUpdates.Text = "Check for updates";
            // 
            // mnuIconPreferences
            // 
            this.mnuIconPreferences.Name = "mnuIconPreferences";
            this.mnuIconPreferences.Size = new System.Drawing.Size(170, 22);
            this.mnuIconPreferences.Text = "Preferences";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(167, 6);
            // 
            // mnuIconQuit
            // 
            this.mnuIconQuit.Name = "mnuIconQuit";
            this.mnuIconQuit.Size = new System.Drawing.Size(170, 22);
            this.mnuIconQuit.Text = "Quit";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 248);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FFT - FastFileTransfer 2.0.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPartnerPort)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mnuIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mnuQuit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem helpToolStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuPreferences;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TextBox txtMyPort;
        private System.Windows.Forms.TextBox txtMyIp;
        private System.Windows.Forms.TextBox txtMyPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtPartnerPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPartnerIp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numPartnerPort;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdates;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripMenuItem mnuBuddyList;
        private System.Windows.Forms.NotifyIcon icnMain;
        private System.Windows.Forms.ContextMenuStrip mnuIcon;
        private System.Windows.Forms.ToolStripMenuItem mnuIconOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuIconCheckForUpdates;
        private System.Windows.Forms.ToolStripMenuItem mnuIconPreferences;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuIconQuit;
    }
}

