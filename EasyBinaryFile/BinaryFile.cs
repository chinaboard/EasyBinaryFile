using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile
{
    public class BinaryFile : IBinaryFile, IDisposable
    {

        #region 字段
        private BufferedStream _bufferStream = null;
        private BinaryWriter _binaryWriter = null;
        private BinaryReader _binaryReader = null;
        private FileStream _fileStream = null;
        private int _bufferSize = 4096000;
        #endregion

        #region 属性
        public bool IsDisposed { get; private set; }
        public long Length { get { return this._bufferStream.Seek(0, SeekOrigin.End); } }
        #endregion

        #region 构造
        public BinaryFile(FileStream fileStream, int bufferSize = 4096000)
        {
            if (fileStream == null)
                throw new ArgumentNullException();
            if (bufferSize < 4096)
                throw new ArgumentOutOfRangeException();

            this._fileStream = fileStream;
            this._bufferSize = bufferSize;
            this.IsDisposed = false;
        }

        public BinaryFile(string path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, int bufferSize = 4096000)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException();
            if (!File.Exists(path) && mode != FileMode.OpenOrCreate)
                throw new FileNotFoundException();
            if (bufferSize < 4096)
                throw new ArgumentOutOfRangeException();

            this._fileStream = File.Open(path, mode, access, share);
            this._bufferSize = bufferSize;
            this.IsDisposed = false;
        }
        #endregion

        #region 读方法
        public byte[] Read(long startPosition, long endPosition)
        {
            Preconditions.CheckDisposed(this.IsDisposed, this.GetType().Name);
            Preconditions.CheckLessZero(endPosition - startPosition, "endPosition - startPosition");
            Preconditions.CheckLessZero(endPosition, "endPosition");
            this.InitBufferStream();
            this._bufferStream.Seek(startPosition, SeekOrigin.Begin);

            var length = (int)(endPosition - startPosition);
            var buffer = new byte[length];

            this._binaryReader = new BinaryReader(this._bufferStream);
            this._binaryReader.Read(buffer, 0, length);

            return buffer;
        }

        public byte[] ReadOffset(long startPosition, int offset)
        {
            Preconditions.CheckDisposed(this.IsDisposed, this.GetType().Name);
            Preconditions.CheckLessZero(startPosition, "startPosition");
            Preconditions.CheckLessZero(offset, "offset");
            this.InitBufferStream();
            this._bufferStream.Seek(startPosition, SeekOrigin.Begin);

            var length = (int)(offset);
            var buffer = new byte[length];

            this._binaryReader = new BinaryReader(this._bufferStream);
            this._binaryReader.Read(buffer, 0, length);

            return buffer;
        }

        public string Read(long startPosition, long endPosition, Encoding encoding)
        {
            Preconditions.CheckNotNull(encoding, "encoding");
            byte[] buffer = this.Read(startPosition, endPosition);
            return encoding.GetString(buffer);
        }

        public string ReadOffset(long startPosition, int offset, Encoding encoding)
        {
            Preconditions.CheckNotNull(encoding, "encoding");
            byte[] buffer = this.ReadOffset(startPosition, offset);
            return encoding.GetString(buffer);
        }
        #endregion

        #region 写方法
        public void Write(long startPosition, byte[] buffer)
        {
            Preconditions.CheckDisposed(this.IsDisposed, this.GetType().Name);
            Preconditions.CheckLessZero(startPosition, "startPosition");
            Preconditions.CheckNotNull(buffer, "data");

            this.InitBufferStream();
            this._bufferStream.Seek(startPosition, SeekOrigin.Begin);

            this._binaryWriter = new BinaryWriter(this._bufferStream);

            this._binaryWriter.Write(buffer);
            this._binaryWriter.Flush();
        }

        public void Write(long startPosition, string content, Encoding encoding)
        {
            Preconditions.CheckDisposed(this.IsDisposed, this.GetType().Name);
            Preconditions.CheckNotBlank(content, "content");
            Preconditions.CheckNotNull(encoding, "encoding");

            this.Write(startPosition, encoding.GetBytes(content));
        }

        public void Write(long startPosition, byte[] buffer, out long endPosition)
        {
            this.Write(startPosition, buffer);
            endPosition = startPosition + buffer.Length;
        }

        public void Write(long startPosition, string content, Encoding encoding, out long endPosition)
        {
            this.Write(startPosition, content, encoding);
            endPosition = startPosition + content.Length;
        }
        #endregion

        #region 私有方法
        private void InitBufferStream()
        {
            this._bufferStream = new BufferedStream(this._fileStream, this._bufferSize);
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
