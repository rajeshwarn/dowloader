using System;
using System.IO;
using System.Net;
using System.Collections.Generic;

namespace Asharism.Downloader
{
    public delegate void DownloadProgressHandler(object sender, DownloadEventArgs e);

    /// <summary>
    /// Downloads and resumes files from HTTP, FTP, and File (file://) URLS
    /// </summary>
    public class FileDownloader
    {
        // Block size to download is by default 1K.
        private const int downloadBlockSize = 1024;

        // Determines whether the user has canceled or not.
        private bool canceled = false;

        private string downloadingTo;

        /// <summary>
        /// This is the name of the file we get back from the server when we
        /// try to download the provided url. It will only contain a non-null
        /// string when we've successfully contacted the server and it has started
        /// sending us a file.
        /// </summary>
        public string DownloadingTo
        {
            get { return this.downloadingTo; }
        }

        public void Cancel()
        {
            this.canceled = true;
        }

        /// <summary>
        /// Progress update
        /// </summary>
        public event DownloadProgressHandler ProgressChanged;

        private IWebProxy proxy = null;

        /// <summary>
        /// Proxy to be used for http and ftp requests.
        /// </summary>
        public IWebProxy Proxy
        {
            get { return this.proxy; }
            set { this.proxy = value; }
        }

        /// <summary>
        /// Fired when progress reaches 100%.
        /// </summary>
        public event EventHandler DownloadComplete;

        private void OnDownloadComplete()
        {
            if (this.DownloadComplete != null)
                this.DownloadComplete(this, new EventArgs());
        }

        /// <summary>
        /// Begin downloading the file at the specified url, and save it to the current folder.
        /// </summary>
        public void Download(string url)
        {
            this.Download(url, "");
        }
        /// <summary>
        /// Begin downloading the file at the specified url, and save it to the given folder.
        /// </summary>
        public void Download(string url, string destFolder)
        {
            DownloadData data = null;
            this.canceled = false;

            try
            {
                // get download details                
                data = DownloadData.Create(url, destFolder, this.proxy);
                // Find out the name of the file that the web server gave us.
                string destFileName = Path.GetFileName(data.Response.ResponseUri.ToString());


                // The place we're downloading to (not from) must not be a URI,
                // because Path and File don't handle them...
                destFolder = destFolder.Replace("file:///", "").Replace("file://", "");
                this.downloadingTo = Path.Combine(destFolder, destFileName);

                // Create the file on disk here, so even if we don't receive any data of the file
                // it's still on disk. This allows us to download 0-byte files.
                if (!File.Exists(this.downloadingTo))
                {
                    FileStream fs = File.Create(this.downloadingTo);
                    fs.Close();
                }

                // create the download buffer
                byte[] buffer = new byte[downloadBlockSize];

                int readCount;

                // update how many bytes have already been read
                long totalDownloaded = data.StartPoint;

                bool gotCanceled = false;

                while ((readCount = data.DownloadStream.Read(buffer, 0, downloadBlockSize)) > 0)
                {
                    // break on cancel
                    if (this.canceled)
                    {
                        gotCanceled = true;
                        data.Close();
                        break;
                    }

                    // update total bytes read
                    totalDownloaded += readCount;

                    // save block to end of file
                    SaveToFile(buffer, readCount, this.downloadingTo);

                    // send progress info
                    if (data.IsProgressKnown)
                        this.RaiseProgressChanged(totalDownloaded, data.FileSize);

                    // break on cancel
                    if (this.canceled)
                    {
                        gotCanceled = true;
                        data.Close();
                        break;
                    }
                }

                if (!gotCanceled)
                    this.OnDownloadComplete();
            }
            catch (UriFormatException e)
            {
                throw new ArgumentException(
                    String.Format("Could not parse the URL \"{0}\" - it's either malformed or is an unknown protocol.", url), e);
            }
            finally
            {
                if (data != null)
                    data.Close();
            }
        }

        /// <summary>
        /// Download a file from a list or URLs. If downloading from one of the URLs fails,
        /// another URL is tried.
        /// </summary>
        public void Download(List<string> urlList)
        {
            this.Download(urlList, "");
        }

        /// <summary>
        /// Download a file from a list or URLs. If downloading from one of the URLs fails,
        /// another URL is tried.
        /// </summary>
        public void Download(List<string> urlList, string destFolder)
        {
            // validate input
            if (urlList == null)
                throw new ArgumentException("Url list not specified.");

            if (urlList.Count == 0)
                throw new ArgumentException("Url list empty.");

            // try each url in the list.
            // if one succeeds, we are done.
            // if any fail, move to the next.
            Exception ex = null;
            foreach (string s in urlList)
            {
                ex = null;
                try
                {
                    this.Download(s, destFolder);
                }
                catch (Exception e)
                {
                    ex = e;
                }
                // If we got through that without an exception, we found a good url
                if (ex == null)
                    break;
            }
            if (ex != null)
                throw ex;
        }

        /// <summary>
        /// Asynchronously download a file from the url.
        /// </summary>
        public void AsyncDownload(string url)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(this.WaitCallbackMethod), new string[] { url, "" });
        }

        /// <summary>
        /// Asynchronously download a file from the url to the destination folder.
        /// </summary>
        public void AsyncDownload(string url, string destFolder)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(this.WaitCallbackMethod), new string[] { url, destFolder });
        }

        /// <summary>
        /// Asynchronously download a file from a list or URLs. If downloading from one of the URLs fails,
        /// another URL is tried.
        /// </summary>
        public void AsyncDownload(List<string> urlList, string destFolder)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(this.WaitCallbackMethod), new object[] { urlList, destFolder });
        }

        /// <summary>
        /// Asynchronously download a file from a list or URLs. If downloading from one of the URLs fails,
        /// another URL is tried.
        /// </summary>
        public void AsyncDownload(List<string> urlList)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(this.WaitCallbackMethod), new object[] { urlList, "" });
        }

        /// <summary>
        /// A WaitCallback used by the AsyncDownload methods.
        /// </summary>
        private void WaitCallbackMethod(object data)
        {
            // Can either be a string array of two strings (url and dest folder),
            // or an object array containing a list<string> and a dest folder
            if (data is string[])
            {
                String[] strings = data as String[];
                this.Download(strings[0], strings[1]);
            }
            else
            {
                Object[] list = data as Object[];
                List<String> urlList = list[0] as List<String>;
                String destFolder = list[1] as string;
                this.Download(urlList, destFolder);
            }
        }

        private static void SaveToFile(byte[] buffer, int count, string fileName)
        {
            FileStream f = null;

            try
            {
                f = File.Open(fileName, FileMode.Append, FileAccess.Write);
                f.Write(buffer, 0, count);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(
                    String.Format("Error trying to save file \"{0}\": {1}", fileName, e.Message), e);
            }
            finally
            {
                if (f != null)
                    f.Close();
            }
        }

        private void RaiseProgressChanged(long current, long target)
        {
            if (this.ProgressChanged != null)
                this.ProgressChanged(this, new DownloadEventArgs(target, current));
        }
    }
}