using System;
using System.IO;

namespace Task_6_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sourceFileName = "../../../database.txt";
            string resultFileName = "../../../zvit.txt";

            StreamReader source = new StreamReader(sourceFileName);
            Console.WriteLine("Reading the database from " + sourceFileName);
            Accounting acc = new Accounting(source);
            source.Close();

            Console.WriteLine("The database was read. This is the report on all flats:");
            Console.WriteLine(acc + "\r\n");

            StreamWriter result = new StreamWriter(resultFileName);
            result.WriteLine(acc);
            result.Close();
            Console.WriteLine("The report was also written to " + resultFileName);

            Console.WriteLine("\r\nThis is the report on the flat #7:");
            Console.WriteLine(acc.PrintFlatByNumber(7));

            Console.WriteLine("\r\nThis is the report on the flat #8:");
            Console.WriteLine(acc.PrintFlatByNumber(8));
            
            Console.WriteLine("Done");

        }
    }
}
