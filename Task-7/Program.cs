using System;
using System.IO;

namespace Task_7
{ // Task 7.1: modifying namespace of task 2.2 "BuyDairyProducts" (зокрема, клас Storage)
  // Task 7.2: Теоретичне завдання. Визначити ситуації, коли метод фіналізації винятків не буде спрацьовувати.
  // Відповідь: якщо до входу в блок finally (а) попали у нескінчений цикл або (б) програма неочікувано завершилася

    internal class Program
    {
        static Log log; // logfile for errors reporting

        static int Main(string[] args)
        {
            try
            {
                log = new Log(Log.errorLogName);
            }
            catch (Exception ex)
            {
                if (log == null)
                    Console.WriteLine("Cannot create Log file " + Log.errorLogName);
                Console.WriteLine("Log file error: " + ex.Message);
                return -1;
            }

            // trying to open the database for reading
            int tryCount = 3;
            StreamReader reader = null;
            do
            {
                try
                {
                    reader = new StreamReader(Storage.productsListName);
                    tryCount = 0;
                }
                catch (Exception ex)
                {
                    Log.PutRecord($"Error: " + ex.Message);
                    Console.WriteLine($"Enter new products list name (with path), {tryCount} attempts remained: ");
                    Storage.productsListName = Console.ReadLine();
                    tryCount--;
                }
            } while (tryCount > 0);

            Storage storage = new Storage(reader); // reading from the file

            if (Storage.GetCount() == 0)
            {
                Console.WriteLine("\r\nThe list of products in file is empty.");
            }
            else
            {
                Console.WriteLine("\r\nThe list of products from the file:");
                Console.WriteLine(storage);
            }

            Console.WriteLine("\r\nNow the randomly initialised list of products:");
            storage = new Storage(10);
            Console.WriteLine(storage);
            reader.Close();

            // show log file
            log.ReadLog(); // read from the log file to the memory
            Console.WriteLine("\r\nThe log file content:\r\n" + log);

            while (true)
            {
                Console.WriteLine("\r\nDo you want to change a log string? Enter Y or y if yes");
                string answer = Console.ReadLine();
            
                if (answer.ToUpper() == "Y")
                {
                    Console.Write("Enter the log line number to change: ");
                    int lineNo;
                    if (int.TryParse(Console.ReadLine(), out lineNo))
                    {
                        Console.WriteLine("Enter the new log line:");
                        string change = Console.ReadLine();
                        if (log.CorrectLine(lineNo, change))
                            Console.WriteLine("Changed in the memory and appended to the log");
                    }
                }
                else
                    break;
            }
            log.Close();
            return 0;
        }
    }
}
