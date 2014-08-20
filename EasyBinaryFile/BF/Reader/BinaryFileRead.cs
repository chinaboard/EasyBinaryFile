using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.BF.Reader
{
    public class BinaryFileRead : AbstractBinaryFileRead, IDisposable
    {

        #region 构造
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="bufferStream">BufferStream</param>
        /// <param name="enableSmartGzip">是否开启字符串智能压缩</param>
        public BinaryFileRead(BufferedStream bufferStream, bool enableSmartGzip = true)
            : base(bufferStream, enableSmartGzip)
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="fileStream">FileStream</param>
        /// <param name="enableSmartGzip">是否开启字符串智能压缩</param>
        /// <param name="bufferSize">缓冲区大小</param>
        public BinaryFileRead(FileStream fileStream, bool enableSmartGzip = true, int bufferSize = 4096)
            : base(fileStream, enableSmartGzip, bufferSize)
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="share">文件共享方式</param>
        /// <param name="mode">文件打开方式</param>
        /// <param name="access">文件控制方式</param>
        /// <param name="enableSmartGzip">是否开启字符串智能压缩</param>
        /// <param name="bufferSize">缓冲区大小</param>
        public BinaryFileRead(string path, bool enableSmartGzip = true, FileShare share = FileShare.None, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, int bufferSize = 4096)
            : base(path, enableSmartGzip, share, mode, access, bufferSize)
        {
        }
        #endregion

        #region 方法
        /// <summary>
        /// 设置当前缓冲流的位置
        /// </summary>
        /// <param name="offset">相对于origin的字节偏移量</param>
        /// <param name="origin">相对于新位置的参考点</param>
        /// <returns>当前缓冲流中的新位置</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return this._bufferStream.Seek(offset, origin);
        }


        /// <summary>
        /// 读取缓冲区的所有内容到字符串
        /// </summary>
        /// <returns>字符串</returns>
        public virtual string ReadString()
        {
            return this.ReadString(Encoding.UTF8);
        }
        /// <summary>
        /// 读取缓冲区的指定位置内容到字符串
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="endPosition">结束位置</param>
        /// <returns>字符串</returns>
        public virtual string ReadString(long startPosition, long endPosition)
        {
            return this.ReadString(startPosition, endPosition, Encoding.UTF8);
        }
        /// <summary>
        /// 读取缓冲区的所有内容到字符串
        /// </summary>
        /// <param name="encoding">编码</param>
        /// <returns>字符串</returns>
        public virtual string ReadString(Encoding encoding)
        {
            return this.ReadString(0, this.Length, encoding);
        }
        /// <summary>
        /// 读取缓冲区的指定位置内容到字符串
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="endPosition">结束位置</param>
        /// <param name="encoding">编码</param>
        /// <returns>字符串</returns>
        public virtual string ReadString(long startPosition, long endPosition, Encoding encoding)
        {
            Preconditions.CheckLessZero(startPosition, "startPosition");
            Preconditions.CheckLessZero(endPosition - startPosition, "endPosition - startPosition");
            Preconditions.CheckLessZero(endPosition, "endPosition");
            Preconditions.CheckNotNull(encoding, "encoding");
            var buffer = this.ReadByte(startPosition, endPosition);
            var baseString = encoding.GetString(buffer);

            if (this.EnableSmartGzip)
                return _gzip.GZipDecompressString(baseString, encoding);

            return baseString;
        }


        /// <summary>
        /// 读取缓冲区指定起始位到要读取的字符数到字符串
        /// </summary>
        /// <param name="count">要读取的字符数</param>
        /// <returns>字符串</returns>
        public virtual string ReadStringOffset(int count)
        {
            return this.ReadStringOffset(0, count, Encoding.UTF8);
        }
        /// <summary>
        /// 读取缓冲区起始位到指定偏移位的内容到字符串
        /// </summary>
        /// <param name="count">要读取的字符数</param>
        /// <param name="encoding">编码</param>
        /// <returns>字符串</returns>
        public virtual string ReadStringOffset(int count, Encoding encoding)
        {
            return this.ReadStringOffset(0, count, encoding);
        }
        /// <summary>
        /// 读取缓冲区指定起始位到要读取的字符数到字符串
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="count">要读取的字符数</param>
        /// <returns>字符串</returns>
        public virtual string ReadStringOffset(long startPosition, int count)
        {
            return this.ReadStringOffset(startPosition, count, Encoding.UTF8);
        }
        /// <summary>
        /// 读取缓冲区指定起始位到指定偏移位的内容到字符串
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="count">要读取的字符数</param>
        /// <param name="encoding">编码</param>
        /// <returns>字符串</returns>
        public virtual string ReadStringOffset(long startPosition, int count, Encoding encoding)
        {
            Preconditions.CheckLessZero(startPosition, "startPosition");
            Preconditions.CheckLessZero(count, "offset");
            Preconditions.CheckNotNull(encoding, "encoding");
            var buffer = this.ReadByteOffset(startPosition, count);
            var baseString = encoding.GetString(buffer);

            if (this.EnableSmartGzip)
                return _gzip.GZipDecompressString(baseString, encoding);

            return baseString;
        }


        /// <summary>
        /// 读取缓冲区指定起始位到指定偏移位的内容到字节数组
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="offset">offset</param>
        /// <returns>字节数组</returns>
        public virtual byte[] ReadByteOffset(long startPosition, int offset)
        {
            Preconditions.CheckLessZero(offset, "offset");
            return this.ReadByte(startPosition, startPosition + offset);
        }
        /// <summary>
        /// 读取缓冲区起始位到要读取的字符数到字节数组
        /// </summary>
        /// <param name="count">要读取的字符数</param>
        /// <returns>字节数组</returns>
        public virtual byte[] ReadByte(int count)
        {
            return this.ReadByte(0, count);
        }
        /// <summary>
        /// 读取缓冲区指定起始位到指定结束位置的内容到字节数组
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="endPosition">结束位置</param>
        /// <returns>字节数组</returns>
        public virtual byte[] ReadByte(long startPosition, long endPosition)
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
        /// <summary>
        /// 释放由BinaryFileRead所占的资源
        /// </summary>
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
