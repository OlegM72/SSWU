using System;
using System.IO;

namespace Task_8_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sourceFileName = "../../../ip_stat.txt";
            string resultFileName1 = "../../../all_attendings.txt";
            string resultFileName2 = "../../../ips_summary.txt";

            try { IPCollection.Dialog(sourceFileName, resultFileName1, resultFileName2); }
            catch (Exception ex) when (ex.Message != null)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }
}
