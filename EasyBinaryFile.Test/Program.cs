using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyBinaryFile.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
            Console.Read();
        }
        static void Test1()
        {
            File.Delete("test.txt");
            var ef = new BinaryFile("test.txt", FileShare.ReadWrite);
            var reader = ef.GetReader();
            var writer = ef.GetWriter();

            Console.WriteLine(ef.EnableSmartGzip);

            var smartText = "";
            Random rand = new Random();
            while (smartText.Length < 200)
                smartText += rand.Next(16).ToString("x");

            //writer.Write(smartText);
            //writer.Write(BitConverter.GetBytes(int.MinValue));
            //Console.WriteLine(reader.ReadString() == smartText);
            //ef.Dispose();

            //writer.Write(BitConverter.GetBytes(int.MinValue));
            byte bt = 2;
            writer.Write(bt);
            //File.Delete("test.txt");
        }
    }
}
