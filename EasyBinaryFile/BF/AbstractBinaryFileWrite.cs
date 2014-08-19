using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.BF
{

    public abstract class AbstractBinaryFileWrite : AbstractBaseBinaryFile
    {
        #region 字段
        protected BinaryWriter _binaryWriter = null;
        #endregion

        #region 属性
        /// <summary>
        /// 基础二进制写入流
        /// </summary>
        public BinaryWriter BaseWriter { get { return new BinaryWriter(this._bufferStream); } }
        #endregion

        #region 构造函数
        internal AbstractBinaryFileWrite(FileStream fileStream, bool enableSmartGzip = true, int bufferSize = 4096)
            : base(fileStream, enableSmartGzip, bufferSize)
        {
        }
        internal AbstractBinaryFileWrite(BufferedStream bufferStream, bool enableSmartGzip = true)
            : base(bufferStream, enableSmartGzip)
        {
        }
        internal AbstractBinaryFileWrite(string path, bool enableSmartGzip = true, FileShare share = FileShare.None, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, int bufferSize = 4096)
            : base(path, enableSmartGzip, share, mode, access, bufferSize)
        {
        }
        #endregion

        #region 抽象方法
        public abstract void Write(string content);
        public abstract void Write(string content, Encoding encoding);
        public abstract void Write(byte[] value);


        public abstract void Write(long startPosition, string content);
        public abstract void Write(long startPosition, string content, out long endPosition);
        public abstract void Write(long startPosition, string content, Encoding encoding, out long endPosition);
        public abstract void Write(long startPosition, string content, Encoding encoding);


        public abstract void Write(long startPosition, byte[] value, out long endPosition);
        public abstract void Write(long startPosition, byte[] value);
        #endregion

        #region 方法
        /// <summary>
        /// 向当前流中写入布尔型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">布尔型</param>
        public virtual void Write(bool value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 向当前流中写入字符型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">字符型</param>
        public virtual void Write(char value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 向当前流中写入双精度浮点型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">双精度浮点型</param>
        public virtual void Write(double value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 向当前流中写入浮点型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">浮点型</param>
        public virtual void Write(float value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 向当前流中写入整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">整型</param>
        public virtual void Write(int value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 向当前流中写入长整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">长整型</param>
        public virtual void Write(long value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 将一个十进制值写入当前流，并将流位置提升十六个字节。
        /// </summary>
        /// <param name="value">要写入的十进制值</param>
        public virtual void Write(decimal value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 向当前流中写入短整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">短整型</param>
        public virtual void Write(short value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 向当前流中写入无符号整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">无符号整型</param>
        public virtual void Write(uint value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 向当前流中写入无符号长整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">无符号长整型</param>
        public virtual void Write(ulong value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 向当前流中写入无符号短整型，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">无符号短整型</param>
        public virtual void Write(ushort value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 向当前流中写入一个字节，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">要写入的字节</param>
        public virtual void Write(byte value)
        {
            this.BaseWriter.Write(value);
        }
        /// <summary>
        /// 向当前流中写入一个有符号字节，并将此流中的当前位置提升写入的字节数
        /// </summary>
        /// <param name="value">要写入的字节</param>
        public virtual void Write(sbyte value)
        {
            this.BaseWriter.Write(value);
        }
        #endregion
    }
}
