namespace fft_2
{
    partial class frmFileExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFileExplorer));
            this.label1 = new System.Windows.Forms.Label();
            this.txtNav = new System.Windows.Forms.TextBox();
            this.lstFiles = new fft_2.FanceyListView();
            this.mnuExplore = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDirectoryCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDirectoryMove = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDirectoryDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDirectoryCompress = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileMove = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileCompress = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.lstIcons = new System.Windows.Forms.ImageList(this.components);
            this.lstTransfers = new fft_2.FanceyListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.mnuTransfers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuTransferPause = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTransferCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExplore.SuspendLayout();
            this.mnuTransfers.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Location:";
            // 
            // txtNav
            // 
            this.txtNav.Location = new System.Drawing.Point(74, 12);
            this.txtNav.Name = "txtNav";
            this.txtNav.Size = new System.Drawing.Size(760, 23);
            this.txtNav.TabIndex = 1;
            // 
            // lstFiles
            // 
            this.lstFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstFiles.ContextMenuStrip = this.mnuExplore;
            this.lstFiles.FullRowSelect = true;
            this.lstFiles.HideSelection = false;
            this.lstFiles.LargeImageList = this.lstIcons;
            this.lstFiles.Location = new System.Drawing.Point(12, 41);
            this.lstFiles.MultiSelect = false;
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(822, 370);
            this.lstFiles.SmallImageList = this.lstIcons;
            this.lstFiles.TabIndex = 0;
            this.lstFiles.UseCompatibleStateImageBehavior = false;
            this.lstFiles.View = System.Windows.Forms.View.Details;
            // 
            // mnuExplore
            // 
            this.mnuExplore.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRefresh,
            this.toolStripSeparator1,
            this.mnuDirectory,
            this.mnuFile});
            this.mnuExplore.Name = "mnuExplore";
            this.mnuExplore.Size = new System.Drawing.Size(123, 76);
            this.mnuExplore.Opening += new System.ComponentModel.CancelEventHandler(this.mnuExplore_Opening);
            // 
            // mnuRefresh
            // 
            this.mnuRefresh.Name = "mnuRefresh";
            this.mnuRefresh.Size = new System.Drawing.Size(122, 22);
            this.mnuRefresh.Text = "Refresh";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
            // 
            // mnuDirectory
            // 
            this.mnuDirectory.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDirectoryCreate,
            this.mnuDirectoryMove,
            this.mnuDirectoryDelete,
            this.toolStripSeparator2,
            this.mnuDirectoryCompress});
            this.mnuDirectory.Name = "mnuDirectory";
            this.mnuDirectory.Size = new System.Drawing.Size(122, 22);
            this.mnuDirectory.Text = "Directory";
            // 
            // mnuDirectoryCreate
            // 
            this.mnuDirectoryCreate.Name = "mnuDirectoryCreate";
            this.mnuDirectoryCreate.Size = new System.Drawing.Size(127, 22);
            this.mnuDirectoryCreate.Text = "Create";
            // 
            // mnuDirectoryMove
            // 
            this.mnuDirectoryMove.Name = "mnuDirectoryMove";
            this.mnuDirectoryMove.Size = new System.Drawing.Size(127, 22);
            this.mnuDirectoryMove.Text = "Move";
            // 
            // mnuDirectoryDelete
            // 
            this.mnuDirectoryDelete.Name = "mnuDirectoryDelete";
            this.mnuDirectoryDelete.Size = new System.Drawing.Size(127, 22);
            this.mnuDirectoryDelete.Text = "Delete";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(124, 6);
            // 
            // mnuDirectoryCompress
            // 
            this.mnuDirectoryCompress.Name = "mnuDirectoryCompress";
            this.mnuDirectoryCompress.Size = new System.Drawing.Size(127, 22);
            this.mnuDirectoryCompress.Text = "Compress";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileMove,
            this.mnuFileDelete,
            this.toolStripSeparator3,
            this.mnuFileCompress,
            this.toolStripSeparator4,
            this.mnuDownload,
            this.mnuUpload});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(122, 22);
            this.mnuFile.Text = "File";
            // 
            // mnuFileMove
            // 
            this.mnuFileMove.Name = "mnuFileMove";
            this.mnuFileMove.Size = new System.Drawing.Size(128, 22);
            this.mnuFileMove.Text = "Move";
            // 
            // mnuFileDelete
            // 
            this.mnuFileDelete.Name = "mnuFileDelete";
            this.mnuFileDelete.Size = new System.Drawing.Size(128, 22);
            this.mnuFileDelete.Text = "Delete";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(125, 6);
            // 
            // mnuFileCompress
            // 
            this.mnuFileCompress.Name = "mnuFileCompress";
            this.mnuFileCompress.Size = new System.Drawing.Size(128, 22);
            this.mnuFileCompress.Text = "Compress";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(125, 6);
            // 
            // mnuDownload
            // 
            this.mnuDownload.Name = "mnuDownload";
            this.mnuDownload.Size = new System.Drawing.Size(128, 22);
            this.mnuDownload.Text = "Download";
            // 
            // mnuUpload
            // 
            this.mnuUpload.Name = "mnuUpload";
            this.mnuUpload.Size = new System.Drawing.Size(128, 22);
            this.mnuUpload.Text = "Upload";
            // 
            // lstIcons
            // 
            this.lstIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.lstIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("lstIcons.ImageStream")));
            this.lstIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.lstIcons.Images.SetKeyName(0, "drive.png");
            // 
            // lstTransfers
            // 
            this.lstTransfers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstTransfers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.lstTransfers.ContextMenuStrip = this.mnuTransfers;
            this.lstTransfers.FullRowSelect = true;
            this.lstTransfers.HideSelection = false;
            this.lstTransfers.Location = new System.Drawing.Point(12, 417);
            this.lstTransfers.MultiSelect = false;
            this.lstTransfers.Name = "lstTransfers";
            this.lstTransfers.Size = new System.Drawing.Size(822, 191);
            this.lstTransfers.TabIndex = 2;
            this.lstTransfers.UseCompatibleStateImageBehavior = false;
            this.lstTransfers.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Type";
            this.columnHeader1.Width = 66;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Status";
            this.columnHeader2.Width = 105;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Local Path";
            this.columnHeader3.Width = 124;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Remote Path";
            this.columnHeader4.Width = 124;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "File Size";
            this.columnHeader5.Width = 75;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Sent";
            this.columnHeader6.Width = 84;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Speed (Sec)";
            this.columnHeader7.Width = 84;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Progress";
            this.columnHeader8.Width = 150;
            // 
            // mnuTransfers
            // 
            this.mnuTransfers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTransferPause,
            this.mnuTransferCancel});
            this.mnuTransfers.Name = "mnuTransfers";
            this.mnuTransfers.Size = new System.Drawing.Size(167, 48);
            this.mnuTransfers.Opening += new System.ComponentModel.CancelEventHandler(this.mnuTransfers_Opening);
            // 
            // mnuTransferPause
            // 
            this.mnuTransferPause.Name = "mnuTransferPause";
            this.mnuTransferPause.Size = new System.Drawing.Size(166, 22);
            this.mnuTransferPause.Text = "Suspend/Resume";
            // 
            // mnuTransferCancel
            // 
            this.mnuTransferCancel.Name = "mnuTransferCancel";
            this.mnuTransferCancel.Size = new System.Drawing.Size(166, 22);
            this.mnuTransferCancel.Text = "Cancel";
            // 
            // frmFileExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 620);
            this.Controls.Add(this.lstTransfers);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.txtNav);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmFileExplorer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmFileExplorer";
            this.Load += new System.EventHandler(this.frmFileExplorer_Load);
            this.mnuExplore.ResumeLayout(false);
            this.mnuTransfers.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNav;
        private System.Windows.Forms.ImageList lstIcons;
        private System.Windows.Forms.ContextMenuStrip mnuExplore;
        private System.Windows.Forms.ToolStripMenuItem mnuRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuDirectory;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuDirectoryCreate;
        private System.Windows.Forms.ToolStripMenuItem mnuDirectoryMove;
        private System.Windows.Forms.ToolStripMenuItem mnuDirectoryDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuDirectoryCompress;
        private System.Windows.Forms.ToolStripMenuItem mnuFileMove;
        private System.Windows.Forms.ToolStripMenuItem mnuFileDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuFileCompress;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mnuDownload;
        private System.Windows.Forms.ToolStripMenuItem mnuUpload;
        private FanceyListView lstFiles;
        private FanceyListView lstTransfers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ContextMenuStrip mnuTransfers;
        private System.Windows.Forms.ToolStripMenuItem mnuTransferPause;
        private System.Windows.Forms.ToolStripMenuItem mnuTransferCancel;
    }
}