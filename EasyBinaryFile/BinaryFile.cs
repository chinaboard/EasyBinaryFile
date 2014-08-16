using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EasyBinaryFile.Utility;
using EasyBinaryFile.Writer;
using EasyBinaryFile.Reader;

namespace EasyBinaryFile
{
    public class BinaryFile : IDisposable
    {

        #region 字段
        private BufferedStream _bufferStream = null;
        private BinaryWriter _binaryWriter = null;
        private BinaryReader _binaryReader = null;
        private FileStream _fileStream = null;
        private int _bufferSize = 0;
        #endregion

        #region 属性
        public bool IsDisposed { get; private set; }
        public bool EnableSmartGzip { get; private set; }
        #endregion

        #region 构造
        public BinaryFile(FileStream fileStream, bool enableSmartGzip = true, int bufferSize = 4096)
        {
            Preconditions.CheckNotNull(fileStream, "fileStream");
            if (bufferSize < 4096)
                bufferSize = 4096;

            this._fileStream = fileStream;
            this._bufferSize = bufferSize;
            this._bufferStream = new BufferedStream(this._fileStream, this._bufferSize);
            this.EnableSmartGzip = enableSmartGzip;
        }

        public BinaryFile(string path, FileShare share = FileShare.None, FileMode mode = FileMode.Open, FileAccess access = FileAccess.ReadWrite, bool enableSmartGzip = true, int bufferSize = 4096)
        {
            Preconditions.CheckNotBlank(path, "path");
            if (!File.Exists(path))
                mode = FileMode.OpenOrCreate;
            if (bufferSize < 4096)
                bufferSize = 4096;

            this._fileStream = File.Open(path, mode, access, share);
            this._bufferSize = bufferSize;
            this._bufferStream = new BufferedStream(this._fileStream, this._bufferSize);
            this.EnableSmartGzip = enableSmartGzip;
        }
        #endregion

        #region 方法
        public BinaryFileRead GetReader()
        {
            Preconditions.CheckDisposed(this.IsDisposed, this.GetType().Name);
            return new BinaryFileRead(this._bufferStream);
        }
        public BinaryFileWrite GetWriter()
        {
            Preconditions.CheckDisposed(this.IsDisposed, this.GetType().Name);
            return new BinaryFileWrite(this._bufferStream);
        }
        #endregion


        #region Dispose
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            this.IsDisposed = true;
        }
        ~BinaryFile()
        {
            this.Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._binaryReader != null)
                {
                    this._binaryReader.Close();
                    this._binaryReader.Dispose();
                    this._binaryReader = null;
                }
                if (this._binaryWriter != null)
                {
                    this._binaryWriter.Close();
                    this._binaryWriter.Dispose();
                    this._binaryWriter = null;
                }
                if (this._bufferStream != null)
                {
                    this._bufferStream.Close();
                    this._bufferStream.Dispose();
                    this._bufferStream = null;
                }
                if (this._fileStream != null)
                {
                    this._fileStream.Close();
                    this._fileStream.Dispose();
                    this._fileStream = null;
                }
            }
        }
        #endregion

    }
}
