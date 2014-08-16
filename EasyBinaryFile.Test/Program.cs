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
            //Test1();
            //Test2();
            Test3();
            Console.Read();
        }

        static void Test1()
        {
            File.Delete("Test.txt");

            BinaryFile ef = new BinaryFile(@"Test.txt", FileMode.OpenOrCreate);

            var str = "testString";

            var data = Encoding.UTF8.GetBytes(str);

            var length = data.Length;

            ef.Write(0, Encoding.UTF8.GetBytes(str));

            var str2 = str.Substring(2, 3);

            var r1 = ef.Read(2, 2 + Encoding.UTF8.GetBytes(str2).Length);
            var r2 = ef.ReadOffset(2, Encoding.UTF8.GetBytes(str2).Length);


            var rs1 = Encoding.UTF8.GetString(r1);
            var rs2 = Encoding.UTF8.GetString(r2);

            Console.WriteLine(str2);
            Console.WriteLine(rs1);
            Console.WriteLine(rs2);
            Console.WriteLine(rs1 == rs2);
            Console.WriteLine(rs1 == str2);

            ef.Dispose();

            File.Delete("Test.txt");
        }

        static void Test2()
        {
            File.Delete("Test.txt");

            BinaryFile ef = new BinaryFile(@"Test.txt", FileMode.OpenOrCreate);

            var str = "testString";

            var data = Encoding.UTF8.GetBytes(str);

            var length = data.Length;

            ef.Write(0, Encoding.UTF8.GetBytes(str));

            var str2 = str.Substring(2, 3);

            var r1 = ef.Read(2, 2 + Encoding.UTF8.GetBytes(str2).Length, Encoding.UTF8);
            var r2 = ef.ReadOffset(2, Encoding.UTF8.GetBytes(str2).Length, Encoding.UTF8);

            Console.WriteLine(str2);
            Console.WriteLine(r1);
            Console.WriteLine(r2);
            Console.WriteLine(r1 == r2);
            Console.WriteLine(r1 == str2);

            ef.Dispose();

            File.Delete("Test.txt");
        }

        static void Test3()
        {
            File.Delete("Test.txt");

            BinaryFile ef = new BinaryFile(@"Test.txt", FileMode.OpenOrCreate);

            var str = "testString";

            var data = Encoding.UTF8.GetBytes(str);

            var length = data.Length;

            long persion;

            ef.Write(0, Encoding.UTF8.GetBytes(str), out persion);

            Console.WriteLine(ef.Read(0, persion, Encoding.UTF8));

            ef.Write(2, Encoding.UTF8.GetBytes("ABCD"), out persion);

            Console.WriteLine(ef.Read(0, ef.Length, Encoding.UTF8));
            Console.WriteLine(ef.Read(2, persion, Encoding.UTF8));


            ef.Dispose();

            File.Delete("Test.txt");
        }
    }
}
