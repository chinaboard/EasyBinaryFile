using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.Reader
{
    public class BinaryFileRead : IBinaryFileRead, IDisposable
    {

        #region 字段
        private BufferedStream _bufferStream = null;
        private BinaryReader _binaryReader = null;
        private FileStream _fileStream = null;
        private int _bufferSize = 4096;
        private SmartGzip gzip = new SmartGzip();
        #endregion

        #region 属性
        public bool IsDisposed { get; private set; }
        public bool EnableSmartGzip { get; private set; }
        #endregion

        #region 构造
        public BinaryFileRead(BufferedStream bufferStream, bool enableSmartGzip = true)
        {
            Preconditions.CheckNotNull(bufferStream, "bufferStream");

            this.EnableSmartGzip = enableSmartGzip;

            this._bufferStream = bufferStream;
        }

        public BinaryFileRead(FileStream fileStream, bool enableSmartGzip = true, int bufferSize = 4096)
        {
            Preconditions.CheckNotNull(fileStream, "fileStream");
            if (bufferSize < 4096)
                bufferSize = 4096;

            this.EnableSmartGzip = enableSmartGzip;

            this._fileStream = fileStream;
            this._bufferSize = bufferSize;
            this._bufferStream = new BufferedStream(this._fileStream, this._bufferSize);
        }

        public BinaryFileRead(string path, FileShare share = FileShare.None, FileMode mode = FileMode.Open, FileAccess access = FileAccess.ReadWrite, bool enableSmartGzip = true, int bufferSize = 4096)
        {
            Preconditions.CheckNotBlank(path, "path");
            if (!File.Exists(path))
                mode = FileMode.OpenOrCreate;
            if (bufferSize < 4096)
                bufferSize = 4096;

            this.EnableSmartGzip = enableSmartGzip;

            this._fileStream = File.Open(path, mode, access, share);
            this._bufferSize = bufferSize;
            this._bufferStream = new BufferedStream(this._fileStream, this._bufferSize);
        }
        #endregion

        #region 方法
        public string ReadString()
        {
            return this.ReadString(0, this._bufferStream.Seek(0, SeekOrigin.End));
        }
        public string ReadString(Encoding encoding)
        {
            return this.ReadString(0, this._bufferStream.Seek(0, SeekOrigin.End), encoding);
        }
        public string ReadString(long startPosition, long endPosition)
        {
            return this.ReadString(startPosition, endPosition, Encoding.UTF8);
        }
        public string ReadString(long startPosition, long endPosition, Encoding encoding)
        {
            Preconditions.CheckLessZero(startPosition, "startPosition");
            Preconditions.CheckLessZero(endPosition - startPosition, "endPosition - startPosition");
            Preconditions.CheckLessZero(endPosition, "endPosition");
            Preconditions.CheckNotNull(encoding, "encoding");
            var buffer = this.ReadByte(startPosition, endPosition);
            var baseString = encoding.GetString(buffer);

            if (this.EnableSmartGzip)
                return gzip.GZipDecompressString(baseString, encoding);

            return baseString;
        }


        public string ReadStringOffset(int offset)
        {
            return this.ReadStringOffset(0, offset, Encoding.UTF8);
        }
        public string ReadStringOffset(int offset, Encoding encoding)
        {
            return this.ReadStringOffset(0, offset, encoding);
        }
        public string ReadStringOffset(long startPosition, int offset)
        {
            return this.ReadStringOffset(startPosition, offset, Encoding.UTF8);
        }
        public string ReadStringOffset(long startPosition, int offset, Encoding encoding)
        {
            Preconditions.CheckLessZero(startPosition, "startPosition");
            Preconditions.CheckLessZero(offset, "offset");
            Preconditions.CheckNotNull(encoding, "encoding");
            var buffer = this.ReadByteOffset(startPosition, offset);
            var baseString = encoding.GetString(buffer);

            if (this.EnableSmartGzip)
                return gzip.GZipDecompressString(baseString, encoding);

            return baseString;
        }


        public byte[] ReadByteOffset(long startPosition, int offset)
        {
            Preconditions.CheckLessZero(offset, "offset");
            return this.ReadByte(startPosition, startPosition + offset);
        }
        public byte[] ReadByte(int offset)
        {
            return this.ReadByte(0, offset);
        }
        public byte[] ReadByte(long startPosition, long endPosition)
        {
            Preconditions.CheckLessZero(endPosition - startPosition, "endPosition - startPosition");
            Preconditions.CheckLessZero(endPosition, "endPosition");
            this._bufferStream.Seek(startPosition, SeekOrigin.Begin);

            var length = (int)(endPosition - startPosition);
            var buffer = new byte[length];

            this._binaryReader = new BinaryReader(this._bufferStream);
            this._binaryReader.Read(buffer, 0, length);

            return buffer;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~BinaryFileRead()
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
