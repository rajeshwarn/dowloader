using System;
using System.IO;
using System.Windows.Forms;
using Downloader.Core;

namespace Downloader.Test
{
    public partial class DownloadForm : Form
    {
        #region Fields

        private bool _IsPaused = false;

        #endregion

        #region Delegates

        private delegate void ParamMethodInvoker(long fileSize, long progressValue, string message);
        private delegate void BooleanMethodInvoker(bool value);
        private delegate void UpdateStatusInvoker(bool successfull, bool aborted);

        #endregion

        #region Ctor

        public DownloadForm()
        {
            InitializeComponent();
        }

        #endregion

        #region EventHandlers

        private void downloader_DownloadCompleted(object sender, DownloadCompleteEventArgs e)
        {
            DownloadFinished(e.IsSuccessfull, e.IsAborted);
        }

        private void downloader_DownloadError(object sender, DownloadErrorEventArgs e)
        {
            MessageBox.Show(string.Format("{0}{1}{2}", e.Message, Environment.NewLine, e.Exception));
        }

        private void downloader_DownloadProgressChanged(object sender, DownloadProgressEventArgs e)
        {
            Invoke(new ParamMethodInvoker(UpdateProgress), new object[] { e.BytesRead, e.TotalBytes, e.Message });
        }

        private void cbUserProxy_CheckedChanged(object sender, EventArgs e)
        {
            grpProxy.Enabled = cbUserProxy.Checked;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _IsPaused = false;
            if(!HasErrors() && !downloader.IsRunning)
            {
                downloader.FileURL = txtURL.Text;
                downloader.DownloadPath = txtSaveTo.Text;
                
                if(cbUserProxy.Checked)
                {
                    downloader.ProxyEnabled = true;
                    downloader.ProxyURL = txtProxyAddress.Text;
                    downloader.LoginUsername = txtUsername.Text;
                    downloader.LoginPassword = txtPassword.Text;
                }

                downloader.TryDownload();
                UpdateButtons(downloader.IsRunning);
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            _IsPaused = true;
            if(downloader.IsRunning)
            {
                downloader.AbortDownload();
            }
            else
            {
                downloader.TryDownload();
            }

            UpdateButtons(downloader.IsRunning);
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            _IsPaused = false;
            if (downloader.IsRunning)
            {
                downloader.AbortDownload();
            }
        }

        #endregion

        #region Methods

        #region Private

        private void DownloadFinished(bool successfull, bool aborted)
        {
            Invoke(new MethodInvoker(ClearProgress));
            Invoke(new BooleanMethodInvoker(UpdateButtons), new object[] { false });
            Invoke(new UpdateStatusInvoker(UpdateStatus), new object[] { successfull, aborted });
        }

        private void UpdateStatus(bool successfull, bool aborted)
        {
            string message = string.Empty;

            if (successfull)
            {
                message = "Download finished successfully.";
            }
            else
            {
                if (!aborted)
                {
                    message = "Download is finished with errors.";
                }
                else
                {
                    if (_IsPaused)
                    {
                        message = "Download is paused.";
                    }
                    else
                    {
                        Application.DoEvents();
                        if (File.Exists(downloader.DownloadPath))
                            File.Delete(downloader.DownloadPath);

                        message = "Download is aborted.";
                        lblDownloadMessage.Text = string.Empty;
                        UpdateButtons(downloader.IsRunning);
                        ClearProgress();
                    }
                }
            }

            lblStatusMessage.Text = message;
        }

        private void ClearProgress()
        {
            progress.Value = 0;
        }

        private void UpdateProgress(long bytesRead, long totalBytes, string message)
        {
            if (bytesRead > 0 && totalBytes > 0)
            {
                int kbRead = Convert.ToInt32(bytesRead) / 1024;
                int kbTotal = Convert.ToInt32(totalBytes) / 1024;
                int percent = Convert.ToInt32((bytesRead * 100) / totalBytes);

                progress.Value = percent;
                lblDownloadMessage.Text = string.Format("{0:#,###} of {1:#,###} KBytes ({2}%)", kbRead, kbTotal, percent);
            }
            else
            {
                progress.Value = 0;
            }

            lblStatusMessage.Text = message;
        }

        private void UpdateButtons(bool isRunning)
        {
            btnPause.Enabled = isRunning;
            btnStart.Enabled = !isRunning;
        }

        private bool HasErrors()
        {
            error.Clear();
            bool hasErrors = false;
            
            if (string.IsNullOrEmpty(txtURL.Text))
            {
                error.SetError(txtURL, "Please enter a URL address to download from.");
                hasErrors = true;
            }

            if (string.IsNullOrEmpty(txtSaveTo.Text))
            {
                error.SetError(txtSaveTo, "Please specify a path to save the downloaded file.");
                hasErrors = true;
            }

            if (cbUserProxy.Checked && string.IsNullOrEmpty(txtProxyAddress.Text))
            {
                error.SetError(txtProxyAddress, "Please enter the address of your proxy server e.g. http://myproxy:8080/");
                hasErrors = true;
            }

            return hasErrors;
        }

        #endregion

        #endregion
    }
}