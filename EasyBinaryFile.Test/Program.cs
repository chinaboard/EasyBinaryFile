﻿using EasyBinaryFile.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            File.Delete("zip.txt");
            File.Delete("unzip.txt");

            //Console.WriteLine(ef.EnableSmartGzip);

            var smartText = "";

            Random rand = new Random();
            while (smartText.Length < 10000)
                smartText += rand.Next(16).ToString("x");

            while (smartText.Length < 100000)
                smartText += smartText;

            var ef1 = new BinaryFile("zip.txt", true, FileShare.ReadWrite);
            var ef2 = new BinaryFile("unzip.txt", true, FileShare.ReadWrite);
            var reader1 = ef1.GetReader();
            var writer1 = ef1.GetWriter();
            var reader2 = ef2.GetReader();
            var writer2 = ef2.GetWriter();

            Console.WriteLine(ef1.EnableSmartGzip);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            writer1.Write(smartText);
            Console.WriteLine(reader1.ReadString() == smartText);
            writer1.Write(12);
            sw.Stop();
            Console.WriteLine("zip : " + sw.ElapsedMilliseconds);


            Console.WriteLine(ef2.EnableSmartGzip);
            sw.Reset();
            sw.Start();
            writer2.Write(smartText);
            Console.WriteLine(reader2.ReadString() == smartText);
            writer2.Write(12);
            sw.Stop();
            Console.WriteLine("unzip : " + sw.ElapsedMilliseconds);


            File.Delete("zip.txt");
            File.Delete("unzip.txt");

        }
    }
}
