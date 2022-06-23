using System;
using System.IO;

namespace Task_6_1
{
    internal class Program
    {
        static int Main(string[] args)
        {
            try
            {
                string sourceFileName = "../../../database.txt";
                string resultFileName = "../../../zvit.txt";

                Accounting acc = null;
                using (StreamReader source = new StreamReader(sourceFileName))
                {
                    Console.WriteLine("Reading the database from " + sourceFileName);
                    acc = new Accounting(source);
                }
                if (acc == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Could not read the database, exiting...");
                    Console.ResetColor();
                    return -1;
                }

                Console.WriteLine("The database was read. This is the report on all flats:");
                Console.WriteLine(acc + "\r\n");

                using (StreamWriter result = new StreamWriter(resultFileName))
                    result.WriteLine(acc);
                Console.WriteLine("The report was also written to " + resultFileName);

                Console.WriteLine("\r\nThis is the report on the flat #7:");
                Console.WriteLine(acc.PrintFlatByNumber(7));

                Console.WriteLine("\r\nThis is the report on the flat #8:");
                Console.WriteLine(acc.PrintFlatByNumber(8));

                Console.WriteLine("Done");
                return 0;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                return -2;
            }
        }
    }
}
