using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.BF
{
    public abstract class BaseBinaryFile
    {
        #region 字段
        protected BufferedStream _bufferStream = null;
        protected FileStream _fileStream = null;
        protected int _bufferSize = 4096;
        protected SmartGzip _gzip = new SmartGzip();
        #endregion

        #region 属性
        /// <summary>
        /// 对象是否已释放
        /// </summary>
        public bool IsDisposed { get; protected set; }
        /// <summary>
        /// 是否开启字符串智能压缩
        /// </summary>
        public bool EnableSmartGzip { get; protected set; }
        /// <summary>
        /// 获取或设置当前流中的位置
        /// </summary>
        public long Position { get { return this._bufferStream.Position; } set { this._bufferStream.Position = value; } }
        /// <summary>
        /// 获取用字节表示的流长度
        /// </summary>
        public long Length { get { return this._bufferStream.Length; } }
        #endregion 

        #region 构造函数
        internal BaseBinaryFile(FileStream fileStream, bool enableSmartGzip = true, int bufferSize = 4096)
        {
            Preconditions.CheckNotNull(fileStream, "fileStream");
            if (bufferSize < 4096)
                bufferSize = 4096;

            this.EnableSmartGzip = enableSmartGzip;

            this._fileStream = fileStream;
            this._bufferSize = bufferSize;
            this._bufferStream = new BufferedStream(this._fileStream, this._bufferSize);
        }
        internal BaseBinaryFile(BufferedStream bufferStream, bool enableSmartGzip = true)
        {
            Preconditions.CheckNotNull(bufferStream, "bufferStream");

            this.EnableSmartGzip = enableSmartGzip;

            this._bufferStream = bufferStream;
        }
        internal BaseBinaryFile(string path, bool enableSmartGzip = true, FileShare share = FileShare.None, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, int bufferSize = 4096)
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
        /// 设置当前缓冲流的位置
        /// </summary>
        /// <param name="offset">相对于origin的字节偏移量</param>
        /// <param name="origin">相对于新位置的参考点</param>
        /// <returns>当前缓冲流中的新位置</returns>
        public virtual long Seek(long offset, SeekOrigin origin)
        {
            return this._bufferStream.Seek(offset, origin);
        }
        #endregion
    }
}
