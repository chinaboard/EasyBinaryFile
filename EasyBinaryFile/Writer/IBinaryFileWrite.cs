using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.Writer
{
    public interface IBinaryFileWrite
    {
        void Write(string content);
        void Write(string content, Encoding encoding);
        void Write(byte[] buffer);


        void Write(long startPosition, string content);
        void Write(long startPosition, string content, out long endPosition);
        void Write(long startPosition, string content, Encoding encoding, out long endPosition);
        void Write(long startPosition, string content, Encoding encoding);
        

        void Write(long startPosition, byte[] buffer, out long endPosition);
        void Write(long startPosition, byte[] buffer);
    }
}
