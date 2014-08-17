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

        #region 字段
        private const string gzipMark = "@@zip@@";
        #endregion

        #region 方法
        /// <summary>
        /// 压缩字符串并以Base64编码
        /// </summary>
        /// <param name="rawString">字符串</param>
        /// <returns>Base64字符串</returns>
        public string GZipCompressString(string rawString)
        {
            Preconditions.CheckNotBlank(rawString, "rawString");

            return GZipCompressString(rawString, Encoding.UTF8);
        }
        /// <summary>
        /// 以指定编码方式压缩字符串并以Base64编码
        /// </summary>
        /// <param name="rawString">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>Base64字符串</returns>
        public string GZipCompressString(string rawString, Encoding encoding)
        {
            Preconditions.CheckNotBlank(rawString, "rawString");
            Preconditions.CheckNotNull(encoding, "encoding");

            byte[] rawData = encoding.GetBytes(rawString);
            byte[] zippedData = CompressRawData(rawData);

            var zippedBase64String = gzipMark + Convert.ToBase64String(zippedData);

            return zippedBase64String.Length < rawString.Length ? zippedBase64String : rawString;
        }
        /// <summary>
        /// 解压缩Base64字符串
        /// </summary>
        /// <param name="zippedString">Base64字符串</param>
        /// <returns>字符串</returns>
        public string GZipDecompressString(string zippedString)
        {
            Preconditions.CheckNotBlank(zippedString, "zippedString");

            return GZipDecompressString(zippedString, Encoding.UTF8);
        }
        /// <summary>
        /// 以指定编码方式解压缩Base64字符串
        /// </summary>
        /// <param name="zippedString">Base64字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>字符串</returns>
        public string GZipDecompressString(string zippedString, Encoding encoding)
        {
            Preconditions.CheckNotBlank(zippedString, "zippedString");
            Preconditions.CheckNotNull(encoding, "encoding");

            if (!zippedString.StartsWith(gzipMark))
                return zippedString;

            var base64String = zippedString.Remove(0, gzipMark.Length);

            byte[] zippedData = Convert.FromBase64String(base64String);
            return encoding.GetString(DecompressRawData(zippedData));
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 压缩原始字节序列
        /// </summary>
        /// <param name="rawData">原始数据</param>
        /// <returns>压缩后的数据</returns>
        private byte[] CompressRawData(byte[] rawData)
        {
            Preconditions.CheckNotNull(rawData, "rawData");

            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }
        /// <summary>
        /// 解压缩原始字节序列
        /// </summary>
        /// <param name="zippedData">压缩后的数据</param>
        /// <returns>原始数据</returns>
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
        #endregion

    }
}
