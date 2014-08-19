using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.BF
{
    public abstract class AbstractBinaryFileRead : AbstractBaseBinaryFile
    {
        #region 字段
        protected BinaryReader _binaryReader = null;
        #endregion

        #region 属性
        /// <summary>
        /// 基础二进制读取流
        /// </summary>
        public BinaryReader BaseReader { get { return new BinaryReader(this._bufferStream); } }
        #endregion

        #region 构造函数
        internal AbstractBinaryFileRead(FileStream fileStream, bool enableSmartGzip = true, int bufferSize = 4096)
            : base(fileStream, enableSmartGzip, bufferSize)
        {
        }
        internal AbstractBinaryFileRead(BufferedStream bufferStream, bool enableSmartGzip = true)
            : base(bufferStream, enableSmartGzip)
        {
        }
        internal AbstractBinaryFileRead(string path, bool enableSmartGzip = true, FileShare share = FileShare.None, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, int bufferSize = 4096)
            : base(path, enableSmartGzip, share, mode, access, bufferSize)
        {
        }
        #endregion

        #region 抽象方法
        public abstract string ReadString();
        public abstract string ReadString(long startPosition, long endPosition);
        public abstract string ReadString(Encoding encoding);
        public abstract string ReadString(long startPosition, long endPosition, Encoding encoding);


        public abstract string ReadStringOffset(int count);
        public abstract string ReadStringOffset(int count, Encoding encoding);
        public abstract string ReadStringOffset(long startPosition, int count);
        public abstract string ReadStringOffset(long startPosition, int count, Encoding encoding);


        public abstract byte[] ReadByteOffset(long startPosition, int offset);
        public abstract byte[] ReadByte(int count);
        public abstract byte[] ReadByte(long startPosition, long endPosition);
        #endregion
    }
}
