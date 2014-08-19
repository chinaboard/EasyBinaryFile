using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.BF.Reader
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
    }
}
