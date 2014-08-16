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
        private int _bufferSize = 4096;
        private SmartGzip gzip = new SmartGzip();
        private long fileEndPosition { get { return this._bufferStream.Seek(0, SeekOrigin.End); } }
        #endregion

        #region 属性
        /// <summary>
        /// 对象是否已释放
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// 是否开启字符串智能压缩
        /// </summary>
        public bool EnableSmartGzip { get; private set; }
        /// <summary>
        /// 基础二进制写入流
        /// </summary>
        public BinaryWriter BaseWriter { get { return this._binaryWriter; } }
        #endregion

        #region 构造
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="bufferStream">BufferStream</param>
        /// <param name="enableSmartGzip">是否开启字符串智能压缩</param>
        public BinaryFileWrite(BufferedStream bufferStream, bool enableSmartGzip = true, int bufferSize = 4096)
        {
            Preconditions.CheckNotNull(bufferStream, "bufferStream");
            if (bufferSize < 4096)
                bufferSize = 4096;

            this.EnableSmartGzip = enableSmartGzip;

            this._bufferSize = bufferSize;
            this._bufferStream = bufferStream;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="fileStream">FileStream</param>
        /// <param name="enableSmartGzip">是否开启字符串智能压缩</param>
        /// <param name="bufferSize">缓冲区大小</param>
        public BinaryFileWrite(FileStream fileStream, bool enableSmartGzip = true, int bufferSize = 4096)
        {
            Preconditions.CheckNotNull(fileStream, "fileStream");
            if (bufferSize < 4096)
                bufferSize = 4096;

            this.EnableSmartGzip = enableSmartGzip;

            this._fileStream = fileStream;
            this._bufferSize = bufferSize;
            this._bufferStream = new BufferedStream(this._fileStream, this._bufferSize);
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
        public BinaryFileWrite(string path, bool enableSmartGzip = true, FileShare share = FileShare.None, FileMode mode = FileMode.Open, FileAccess access = FileAccess.ReadWrite, int bufferSize = 4096)
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

        /// <summary>
        /// 向当前流中写入字符串，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="content">字符串。此方法将content写入到当前流末尾</param>
        public virtual void Write(string content)
        {
            this.Write(content, Encoding.UTF8);
        }
        /// <summary>
        /// 向当前流中写入字符串，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="content">字符串。此方法将content写入到当前流末尾</param>
        /// <param name="encoding">编码</param>
        public virtual void Write(string content, Encoding encoding)
        {
            this.Write(this.fileEndPosition, content, encoding);
        }
        /// <summary>
        /// 向当前流中写入字节序列，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">字节数组。此方法将value写入到当前流末尾</param>
        public virtual void Write(byte[] value)
        {
            this.Write(this.fileEndPosition, value);
        }


        /// <summary>
        /// 从指定起始位置，向当前流中写入字符串，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="content">字符串。此方法将content写入指定位置之后</param>
        public virtual void Write(long startPosition, string content)
        {
            this.Write(startPosition, content, Encoding.UTF8);
        }
        /// <summary>
        /// 从指定起始位置，向当前流中写入字符串，获取写入之后的末尾位置
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="content">字符串。此方法将content写入指定位置之后</param>
        /// <param name="endPosition">末尾位置</param>
        public virtual void Write(long startPosition, string content, out long endPosition)
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
        public virtual void Write(long startPosition, string content, Encoding encoding, out long endPosition)
        {
            this.Write(startPosition, content, encoding);
            endPosition = startPosition + content.Length;
        }
        /// <summary>
        /// 从指定起始位置，向当前流中写入字符串，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="content">字符串。此方法将content写入指定位置之后</param>
        /// <param name="encoding">编码</param>
        public virtual void Write(long startPosition, string content, Encoding encoding)
        {
            Preconditions.CheckNotBlank(content, "content");
            Preconditions.CheckNotNull(encoding, "encoding");


            if (this.EnableSmartGzip)
                content = gzip.GZipCompressString(content, encoding);

            this.Write(startPosition, encoding.GetBytes(content));
        }


        /// <summary>
        /// 从指定起始位置，向当前流中写入字节序列，获取写入之后的末尾位置
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="value">字节数组。此方法将value写入到当前流末尾</param>
        /// <param name="endPosition">末尾位置</param>
        public virtual void Write(long startPosition, byte[] value, out long endPosition)
        {
            this.Write(startPosition, value);
            endPosition = startPosition + value.Length;
        }
        /// <summary>
        /// 从指定起始位置，向当前流中写入字节序列，获取写入之后的末尾位置
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="value">字节数组。此方法将value写入到当前流末尾</param>
        public virtual void Write(long startPosition, byte[] value)
        {
            Preconditions.CheckLessZero(startPosition, "startPosition");
            Preconditions.CheckNotNull(value, "data");

            this._bufferStream.Seek(startPosition, SeekOrigin.Begin);

            this._binaryWriter = new BinaryWriter(this._bufferStream);

            this._binaryWriter.Write(value);
            this._binaryWriter.Flush();
        }


        /// <summary>
        /// 向当前流中写入布尔型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">布尔型</param>
        public virtual void Write(bool value)
        {
            this.Write(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 向当前流中写入字符型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">字符型</param>
        public virtual void Write(char value)
        {
            this.Write(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 向当前流中写入双精度浮点型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">双精度浮点型</param>
        public virtual void Write(double value)
        {
            this.Write(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 向当前流中写入浮点型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">浮点型</param>
        public virtual void Write(float value)
        {
            this.Write(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 向当前流中写入整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">整型</param>
        public virtual void Write(int value)
        {
            this.Write(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 向当前流中写入长整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">长整型</param>
        public virtual void Write(long value)
        {
            this.Write(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 向当前流中写入短整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">短整型</param>
        public virtual void Write(short value)
        {
            this.Write(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 向当前流中写入无符号整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">无符号整型</param>
        public virtual void Write(uint value)
        {
            this.Write(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 向当前流中写入无符号长整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">无符号长整型</param>
        public virtual void Write(ulong value)
        {
            this.Write(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 向当前流中写入无符号短整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">无符号短整型</param>
        public virtual void Write(ushort value)
        {
            this.Write(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 向当前流中写入一个字节，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">要写入的字节</param>
        public virtual void Write(byte value)
        {
            this.Write(new byte[] { value });
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
