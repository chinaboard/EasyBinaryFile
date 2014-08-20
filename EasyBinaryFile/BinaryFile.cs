using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EasyBinaryFile.Utility;
using EasyBinaryFile.BF;
using EasyBinaryFile.BF.Reader;
using EasyBinaryFile.BF.Writer;

namespace EasyBinaryFile
{
    public class BinaryFile : AbstractBaseBinaryFile, IDisposable
    {

        #region 字段
        private BinaryWriter _binaryWriter = null;
        private BinaryReader _binaryReader = null;
        #endregion

        #region 构造
        public BinaryFile(BufferedStream bufferedStream, bool enableSmartGzip = true)
            : base(bufferedStream, enableSmartGzip)
        {
        }

        public BinaryFile(FileStream fileStream, bool enableSmartGzip = true, int bufferSize = 4096)
            : base(fileStream, enableSmartGzip, bufferSize)
        {
        }

        public BinaryFile(string path, bool enableSmartGzip = true, FileShare share = FileShare.ReadWrite, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.ReadWrite, int bufferSize = 4096)
            : base(path, enableSmartGzip, share, mode, access, bufferSize)
        {
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取BinaryFile.BinaryFileRead的实例对象
        /// </summary>
        /// <returns>BinaryFile.BinaryFileRead实例对象</returns>
        public BinaryFileRead GetReader()
        {
            Preconditions.CheckDisposed(this.IsDisposed, this.GetType().Name);
            return new BinaryFileRead(this._bufferStream, this.EnableSmartGzip);
        }
        /// <summary>
        /// 获取BinaryFile.BinaryFileWrite的实例对象
        /// </summary>
        /// <returns>BinaryFile.BinaryFileWrite实例对象</returns>
        public BinaryFileWrite GetWriter()
        {
            Preconditions.CheckDisposed(this.IsDisposed, this.GetType().Name);
            return new BinaryFileWrite(this._bufferStream, this.EnableSmartGzip);
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
