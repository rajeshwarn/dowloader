using System;

namespace Asharism.Downloader
{
    /// <summary>
    /// Progress of a downloading file.
    /// </summary>
    public class DownloadEventArgs : EventArgs
    {
        private readonly int percentDone;
        private readonly string downloadState;

        private long totalFileSize;

        /// <summary>
        /// Gets or sets the total size of the file.
        /// </summary>
        /// <value>The total size of the file.</value>
        public long TotalFileSize
        {
            get { return this.totalFileSize; }
            set { this.totalFileSize = value; }
        }

        private long currentFileSize;

        /// <summary>
        /// Gets or sets the size of the current file.
        /// </summary>
        /// <value>The size of the current file.</value>
        public long CurrentFileSize
        {
            get { return this.currentFileSize; }
            set { this.currentFileSize = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadEventArgs"/> class.
        /// </summary>
        /// <param name="totalFileSize">Total size of the file.</param>
        /// <param name="currentFileSize">Size of the current file.</param>
        public DownloadEventArgs(long totalFileSize, long currentFileSize)
        {
            this.totalFileSize = totalFileSize;
            this.currentFileSize = currentFileSize;

            this.percentDone = (int)((((double)currentFileSize) / totalFileSize) * 100);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadEventArgs"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        public DownloadEventArgs(string state)
        {
            this.downloadState = state;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadEventArgs"/> class.
        /// </summary>
        /// <param name="percentDone">The percent done.</param>
        /// <param name="state">The state.</param>
        public DownloadEventArgs(int percentDone, string state)
        {
            this.percentDone = percentDone;
            this.downloadState = state;
        }

        /// <summary>
        /// Gets the percent done.
        /// </summary>
        /// <value>The percent done.</value>
        public int PercentDone
        {
            get
            {
                return this.percentDone;
            }
        }

        /// <summary>
        /// Gets the state of the download.
        /// </summary>
        /// <value>The state of the download.</value>
        public string DownloadState
        {
            get
            {
                return this.downloadState;
            }
        }
    }
}
