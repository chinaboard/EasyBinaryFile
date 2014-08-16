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

            var ef = new BinaryFile("test.txt");
            var reader = ef.GetReader();
            var writer = ef.GetWriter();

            writer.Write("1234567890");
            Console.WriteLine(reader.ReadString());

        }
    }
}
