using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyBinaryFile
{
    public interface IBinaryFile
    {
        byte[] Read(long startPosition, long endPosition);

        byte[] ReadOffset(long startPosition, int offset);

        string Read(long startPosition, long endPosition, Encoding encoding);

        string ReadOffset(long startPosition, int offset, Encoding encoding);

        void Write(long startPosition, byte[] buffer);

        void Write(long startPosition, string content, Encoding encoding);

        void Write(long startPosition, byte[] buffer, out long endPosition);

        void Write(long startPosition, string content, Encoding encoding, out long endPosition);
    }
}
