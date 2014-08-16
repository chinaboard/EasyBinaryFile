using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.Utility
{
    public class SmartGzip
    {
        private const string gzipMark = "@@zip@@";

        public string GZipCompressString(string rawString)
        {
            Preconditions.CheckNotBlank(rawString, "rawString");

            return GZipCompressString(rawString, Encoding.UTF8);
        }

        public string GZipCompressString(string rawString, Encoding encoding)
        {
            Preconditions.CheckNotBlank(rawString, "rawString");
            Preconditions.CheckNotNull(encoding, "encoding");

            byte[] rawData = encoding.GetBytes(rawString);
            byte[] zippedData = CompressRawData(rawData);

            var zippedBase64String = gzipMark + Convert.ToBase64String(zippedData);

            return zippedBase64String.Length < rawString.Length ? zippedBase64String : rawString;
        }

        public string GZipDecompressString(string zippedString)
        {
            Preconditions.CheckNotBlank(zippedString, "zippedString");

            return GZipDecompressString(zippedString, Encoding.UTF8);
        }

        public string GZipDecompressString(string zippedString, Encoding encoding)
        {
            Preconditions.CheckNotBlank(zippedString, "zippedString");
            Preconditions.CheckNotNull(encoding, "encoding");

            if (!zippedString.StartsWith(gzipMark))
                return zippedString;

            byte[] zippedData = Convert.FromBase64String(zippedString.Remove(0, gzipMark.Length));
            return encoding.GetString(DecompressRawData(zippedData));
        }

        private byte[] CompressRawData(byte[] rawData)
        {
            Preconditions.CheckNotNull(rawData, "rawData");

            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }

        private byte[] DecompressRawData(byte[] zippedData)
        {
            Preconditions.CheckNotNull(zippedData, "zippedData");

            MemoryStream ms = new MemoryStream(zippedData);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }
    }
}
