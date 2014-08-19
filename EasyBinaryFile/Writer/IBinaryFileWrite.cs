using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.Writer
{
 
    public abstract class IBinaryFileWrite
    {
        #region 字段
        protected BufferedStream _bufferStream = null;
        protected BinaryWriter _binaryWriter = null;
        protected FileStream _fileStream = null;
        protected int _bufferSize = 4096;
        protected SmartGzip _gzip = new SmartGzip();
        #endregion
        public abstract void Write(string content);
        public abstract void Write(string content, Encoding encoding);
        public abstract void Write(byte[] value);


        public abstract void Write(long startPosition, string content);
        public abstract void Write(long startPosition, string content, out long endPosition);
        public abstract void Write(long startPosition, string content, Encoding encoding, out long endPosition);
        public abstract void Write(long startPosition, string content, Encoding encoding);


        public abstract void Write(long startPosition, byte[] value, out long endPosition);
        public abstract void Write(long startPosition, byte[] value);


        public abstract void Write(bool value);
        public abstract void Write(char value);
        public abstract void Write(double value);
        public abstract void Write(float value);
        public abstract void Write(int value);
        public abstract void Write(long value);
        public abstract void Write(short value);
        public abstract void Write(uint value);
        public abstract void Write(ulong value);
        public abstract void Write(ushort value);
        public abstract void Write(byte value);
    }
}
