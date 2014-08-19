using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.Reader
{
    public abstract class IBinaryFileRead
    {
        #region 字段
        protected BufferedStream _bufferStream = null;
        protected BinaryReader _binaryReader = null;
        protected FileStream _fileStream = null;
        protected int _bufferSize = 4096;
        protected SmartGzip _gzip = new SmartGzip();
        #endregion

        public abstract string ReadString();
        public abstract string ReadString(long startPosition, long endPosition);
        public abstract string ReadString(Encoding encoding);
        public abstract string ReadString(long startPosition, long endPosition, Encoding encoding);


        public abstract string ReadStringOffset(int offset);
        public abstract string ReadStringOffset(int offset, Encoding encoding);
        public abstract string ReadStringOffset(long startPosition, int offset);
        public abstract string ReadStringOffset(long startPosition, int offset, Encoding encoding);


        public abstract byte[] ReadByteOffset(long startPosition, int offset);
        public abstract byte[] ReadByte(int offset);
        public abstract byte[] ReadByte(long startPosition, long endPosition);

    }
}
