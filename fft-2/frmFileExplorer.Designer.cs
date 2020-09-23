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
            this.lstFiles = new System.Windows.Forms.ListView();
            this.lstIcons = new System.Windows.Forms.ImageList(this.components);
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
            this.lstFiles.FullRowSelect = true;
            this.lstFiles.HideSelection = false;
            this.lstFiles.LargeImageList = this.lstIcons;
            this.lstFiles.Location = new System.Drawing.Point(12, 41);
            this.lstFiles.MultiSelect = false;
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(822, 567);
            this.lstFiles.SmallImageList = this.lstIcons;
            this.lstFiles.TabIndex = 2;
            this.lstFiles.UseCompatibleStateImageBehavior = false;
            this.lstFiles.View = System.Windows.Forms.View.Details;
            // 
            // lstIcons
            // 
            this.lstIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.lstIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("lstIcons.ImageStream")));
            this.lstIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.lstIcons.Images.SetKeyName(0, "drive.png");
            // 
            // frmFileExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 620);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.txtNav);
            this.Controls.Add(this.label1);
            this.Name = "frmFileExplorer";
            this.Text = "frmFileExplorer";
            this.Load += new System.EventHandler(this.frmFileExplorer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNav;
        private System.Windows.Forms.ListView lstFiles;
        private System.Windows.Forms.ImageList lstIcons;
    }
}