using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Downloader.Core
{
    public class Downloader : Component, IDisposable
    {
        #region Constants

        private static readonly int DEF_PACKET_LENGTH = 2048;

        #endregion

        #region Fields

        private bool _ProxyEnabled;
        private bool _ExitThread;
        private string _ProxyURL;
        private string _LoginUsername;
        private string _LoginPassword;
        private string _FileURL;
        private string _DownloadPath;
        private FileStream _SaveFileStream = null;
        private HttpWebResponse _Response = null;
        private HttpWebRequest _Request = null;
        private Thread _DownloadThread;

        #endregion

        #region Events

        public event EventHandler<DownloadErrorEventArgs> DownloadError;
        public event EventHandler<DownloadProgressEventArgs> DownloadProgressChanged;
        public event EventHandler<DownloadCompleteEventArgs> DownloadCompleted;

        #endregion

        #region Ctor


        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AbortDownload();
            }

            base.Dispose(disposing);
        }


        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region Props

        /// <summary>
        /// Checks if the update is already running
        /// </summary>
        [Browsable(false)]
        public bool IsRunning
        {
            get { return _DownloadThread != null && _DownloadThread.IsAlive; }
        }

        /// <summary>
        /// ProxyEnabled
        /// </summary>
        [DefaultValue(false)]
        public bool ProxyEnabled
        {
            get { return _ProxyEnabled; }
            set { _ProxyEnabled = value; }
        }

        /// <summary>
        /// ProxyURL
        /// </summary>
        [DefaultValue("")]
        public string ProxyURL
        {
            get { return _ProxyURL; }
            set { _ProxyURL = value; }
        }

        /// <summary>
        /// Proxy Login Username
        /// </summary>
        [DefaultValue("")]
        public string LoginUsername
        {
            get { return _LoginUsername; }
            set { _LoginUsername = value; }
        }

        /// <summary>
        /// Proxy Login Password
        /// </summary>
        [DefaultValue("")]
        public string LoginPassword
        {
            get { return _LoginPassword; }
            set { _LoginPassword = value; }
        }

        /// <summary>
        /// URL of the file to download
        /// </summary>
        [DefaultValue("")]
        public string FileURL
        {
            get { return _FileURL; }
            set { _FileURL = value; }
        }

        /// <summary>
        /// Download Directory where the file is being downloaded is written to.
        /// </summary>
        public string DownloadPath
        {
            get { return _DownloadPath; }
            set { _DownloadPath = value; }
        }

        #endregion

        #region Methods

        #region Protected

        /// <summary>
        /// Fires the DownloadProgressChanged event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnProgressChanged(DownloadProgressEventArgs e)
        {
            if (DownloadProgressChanged != null)
                DownloadProgressChanged(this, e);
        }

        /// <summary>
        /// Fire the DownloadCompleted event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDownloadCompleted(DownloadCompleteEventArgs e)
        {
            if (DownloadCompleted != null)
                DownloadCompleted(this, e);
        }

        /// <summary>
        /// Fires DownloadError event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDownloadError(DownloadErrorEventArgs e)
        {
            if (DownloadError != null)
                DownloadError(this, e);
        }

        #endregion

        #region Public

        /// <summary>
        /// Invoke this method when you want to download the file
        /// </summary>
        public void TryDownload()
        {
            _ExitThread = false;
            if (IsRunning)
            {
                OnDownloadError(new DownloadErrorEventArgs(null, "AutoUpdate is already in progress."));
                return;
            }

            _DownloadThread = new Thread(new ThreadStart(DownloadOperation));
            _DownloadThread.IsBackground = true;
            _DownloadThread.Start();
        }

        /// <summary>
        /// Abort any running download
        /// </summary>
        public void AbortDownload()
        {
            _ExitThread = true;
        }

        #endregion

        #region Private

        /// <summary>
        /// Download a file from the specified url and copy it to the specified path
        /// </summary>
        private void DownloadFile(string url, string path, long startPoint)
        {
            int startingPoint = Convert.ToInt32(startPoint);

            try
            {
                //For using untrusted SSL Certificates
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(OnCheckRemoteCallback);

                _Request = (HttpWebRequest)HttpWebRequest.Create(url);
                _Request.AddRange(startingPoint);

                if (!string.IsNullOrEmpty(LoginUsername))
                {
                    _Request.Credentials = new NetworkCredential(LoginUsername, LoginPassword);
                }
                else
                {
                    _Request.Credentials = CredentialCache.DefaultCredentials;
                }

                if (ProxyEnabled)
                {
                    _Request.Proxy = new WebProxy(ProxyURL);
                }

                _Response = (HttpWebResponse)_Request.GetResponse();
                Stream responseSteam = _Response.GetResponseStream();

                if (startingPoint == 0)
                {
                    _SaveFileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                }
                else
                {
                    _SaveFileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                }

                int bytesSize;
                long fileSize = _Response.ContentLength;
                byte[] downloadBuffer = new byte[DEF_PACKET_LENGTH];

                OnProgressChanged(new DownloadProgressEventArgs(fileSize, 0, "Starting the download..."));

                while ((bytesSize = responseSteam.Read(downloadBuffer, 0, downloadBuffer.Length)) > 0 && !_ExitThread)
                {
                    if (_ExitThread)
                    {
                        break;
                    }

                    _SaveFileStream.Write(downloadBuffer, 0, bytesSize);
                    OnProgressChanged(new DownloadProgressEventArgs(_SaveFileStream.Length, fileSize + startingPoint, "Download in progress..."));
                }

                if (_ExitThread)
                {
                    _SaveFileStream.Close();
                    _SaveFileStream.Dispose();
                    OnDownloadCompleted(new DownloadCompleteEventArgs(false, true));

                    return;
                }

                OnDownloadCompleted(new DownloadCompleteEventArgs(true, false));
            }
            catch (Exception ex)
            {
                OnDownloadError(new DownloadErrorEventArgs(ex, string.Format("Problem downloading and copying file from {0} to {1}.", url, path)));
                OnDownloadCompleted(new DownloadCompleteEventArgs(false, false));
            }
            finally
            {
                if (_SaveFileStream != null)
                {
                    _SaveFileStream.Close();
                    _SaveFileStream.Dispose();
                }

                if (_Response != null)
                {
                    _Response.Close();
                }
            }
        }

        /// <summary>
        /// Using untrusted SSL certificates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private bool OnCheckRemoteCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void DownloadOperation()
        {
            long startingPoint = 0;

            if(File.Exists(DownloadPath))
            {
                startingPoint = new FileInfo(DownloadPath).Length;
            }

            DownloadFile(FileURL, DownloadPath, startingPoint);
        }

        #endregion

        #endregion
    }
}
