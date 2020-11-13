namespace fft_2
{
    partial class frmPreferences
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numBuffer = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbWindows = new System.Windows.Forms.CheckBox();
            this.cbDebug = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lstProtected = new System.Windows.Forms.ListBox();
            this.mnuProtected = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radEncAES = new System.Windows.Forms.RadioButton();
            this.radEncNone = new System.Windows.Forms.RadioButton();
            this.radEncRC4 = new System.Windows.Forms.RadioButton();
            this.radEncXOR = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.mnuProtected.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(420, 360);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(412, 332);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic Configuration";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numBuffer);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(52, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 159);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Networking";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(6, 125);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(298, 23);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.Text = "https://jordanhook.com";
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Preferred Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(284, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "KB";
            // 
            // numBuffer
            // 
            this.numBuffer.Location = new System.Drawing.Point(6, 81);
            this.numBuffer.Maximum = new decimal(new int[] {
            10240,
            0,
            0,
            0});
            this.numBuffer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBuffer.Name = "numBuffer";
            this.numBuffer.Size = new System.Drawing.Size(274, 23);
            this.numBuffer.TabIndex = 1;
            this.numBuffer.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Max Buffer Size:";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(6, 37);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(298, 23);
            this.numPort.TabIndex = 1;
            this.numPort.Value = new decimal(new int[] {
            15056,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Inbound Port:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbWindows);
            this.tabPage2.Controls.Add(this.cbDebug);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(412, 332);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced Options";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cbWindows
            // 
            this.cbWindows.AutoSize = true;
            this.cbWindows.Location = new System.Drawing.Point(58, 297);
            this.cbWindows.Name = "cbWindows";
            this.cbWindows.Size = new System.Drawing.Size(126, 19);
            this.cbWindows.TabIndex = 1;
            this.cbWindows.Text = "Start with windows";
            this.cbWindows.UseVisualStyleBackColor = true;
            // 
            // cbDebug
            // 
            this.cbDebug.AutoSize = true;
            this.cbDebug.Location = new System.Drawing.Point(261, 297);
            this.cbDebug.Name = "cbDebug";
            this.cbDebug.Size = new System.Drawing.Size(95, 19);
            this.cbDebug.TabIndex = 1;
            this.cbDebug.Text = "Debug Mode";
            this.cbDebug.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(52, 31);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 260);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Security";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lstProtected);
            this.groupBox4.Location = new System.Drawing.Point(6, 81);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(298, 173);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Protected Directories";
            // 
            // lstProtected
            // 
            this.lstProtected.ContextMenuStrip = this.mnuProtected;
            this.lstProtected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstProtected.FormattingEnabled = true;
            this.lstProtected.ItemHeight = 15;
            this.lstProtected.Location = new System.Drawing.Point(3, 19);
            this.lstProtected.Name = "lstProtected";
            this.lstProtected.Size = new System.Drawing.Size(292, 151);
            this.lstProtected.TabIndex = 0;
            // 
            // mnuProtected
            // 
            this.mnuProtected.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAdd,
            this.mnuRemove});
            this.mnuProtected.Name = "mnuProtected";
            this.mnuProtected.Size = new System.Drawing.Size(118, 48);
            // 
            // mnuAdd
            // 
            this.mnuAdd.Name = "mnuAdd";
            this.mnuAdd.Size = new System.Drawing.Size(117, 22);
            this.mnuAdd.Text = "Add";
            // 
            // mnuRemove
            // 
            this.mnuRemove.Name = "mnuRemove";
            this.mnuRemove.Size = new System.Drawing.Size(117, 22);
            this.mnuRemove.Text = "Remove";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radEncAES);
            this.groupBox3.Controls.Add(this.radEncNone);
            this.groupBox3.Controls.Add(this.radEncRC4);
            this.groupBox3.Controls.Add(this.radEncXOR);
            this.groupBox3.Location = new System.Drawing.Point(6, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(298, 53);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Encryption Algorithm";
            // 
            // radEncAES
            // 
            this.radEncAES.AutoSize = true;
            this.radEncAES.Checked = true;
            this.radEncAES.Location = new System.Drawing.Point(172, 22);
            this.radEncAES.Name = "radEncAES";
            this.radEncAES.Size = new System.Drawing.Size(66, 19);
            this.radEncAES.TabIndex = 0;
            this.radEncAES.TabStop = true;
            this.radEncAES.Text = "AES 256";
            this.radEncAES.UseVisualStyleBackColor = true;
            // 
            // radEncNone
            // 
            this.radEncNone.AutoSize = true;
            this.radEncNone.Location = new System.Drawing.Point(6, 22);
            this.radEncNone.Name = "radEncNone";
            this.radEncNone.Size = new System.Drawing.Size(54, 19);
            this.radEncNone.TabIndex = 0;
            this.radEncNone.Text = "None";
            this.radEncNone.UseVisualStyleBackColor = true;
            // 
            // radEncRC4
            // 
            this.radEncRC4.AutoSize = true;
            this.radEncRC4.Location = new System.Drawing.Point(120, 22);
            this.radEncRC4.Name = "radEncRC4";
            this.radEncRC4.Size = new System.Drawing.Size(46, 19);
            this.radEncRC4.TabIndex = 0;
            this.radEncRC4.Text = "RC4";
            this.radEncRC4.UseVisualStyleBackColor = true;
            // 
            // radEncXOR
            // 
            this.radEncXOR.AutoSize = true;
            this.radEncXOR.Location = new System.Drawing.Point(66, 22);
            this.radEncXOR.Name = "radEncXOR";
            this.radEncXOR.Size = new System.Drawing.Size(48, 19);
            this.radEncXOR.TabIndex = 0;
            this.radEncXOR.Text = "XOR";
            this.radEncXOR.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(353, 378);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(272, 378);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "&Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // frmPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 413);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPreferences";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.Load += new System.EventHandler(this.frmPreferences_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBuffer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.mnuProtected.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numBuffer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbWindows;
        private System.Windows.Forms.CheckBox cbDebug;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lstProtected;
        private System.Windows.Forms.RadioButton radEncAES;
        private System.Windows.Forms.RadioButton radEncNone;
        private System.Windows.Forms.RadioButton radEncRC4;
        private System.Windows.Forms.RadioButton radEncXOR;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip mnuProtected;
        private System.Windows.Forms.ToolStripMenuItem mnuAdd;
        private System.Windows.Forms.ToolStripMenuItem mnuRemove;
    }
}