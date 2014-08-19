using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.BF.Reader
{
    interface IBinaryFileRead
    {
        #region 方法
        void Write(string content);
        void Write(string content, Encoding encoding);
        void Write(byte[] value);


        void Write(long startPosition, string content);
        void Write(long startPosition, string content, out long endPosition);
        void Write(long startPosition, string content, Encoding encoding, out long endPosition);
        void Write(long startPosition, string content, Encoding encoding);


        void Write(long startPosition, byte[] value, out long endPosition);
        void Write(long startPosition, byte[] value);



        void Write(bool value);
        void Write(char value);
        void Write(double value);
        void Write(float value);
        void Write(int value);
        void Write(long value);
        void Write(decimal value);
        void Write(short value);
        void Write(uint value);
        void Write(ulong value);
        void Write(ushort value);
        void Write(byte value);
        void Write(sbyte value);


        #endregion
    }
}
