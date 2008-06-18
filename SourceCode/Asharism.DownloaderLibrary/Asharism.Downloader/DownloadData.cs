using System;
using System.IO;
using System.Net;

namespace Asharism.Downloader
{
    /// <summary>
    /// Constains the connection to the file server and other statistics about a file
    /// that's downloading.
    /// </summary>
    class DownloadData : IDisposable
    {
        #region Member decleration

        protected internal readonly int BadFileSize = -1;

        private readonly string url;

        private bool objectDisposed;
        private long size;
        private long start;

        private IWebProxy proxy = null;
        private WebResponse response;
        private Stream stream;

        #endregion

        #region Create methods

        public static DownloadData Create(string url, string destFolder)
        {
            return Create(url, destFolder, null);
        }

        public static DownloadData Create(string url, string destFolder, IWebProxy proxy)
        {

            // This is what we will return
            DownloadData downloadData = new DownloadData(url);
            downloadData.proxy = proxy;

            long urlSize = downloadData.GetFileSize(url);
            downloadData.size = urlSize;

            WebRequest req = downloadData.GetRequest(url);

            try
            {
                downloadData.response = req.GetResponse();
            }
            catch (Exception e)
            {
                throw new ArgumentException(String.Format(
                                                "Error downloading \"{0}\": {1}", url, e.Message), e);
            }

            // Check to make sure the response isn't an error. If it is this method
            // will throw exceptions.
            ValidateResponse(downloadData.response, url);

            // Take the name of the file given to use from the web server.
            String fileName = Path.GetFileName(downloadData.response.ResponseUri.ToString());

            String downloadTo = Path.Combine(destFolder, fileName);

            // If we don't know how big the file is supposed to be,
            // we can't resume, so delete what we already have if something is on disk already.
            if (!downloadData.IsProgressKnown && File.Exists(downloadTo))
                File.Delete(downloadTo);

            if (downloadData.IsProgressKnown && File.Exists(downloadTo))
            {
                // We only support resuming on http requests
                if (!(downloadData.Response is HttpWebResponse))
                {
                    File.Delete(downloadTo);
                }
                else
                {
                    // Try and start where the file on disk left off
                    downloadData.start = new FileInfo(downloadTo).Length;

                    // If we have a file that's bigger than what is online, then something 
                    // strange happened. Delete it and start again.
                    if (downloadData.start > urlSize)
                        File.Delete(downloadTo);
                    else if (downloadData.start < urlSize)
                    {
                        // Try and resume by creating a new request with a new start position
                        downloadData.response.Close();
                        req = downloadData.GetRequest(url);
                        ((HttpWebRequest)req).AddRange((int)downloadData.start);
                        downloadData.response = req.GetResponse();

                        if (((HttpWebResponse)downloadData.Response).StatusCode != HttpStatusCode.PartialContent)
                        {
                            // They didn't support our resume request. 
                            File.Delete(downloadTo);
                            downloadData.start = 0;
                        }
                    }
                }
            }
            return downloadData;
        }

        #endregion

        #region Private functions

        // Used by the factory method
        private DownloadData(string url)
        {
            this.url = url;
        }

        /// <summary>
        /// Checks whether a WebResponse is an error.
        /// </summary>
        /// <param name="response"></param>
        private static void ValidateResponse(WebResponse response, string url)
        {
            if (response is HttpWebResponse)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                // If it's an HTML page, it's probably an error page. Comment this
                // out to enable downloading of HTML pages.
                if (httpResponse.ContentType.Contains("text/html") || httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ArgumentException(
                        String.Format("Could not download \"{0}\" - a web page was returned from the web server.",
                                      url));
                }
            }
            else if (response is FtpWebResponse)
            {
                FtpWebResponse ftpResponse = (FtpWebResponse)response;
                if (ftpResponse.StatusCode == FtpStatusCode.ConnectionClosed)
                    throw new ArgumentException(
                        String.Format("Could not download \"{0}\" - FTP server closed the connection.", url));
            }
            // FileWebResponse doesn't have a status code to check.
        }

        /// <summary>
        /// Checks the file size of a remote file. If size is -1, then the file size
        /// could not be determined.
        /// </summary>
        /// <param name="url">Url to get the size of.</param>
        /// <returns>File size or <see cref="BadFileSize"/> if there is an exception.</returns>
        private long GetFileSize(string url)
        {
            try
            {
                using(WebResponse responseForFileSize = this.GetRequest(url).GetResponse())
                {
                    return responseForFileSize.ContentLength;
                }
            }
            catch
            {
                // If there is an exception
                return this.BadFileSize;
            }
        }

        private WebRequest GetRequest(string url)
        {
            WebRequest request = WebRequest.Create(url);

            if (request is HttpWebRequest)
            {
                request.Credentials = CredentialCache.DefaultCredentials;
            }

            if (this.proxy != null)
            {
                request.Proxy = this.proxy;
            }

            return request;
        }

        #endregion

        #region Properties

        public WebResponse Response
        {
            get
            {
                CheckObjectDisposed();

                return this.response;
            }
            set
            {
                CheckObjectDisposed();

                this.response = value;
            }
        }

        public Stream DownloadStream
        {
            get
            {
                CheckObjectDisposed();

                if (this.start == this.size)
                    return Stream.Null;
                if (this.stream == null)
                    this.stream = this.response.GetResponseStream();
                return this.stream;
            }
        }

        public long FileSize
        {
            get
            {
                CheckObjectDisposed();

                return this.size;
            }
        }

        public long StartPoint
        {
            get
            {
                CheckObjectDisposed();

                return this.start;
            }
        }

        public bool IsProgressKnown
        {
            get
            {
                CheckObjectDisposed();

                // If the size of the remote url is -1, that means we
                // couldn't determine it, and so we don't know
                // progress information.
                return this.size > this.BadFileSize;
            }
        }

        #endregion

        #region ToString override

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="DownloadData"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="DownloadData"/>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0} object representing {1}", base.ToString(), this.url);
        }

        #endregion

        #region Close and Dispose implementation

        /// <summary>
        /// Checks the object disposed and throws <see cref="ObjectDisposedException"/> exception if the object is disposed.
        /// </summary>
        public void CheckObjectDisposed()
        {
            if(this.objectDisposed)
            {
                throw new ObjectDisposedException(this.ToString());
            }
        }

        private void CloseWebResponse()
        {
            if (null != this.response)
            {
                this.response.Close();
            }

            if(null != this.stream)
            {
                this.stream.Close();
            }

            this.objectDisposed = true;
        }

        /// <summary>
        /// Closes the web response object held by this instance.
        /// </summary>
        public void Close()
        {
            CloseWebResponse();
        }

        /// <summary>
        /// Closes the web response object held by this instance.
        /// </summary>
        public void Dispose()
        {
            CloseWebResponse();
        }

        #endregion
    }
}
