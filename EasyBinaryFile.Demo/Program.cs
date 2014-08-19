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

namespace EasyBinaryFile.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
            Console.WriteLine();
            //Test2();
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
            Console.WriteLine("Test1  Start");
            Console.WriteLine("String.Length = " + smartText.Length);

            Console.WriteLine("-------zip--------");

            var ef1 = new BinaryFile("zip.txt", true, FileShare.ReadWrite);
            var ef2 = new BinaryFile("unzip.txt", false, FileShare.ReadWrite);
            var reader1 = ef1.GetReader();
            var writer1 = ef1.GetWriter();
            var reader2 = ef2.GetReader();
            var writer2 = ef2.GetWriter();
            Console.WriteLine("SmartGzip : " + ef1.EnableSmartGzip);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            writer1.Write(smartText);
            Console.WriteLine("raw = read : " + (reader1.ReadString() == smartText));
            writer1.Write(12);
            sw.Stop();
            Console.WriteLine("zip Time : " + sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("zip Size : " + writer1.BaseWriter.BaseStream.Seek(0, SeekOrigin.End) / 1024 + "KB");

            Console.WriteLine("-------zip--------");
            Console.WriteLine();
            Console.WriteLine("------unzip-------");
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
            Console.WriteLine("------unzip-------");
            Console.WriteLine("Test1  End");

        }

        static void Test2()
        {
            //unzip disk IO test
            File.Delete(@"z:\test.dta");
            Stopwatch sw = new Stopwatch();

            const int Count = 100;

            var ef = new BinaryFile(@"z:\test.dta", true);
            var writer = ef.GetWriter();
            var reader = ef.GetReader();
            Console.WriteLine("SmartGzip : " + ef.EnableSmartGzip);
            Console.WriteLine("Count : " + Count);

            var str = "                                                                                                          ";
            //string str = "testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest";
            str += str; str += str; str += str; str += str; str += str; str += str;
            str += str; str += str; str += str; str += str; str += str; str += str;
            long length =123;
            sw.Start();
            for (int i = 0; i < Count; i++)
            {
                writer.Write(0, str, out length);
            }
            sw.Stop();
            Console.WriteLine("write : " + sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("write : " + (Count / (sw.ElapsedMilliseconds + 1) * length + " KB/s"));
            sw.Restart();
            reader.Position = 0;

            string n = string.Empty;
            for (int i = 0; i < Count; i++)
            {
                n = reader.ReadString(0, length);
            }
            sw.Stop();
            ef.Dispose();

            Console.WriteLine("read : " + sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("read : " + (Count / (sw.ElapsedMilliseconds + 1) * length + " KB/s"));
            Console.WriteLine(length);
            Console.WriteLine(length * Count / 1024 + "KB");
            Console.WriteLine(str.Length * Count / 1024 + "KB");
        }
    }
}
