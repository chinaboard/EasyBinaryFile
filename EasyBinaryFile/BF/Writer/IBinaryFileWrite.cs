using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.BF.Writer
{
    interface IBinaryFileWrite
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



        void Write(bool value, int count);
        void Write(char value, int count);
        void Write(double value, int count);
        void Write(float value, int count);
        void Write(int value, int count);
        void Write(long value, int count);
        void Write(decimal value, int count);
        void Write(short value, int count);
        void Write(uint value, int count);
        void Write(ulong value, int count);
        void Write(ushort value, int count);
        void Write(byte value, int count);
        void Write(sbyte value, int count);

        #endregion
    }
}
