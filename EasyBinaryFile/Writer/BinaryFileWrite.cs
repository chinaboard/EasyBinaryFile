using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.Writer
{
    public class BinaryFileWrite : IBinaryFileWrite, IDisposable
    {
        #region 字段
        private BufferedStream _bufferStream = null;
        private BinaryWriter _binaryWriter = null;
        private FileStream _fileStream = null;
        private int _bufferSize = 4096000;
        #endregion

        #region 构造
        public BinaryFileWrite(BufferedStream bufferStream)
        {
            Preconditions.CheckNotNull(bufferStream, "bufferStream");
            this._bufferStream = bufferStream;
        }

        public BinaryFileWrite(FileStream fileStream, int bufferSize = 4096000)
        {
            Preconditions.CheckNotNull(fileStream, "fileStream");
            if (bufferSize < 4096)
                bufferSize = 4096;

            this._fileStream = fileStream;
            this._bufferSize = bufferSize;
            this._bufferStream = new BufferedStream(this._fileStream, this._bufferSize);
        }
        public BinaryFileWrite(string path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None, int bufferSize = 4096000)
        {
            Preconditions.CheckNotBlank(path, "path");
            if (!File.Exists(path))
                mode = FileMode.OpenOrCreate;
            if (bufferSize < 4096)
                bufferSize = 4096;

            this._fileStream = File.Open(path, mode, access, share);
            this._bufferSize = bufferSize;
            this._bufferStream = new BufferedStream(this._fileStream, this._bufferSize);
        }
        #endregion

        #region 方法
        public void Write(string content)
        {
            this.Write(0, content,Encoding.UTF8);
        }
        public void Write(string content, Encoding encoding)
        {
            this.Write(0, content, encoding);
        }
        public void Write(byte[] buffer)
        {
            this.Write(0, buffer);
        }


        public void Write(long startPosition, byte[] buffer, out long endPosition)
        {
            this.Write(startPosition, buffer);
            endPosition = startPosition + buffer.Length;
        }
        public void Write(long startPosition, string content, out long endPosition)
        {
            this.Write(startPosition, content, Encoding.UTF8, out endPosition);
        }
        public void Write(long startPosition, string content, Encoding encoding, out long endPosition)
        {
            this.Write(startPosition, content, encoding);
            endPosition = startPosition + content.Length;
        }
        public void Write(long startPosition, string content, Encoding encoding)
        {
            Preconditions.CheckNotBlank(content, "content");
            Preconditions.CheckNotNull(encoding, "encoding");

            this.Write(startPosition, encoding.GetBytes(content));
        }


        public void Write(long startPosition, string content)
        {
            this.Write(startPosition, content, Encoding.UTF8);
        }
        public void Write(long startPosition, byte[] buffer)
        {
            Preconditions.CheckLessZero(startPosition, "startPosition");
            Preconditions.CheckNotNull(buffer, "data");

            this._bufferStream.Seek(startPosition, SeekOrigin.Begin);

            this._binaryWriter = new BinaryWriter(this._bufferStream);

            this._binaryWriter.Write(buffer);
            this._binaryWriter.Flush();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~BinaryFileWrite()
        {
            this.Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
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
