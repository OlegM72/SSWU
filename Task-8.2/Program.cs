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

            StreamReader source = new StreamReader(sourceFileName);
            Console.WriteLine("Reading the IP list from " + sourceFileName);
            IPCollection ip = new(source);
            source.Close();

            Console.WriteLine("The list was read. This is the summary on all IPs:");
            Console.WriteLine(ip);

            StreamWriter result = new StreamWriter(resultFileName1);
            result.WriteLine(ip);
            result.Close();
            Console.WriteLine("The report was also written to " + resultFileName1);

            Console.WriteLine("This is the summary of the most popular hours / days:");
            Console.WriteLine(ip.PrintIPsSummary());
            result = new StreamWriter(resultFileName2);
            result.WriteLine(ip.PrintIPsSummary());
            result.Close();
            Console.WriteLine("The summary was also written to " + resultFileName2);
        }
    }
}
