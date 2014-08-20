using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.BF.Writer
{

    public abstract class BaseBinaryFileWrite : BaseBinaryFile
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
        internal BaseBinaryFileWrite(FileStream fileStream, bool enableSmartGzip = true, int bufferSize = 4096)
            : base(fileStream, enableSmartGzip, bufferSize)
        {
        }
        internal BaseBinaryFileWrite(BufferedStream bufferStream, bool enableSmartGzip = true)
            : base(bufferStream, enableSmartGzip)
        {
        }
        internal BaseBinaryFileWrite(string path, bool enableSmartGzip = true, FileShare share = FileShare.None, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, int bufferSize = 4096)
            : base(path, enableSmartGzip, share, mode, access, bufferSize)
        {
        }
        #endregion

    }
}
