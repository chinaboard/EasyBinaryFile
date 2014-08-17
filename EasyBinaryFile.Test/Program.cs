using EasyBinaryFile.Reader;
using EasyBinaryFile.Utility;
using EasyBinaryFile.Writer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyBinaryFile.Test
{
    class Program
    {
        private const int Count = 100000;
        static void Main(string[] args)
        {
            Test1();
            Console.WriteLine();
            Test2();
            Console.ReadLine();
        }
        static void Test1()
        {
            File.Delete("zip.txt");
            File.Delete("unzip.txt");

            //Console.WriteLine(ef.EnableSmartGzip);

            var smartText = "";

            Random rand = new Random();
            while (smartText.Length < 10000)
                smartText += rand.Next(16).ToString("x");

            while (smartText.Length < 1000000)
                smartText += smartText;

            var ef1 = new BinaryFile("zip.txt", true, FileShare.ReadWrite);
            var ef2 = new BinaryFile("unzip.txt", false, FileShare.ReadWrite);
            var reader1 = ef1.GetReader();
            var writer1 = ef1.GetWriter();
            var reader2 = ef2.GetReader();
            var writer2 = ef2.GetWriter();
            Console.WriteLine("zip test result");
            Console.WriteLine("SmartGzip : " + ef1.EnableSmartGzip);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            writer1.Write(smartText);
            Console.WriteLine("raw = read : " + (reader1.ReadString() == smartText));
            writer1.Write(12);
            sw.Stop();
            Console.WriteLine("zip Time : " + sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("zip Size : " + writer1.BaseWriter.BaseStream.Seek(0, SeekOrigin.End) / 1024 + "KB");

            Console.WriteLine();

            Console.WriteLine("unzip test result");
            Console.WriteLine("SmartGzip : " + ef2.EnableSmartGzip);
            sw.Reset();
            sw.Start();
            writer2.Write(smartText);
            Console.WriteLine("raw = read : " + (reader2.ReadString() == smartText));
            writer2.Write(12);
            sw.Stop();
            Console.WriteLine("unzip Time : " + sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("unzip Size : " + writer2.BaseWriter.BaseStream.Seek(0, SeekOrigin.End) / 1024 + "KB");

            ef1.Dispose();
            ef2.Dispose();

            File.Delete("zip.txt");
            File.Delete("unzip.txt");

        }

        static void Test2()
        {
            //unzip disk IO test
            File.Delete(@"z:\unzip.dta");
            Stopwatch sw = new Stopwatch();
            Random rand = new Random();
            long result = 0;
            long result1 = 0;


            var ef = new BinaryFile(@"z:\unzip.dta", false);
            var writer = ef.GetWriter();
            var reader = ef.GetReader();
            Console.WriteLine("SmartGzip : " + ef.EnableSmartGzip);
            Console.WriteLine("Count : " + Count);


            sw.Start();
            for (int i = 0; i < Count; i++)
            {
                var num = rand.Next();
                result += num;
                writer.Write(num);
            }
            sw.Stop();
            Console.WriteLine("write : " + sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("write : " + (Count / sw.ElapsedMilliseconds + " IOPS"));
            sw.Restart();
            reader.Position = 0;

            for (int i = 0; i < Count; i++)
            {
                int n = reader.BaseReader.ReadInt32();
                result1 += n;
            }

            ef.Dispose();
            sw.Stop();
            Console.WriteLine("read : " + sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("read : " + (Count / sw.ElapsedMilliseconds + " IOPS"));
            Console.WriteLine();
            Console.WriteLine("write = read : " + (result1 == result));
        }
    }
}
