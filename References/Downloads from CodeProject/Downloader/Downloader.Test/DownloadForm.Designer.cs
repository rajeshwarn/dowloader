namespace Downloader.Test
{
    partial class DownloadForm
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
            this.downloader = new Downloader.Core.Downloader();
            this.txtSaveTo = new System.Windows.Forms.TextBox();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.lblDownloadPath = new System.Windows.Forms.Label();
            this.lblURL = new System.Windows.Forms.Label();
            this.grpDownloadInfo = new System.Windows.Forms.GroupBox();
            this.lblProxy = new System.Windows.Forms.Label();
            this.cbUserProxy = new System.Windows.Forms.CheckBox();
            this.grpProxy = new System.Windows.Forms.GroupBox();
            this.txtProxyAddress = new System.Windows.Forms.TextBox();
            this.lblProxyAddress = new System.Windows.Forms.Label();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.error = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblDownload = new System.Windows.Forms.Label();
            this.lblStatusMessage = new System.Windows.Forms.Label();
            this.lblDownloadMessage = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.grpDownloadInfo.SuspendLayout();
            this.grpProxy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.error)).BeginInit();
            this.SuspendLayout();
            // 
            // downloader
            // 
            this.downloader.DownloadPath = null;
            this.downloader.FileURL = null;
            this.downloader.LoginPassword = null;
            this.downloader.LoginUsername = null;
            this.downloader.ProxyURL = null;
            this.downloader.DownloadProgressChanged += new System.EventHandler<Downloader.Core.DownloadProgressEventArgs>(this.downloader_DownloadProgressChanged);
            this.downloader.DownloadError += new System.EventHandler<Downloader.Core.DownloadErrorEventArgs>(this.downloader_DownloadError);
            this.downloader.DownloadCompleted += new System.EventHandler<Downloader.Core.DownloadCompleteEventArgs>(this.downloader_DownloadCompleted);
            // 
            // txtSaveTo
            // 
            this.txtSaveTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtSaveTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.txtSaveTo.Location = new System.Drawing.Point(109, 48);
            this.txtSaveTo.Name = "txtSaveTo";
            this.txtSaveTo.Size = new System.Drawing.Size(258, 20);
            this.txtSaveTo.TabIndex = 3;
            // 
            // txtURL
            // 
            this.txtURL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtURL.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.txtURL.Location = new System.Drawing.Point(109, 25);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(258, 20);
            this.txtURL.TabIndex = 1;
            // 
            // lblDownloadPath
            // 
            this.lblDownloadPath.Location = new System.Drawing.Point(6, 47);
            this.lblDownloadPath.Name = "lblDownloadPath";
            this.lblDownloadPath.Size = new System.Drawing.Size(97, 20);
            this.lblDownloadPath.TabIndex = 2;
            this.lblDownloadPath.Text = "Save To : ";
            this.lblDownloadPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblURL
            // 
            this.lblURL.Location = new System.Drawing.Point(6, 24);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(97, 20);
            this.lblURL.TabIndex = 0;
            this.lblURL.Text = "File URL : ";
            this.lblURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpDownloadInfo
            // 
            this.grpDownloadInfo.Controls.Add(this.txtPassword);
            this.grpDownloadInfo.Controls.Add(this.txtUsername);
            this.grpDownloadInfo.Controls.Add(this.label2);
            this.grpDownloadInfo.Controls.Add(this.lblUsername);
            this.grpDownloadInfo.Controls.Add(this.lblProxy);
            this.grpDownloadInfo.Controls.Add(this.cbUserProxy);
            this.grpDownloadInfo.Controls.Add(this.lblURL);
            this.grpDownloadInfo.Controls.Add(this.txtSaveTo);
            this.grpDownloadInfo.Controls.Add(this.lblDownloadPath);
            this.grpDownloadInfo.Controls.Add(this.txtURL);
            this.grpDownloadInfo.Location = new System.Drawing.Point(12, 12);
            this.grpDownloadInfo.Name = "grpDownloadInfo";
            this.grpDownloadInfo.Size = new System.Drawing.Size(387, 154);
            this.grpDownloadInfo.TabIndex = 0;
            this.grpDownloadInfo.TabStop = false;
            this.grpDownloadInfo.Text = "Download Info";
            // 
            // lblProxy
            // 
            this.lblProxy.Location = new System.Drawing.Point(6, 116);
            this.lblProxy.Name = "lblProxy";
            this.lblProxy.Size = new System.Drawing.Size(97, 20);
            this.lblProxy.TabIndex = 4;
            this.lblProxy.Text = "Use Proxy : ";
            this.lblProxy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbUserProxy
            // 
            this.cbUserProxy.AutoSize = true;
            this.cbUserProxy.Location = new System.Drawing.Point(109, 120);
            this.cbUserProxy.Name = "cbUserProxy";
            this.cbUserProxy.Size = new System.Drawing.Size(15, 14);
            this.cbUserProxy.TabIndex = 5;
            this.cbUserProxy.UseVisualStyleBackColor = true;
            this.cbUserProxy.CheckedChanged += new System.EventHandler(this.cbUserProxy_CheckedChanged);
            // 
            // grpProxy
            // 
            this.grpProxy.Controls.Add(this.txtProxyAddress);
            this.grpProxy.Controls.Add(this.lblProxyAddress);
            this.grpProxy.Enabled = false;
            this.grpProxy.Location = new System.Drawing.Point(12, 169);
            this.grpProxy.Name = "grpProxy";
            this.grpProxy.Size = new System.Drawing.Size(387, 49);
            this.grpProxy.TabIndex = 1;
            this.grpProxy.TabStop = false;
            this.grpProxy.Text = "Proxy";
            // 
            // txtProxyAddress
            // 
            this.txtProxyAddress.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtProxyAddress.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.txtProxyAddress.Location = new System.Drawing.Point(109, 17);
            this.txtProxyAddress.Name = "txtProxyAddress";
            this.txtProxyAddress.Size = new System.Drawing.Size(258, 20);
            this.txtProxyAddress.TabIndex = 1;
            // 
            // lblProxyAddress
            // 
            this.lblProxyAddress.Location = new System.Drawing.Point(6, 17);
            this.lblProxyAddress.Name = "lblProxyAddress";
            this.lblProxyAddress.Size = new System.Drawing.Size(97, 20);
            this.lblProxyAddress.TabIndex = 0;
            this.lblProxyAddress.Text = "Proxy Address : ";
            this.lblProxyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(162, 298);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(87, 23);
            this.btnPause.TabIndex = 8;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(255, 298);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(87, 23);
            this.btnAbort.TabIndex = 9;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(69, 298);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(87, 23);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(12, 227);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(97, 20);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Status : ";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(12, 278);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(387, 12);
            this.progress.TabIndex = 6;
            // 
            // error
            // 
            this.error.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.error.ContainerControl = this;
            // 
            // lblDownload
            // 
            this.lblDownload.Location = new System.Drawing.Point(12, 252);
            this.lblDownload.Name = "lblDownload";
            this.lblDownload.Size = new System.Drawing.Size(97, 20);
            this.lblDownload.TabIndex = 4;
            this.lblDownload.Text = "Downloaded : ";
            this.lblDownload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.Location = new System.Drawing.Point(115, 227);
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(284, 20);
            this.lblStatusMessage.TabIndex = 3;
            this.lblStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDownloadMessage
            // 
            this.lblDownloadMessage.Location = new System.Drawing.Point(115, 252);
            this.lblDownloadMessage.Name = "lblDownloadMessage";
            this.lblDownloadMessage.Size = new System.Drawing.Size(284, 20);
            this.lblDownloadMessage.TabIndex = 5;
            this.lblDownloadMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(109, 94);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(140, 20);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(109, 71);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(140, 20);
            this.txtUsername.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Password : ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point(6, 70);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(97, 20);
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "Username : ";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 330);
            this.Controls.Add(this.lblDownloadMessage);
            this.Controls.Add(this.lblStatusMessage);
            this.Controls.Add(this.lblDownload);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.grpProxy);
            this.Controls.Add(this.grpDownloadInfo);
            this.Name = "DownloadForm";
            this.Text = "Downloader";
            this.grpDownloadInfo.ResumeLayout(false);
            this.grpDownloadInfo.PerformLayout();
            this.grpProxy.ResumeLayout(false);
            this.grpProxy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.error)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Downloader.Core.Downloader downloader;
        private System.Windows.Forms.TextBox txtSaveTo;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Label lblDownloadPath;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.GroupBox grpDownloadInfo;
        private System.Windows.Forms.GroupBox grpProxy;
        private System.Windows.Forms.Label lblProxy;
        private System.Windows.Forms.CheckBox cbUserProxy;
        private System.Windows.Forms.Label lblProxyAddress;
        private System.Windows.Forms.TextBox txtProxyAddress;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.ErrorProvider error;
        private System.Windows.Forms.Label lblDownload;
        private System.Windows.Forms.Label lblDownloadMessage;
        private System.Windows.Forms.Label lblStatusMessage;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblUsername;
    }
}

