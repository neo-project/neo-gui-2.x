namespace Neo.UI
{
    partial class DownloadChainACC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadChainACC));
            this.comboMirrorLocation = new System.Windows.Forms.ComboBox();
            this.progressDownload = new System.Windows.Forms.ProgressBar();
            this.btnDownload = new System.Windows.Forms.Button();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboMirrorLocation
            // 
            this.comboMirrorLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboMirrorLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMirrorLocation.FormattingEnabled = true;
            this.comboMirrorLocation.Location = new System.Drawing.Point(18, 43);
            this.comboMirrorLocation.Name = "comboMirrorLocation";
            this.comboMirrorLocation.Size = new System.Drawing.Size(873, 30);
            this.comboMirrorLocation.TabIndex = 0;
            this.comboMirrorLocation.SelectedIndexChanged += new System.EventHandler(this.MirrorLocation_Changed);
            // 
            // progressDownload
            // 
            this.progressDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressDownload.Location = new System.Drawing.Point(18, 92);
            this.progressDownload.Name = "progressDownload";
            this.progressDownload.Size = new System.Drawing.Size(873, 25);
            this.progressDownload.TabIndex = 3;
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDownload.AutoSize = true;
            this.btnDownload.Location = new System.Drawing.Point(402, 187);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 32);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Tag = "download";
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.DownloadBootstrap_Click);
            // 
            // lblFileSize
            // 
            this.lblFileSize.Location = new System.Drawing.Point(18, 129);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFileSize.Size = new System.Drawing.Size(873, 30);
            this.lblFileSize.TabIndex = 8;
            this.lblFileSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DownloadChainACC
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(905, 231);
            this.Controls.Add(this.lblFileSize);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.progressDownload);
            this.Controls.Add(this.comboMirrorLocation);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DownloadChainACC";
            this.Text = "NEO Blockchain Bootstrap";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownloadChainACC_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboMirrorLocation;
        private System.Windows.Forms.ProgressBar progressDownload;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label lblFileSize;
    }
}