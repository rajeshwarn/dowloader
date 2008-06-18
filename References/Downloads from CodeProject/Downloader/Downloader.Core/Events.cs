using System;

namespace Downloader.Core
{
    #region DownloadErrorEventArgs

    /// <summary>
    /// AutoUpdateErrorEventArgs
    /// </summary>
    public class DownloadErrorEventArgs : EventArgs
    {
        #region Fields

        private Exception ex;
        private string message;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        public DownloadErrorEventArgs(Exception ex, string message)
        {
            this.ex = ex;
            this.message = message;
        }

        #endregion

        #region Props

        /// <summary>
        /// Exception
        /// </summary>
        public Exception Exception
        {
            get { return ex; }
        }

        /// <summary>
        /// Message
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        #endregion
    }

    #endregion

    #region DownloadProgressEventArgs

    /// <summary>
    /// AutoUpdateProgressEventArgs
    /// </summary>
    public class DownloadProgressEventArgs : EventArgs
    {
        #region Fields

        private long _totalBytes;
        private long _bytesRead;
        private string _message;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="totalBytes"></param>
        /// <param name="message"></param>
        /// <param name="bytesRead"></param>
        public DownloadProgressEventArgs(long bytesRead, long totalBytes, string message)
        {
            _totalBytes = totalBytes;
            _message = message;
            _bytesRead = bytesRead;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bytesRead"></param>
        /// <param name="totalBytes"></param>
        public DownloadProgressEventArgs(long bytesRead, long totalBytes)
            : this(bytesRead, totalBytes, string.Empty)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public DownloadProgressEventArgs()
            : this(0, 0)
        {
        }

        #endregion

        #region Props

        /// <summary>
        /// FileLength
        /// </summary>
        public long BytesRead
        {
            get { return _bytesRead; }
        }

        /// <summary>
        /// Current Progress
        /// </summary>
        public long TotalBytes
        {
            get { return _totalBytes; }
        }

        /// <summary>
        /// Message
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        #endregion
    }

    #endregion

    #region DownloadCompleteEventArgs

    /// <summary>
    /// AutoUpdateCompleteEventArgs
    /// </summary>
    public class DownloadCompleteEventArgs : EventArgs
    {
        #region Fields

        private bool _isSuccessfull;
        private bool _isAborted;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="isSuccessfull"></param>
        /// <param name="isAborted"></param>
        public DownloadCompleteEventArgs(bool isSuccessfull, bool isAborted)
        {
            _isSuccessfull = isSuccessfull;
            _isAborted = isAborted;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="isSuccessfull"></param>
        public DownloadCompleteEventArgs(bool isSuccessfull)
        {
            _isSuccessfull = isSuccessfull;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public DownloadCompleteEventArgs() : this(true, false)
        {
        }

        #endregion

        #region Props

        /// <summary>
        /// Is operation finished successfully
        /// </summary>
        public bool IsSuccessfull
        {
            get { return _isSuccessfull; }
        }

        /// <summary>
        /// Is download aborted
        /// </summary>
        public bool IsAborted
        {
            get { return _isAborted; }
        }

        #endregion
    }

    #endregion
}