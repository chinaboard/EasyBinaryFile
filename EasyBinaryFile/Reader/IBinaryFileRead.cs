using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.Reader
{
    public interface IBinaryFileRead
    {
        string ReadString();
        string ReadString(Encoding encoding);
        string ReadString(long startPosition, long endPosition);
        string ReadString(long startPosition, long endPosition, Encoding encoding);


        string ReadStringOffset(int offset);
        string ReadStringOffset(int offset, Encoding encoding);
        string ReadStringOffset(long startPosition, int offset);
        string ReadStringOffset(long startPosition, int offset, Encoding encoding);


        byte[] ReadByteOffset(long startPosition, int offset);
        byte[] ReadByte(int offset);
        byte[] ReadByte(long startPosition, long endPosition);
    }
}
