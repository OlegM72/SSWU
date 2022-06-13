using System;
using System.IO;

namespace Task_8_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sourceFileName1 = "../../../database1.txt";
            string sourceFileName2 = "../../../database2.txt";
            string resultFileName = "../../../zvit.txt";

            StreamReader source = new StreamReader(sourceFileName1);
            Console.WriteLine("Reading the database from " + sourceFileName1);
            Accounting acc1 = new Accounting(source);
            source.Close();

            Console.WriteLine("The database 1 was read. This is the report on all flats:");
            Console.WriteLine(acc1 + "\r\n");

            source = new StreamReader(sourceFileName2);
            Console.WriteLine("Reading the database from " + sourceFileName2);
            Accounting acc2 = new Accounting(source);
            source.Close();

            Console.WriteLine("The database 2 was read. This is the report on all flats:");
            Console.WriteLine(acc2 + "\r\n");

            try
            {
                StreamWriter result = new StreamWriter(resultFileName);
                result.WriteLine(acc1 + acc2);
                result.WriteLine();
                result.WriteLine(acc1 - acc2);
                result.Close();
                Console.WriteLine("Union and Substraction of the databases are in " + resultFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
