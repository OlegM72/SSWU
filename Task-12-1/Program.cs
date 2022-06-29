using System;
using System.IO;
using System.Collections.Generic;

namespace Task_12_1
{
    internal class Program
    {
        static void Execute()
        {
            Storage.utilizationLogName = "../../../UtilizationLog.txt";

            // trying to open the database for reading
            try
            {
                using (StreamReader reader = new StreamReader("../../../Database1.txt"))
                {
                    Storage storage1 = new Storage(reader); // reading from the file 1
                    if (storage1.GetCount() == 0)
                    {
                        Console.WriteLine("The list of products in file 1 is empty.");
                    }
                    else
                    {
                        Console.WriteLine("The list of products from the file 1:");
                        Console.WriteLine(storage1);
                    }
                }
                using (StreamReader reader = new StreamReader("../../../Database2.txt"))
                {
                    Storage storage2 = new Storage(reader); // reading from the file 2
                    if (storage2.GetCount() == 0)
                    {
                        Console.WriteLine("\r\nThe list of products in file 2 is empty.");
                    }
                    else
                    {
                        Console.WriteLine("\r\nThe list of products from the file 2:");
                        Console.WriteLine(storage2);
                    }
                }
                Console.WriteLine("\r\nList of removed expired products:");
                Storage.PrintUtilizationLog();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
	
        static void Main(string[] args)
        {
            Execute();
        }
    }
}