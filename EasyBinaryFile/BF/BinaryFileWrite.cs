using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.BF
{
    public class BinaryFileWrite : AbstractBinaryFileWrite, IDisposable
    {

        #region 构造
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="bufferStream">BufferStream</param>
        /// <param name="enableSmartGzip">是否开启字符串智能压缩</param>
        public BinaryFileWrite(BufferedStream bufferStream, bool enableSmartGzip = true)
            : base(bufferStream, enableSmartGzip)
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="fileStream">FileStream</param>
        /// <param name="enableSmartGzip">是否开启字符串智能压缩</param>
        /// <param name="bufferSize">缓冲区大小</param>
        public BinaryFileWrite(FileStream fileStream, bool enableSmartGzip = true, int bufferSize = 4096)
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
        public BinaryFileWrite(string path, bool enableSmartGzip = true, FileShare share = FileShare.None, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, int bufferSize = 4096)
            : base(path, enableSmartGzip, share, mode, access, bufferSize)
        {
        }
        #endregion

        #region 方法

        /// <summary>
        /// 向当前流中写入字符串，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="content">字符串。此方法将content写入到当前流末尾</param>
        public override void Write(string content)
        {
            this.Write(content, Encoding.UTF8);
        }
        /// <summary>
        /// 向当前流中写入字符串，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="content">字符串。此方法将content写入到当前流末尾</param>
        /// <param name="encoding">编码</param>
        public override void Write(string content, Encoding encoding)
        {
            this.Write(this.Length, content, encoding);
        }
        /// <summary>
        /// 向当前流中写入字节序列，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">字节数组。此方法将value写入到当前流末尾</param>
        public override void Write(byte[] value)
        {
            this.Write(this.Length, value);
        }


        /// <summary>
        /// 从指定起始位置，向当前流中写入字符串，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="content">字符串。此方法将content写入指定位置之后</param>
        public override void Write(long startPosition, string content)
        {
            this.Write(startPosition, content, Encoding.UTF8);
        }
        /// <summary>
        /// 从指定起始位置，向当前流中写入字符串，获取写入之后的末尾位置
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="content">字符串。此方法将content写入指定位置之后</param>
        /// <param name="endPosition">末尾位置</param>
        public override void Write(long startPosition, string content, out long endPosition)
        {
            this.Write(startPosition, content, Encoding.UTF8, out endPosition);
        }
        /// <summary>
        /// 从指定起始位置，向当前流中写入字符串，获取写入之后的末尾位置
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="content">字符串。此方法将content写入指定位置之后</param>
        /// <param name="encoding">编码</param>
        /// <param name="endPosition">末尾位置</param>
        public override void Write(long startPosition, string content, Encoding encoding, out long endPosition)
        {
            this.Write(startPosition, content, encoding);
            endPosition = this.Position;
        }
        /// <summary>
        /// 从指定起始位置，向当前流中写入字符串，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="content">字符串。此方法将content写入指定位置之后</param>
        /// <param name="encoding">编码</param>
        public override void Write(long startPosition, string content, Encoding encoding)
        {
            Preconditions.CheckNotBlank(content, "content");
            Preconditions.CheckNotNull(encoding, "encoding");


            if (content.Length > 100000 && this.EnableSmartGzip)
                content = _gzip.GZipCompressString(content, encoding);

            this.Write(startPosition, encoding.GetBytes(content));
        }


        /// <summary>
        /// 从指定起始位置，向当前流中写入字节序列，获取写入之后的末尾位置
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="value">字节数组。此方法将value写入到当前流末尾</param>
        /// <param name="endPosition">末尾位置</param>
        public override void Write(long startPosition, byte[] value, out long endPosition)
        {
            this.Write(startPosition, value);
            endPosition = startPosition + value.Length;
        }
        /// <summary>
        /// 从指定起始位置，向当前流中写入字节序列，获取写入之后的末尾位置
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="value">字节数组。此方法将value写入到当前流末尾</param>
        public override void Write(long startPosition, byte[] value)
        {
            Preconditions.CheckLessZero(startPosition, "startPosition");
            Preconditions.CheckNotNull(value, "data");

            this._bufferStream.Seek(startPosition, SeekOrigin.Begin);

            this._binaryWriter = new BinaryWriter(this._bufferStream);

            this._binaryWriter.Write(value);
            this._binaryWriter.Flush();
        }

        #endregion

        #region Dispose
        /// <summary>
        /// 释放由BinaryFileWrite所占的资源
        /// </summary>
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
