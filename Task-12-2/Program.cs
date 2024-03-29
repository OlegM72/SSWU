﻿using System;
using System.IO;
using System.Collections.Generic;

namespace Task_12_2
{
    internal class Program
    {
        static void Execute()
        {
            Storage.utilizationLogName = "../../../UtilizationLog.txt"; // we do not use it in this task but the log is filled anyway

            // trying to open the database for reading
            try
            {
                using (StreamReader reader1 = new StreamReader("../../../Database1.txt"))
                using (StreamReader reader2 = new StreamReader("../../../Database2.txt"))
                {
                    Storage storage1 = new Storage(reader1); // reading from the file 1
                    if (storage1.GetCount() == 0)
                    {
                        Console.WriteLine("The list of products in file 1 is empty.");
                    }
                    else
                    {
                        Console.WriteLine("The list of products from the file 1:");
                        Console.WriteLine(storage1);
                    }
                    Storage storage2 = new Storage(reader2); // reading from the file 2
                    if (storage2.GetCount() == 0)
                    {
                        Console.WriteLine("\r\nThe list of products in file 2 is empty.");
                    }
                    else
                    {
                        Console.WriteLine("\r\nThe list of products from the file 2:");
                        Console.WriteLine(storage2);
                    }

                    Console.WriteLine("\r\nHere is the combined list of products:");
                    Storage storageSum = storage1 + storage2;
                    Console.WriteLine(storageSum);

                    int type;
                    do
                    {
                        Console.WriteLine("\r\nDo you want to sort the list by...");
                        Console.WriteLine($"{ProductComparer.ComparerType.Hash} - press 1");
                        Console.WriteLine($"{ProductComparer.ComparerType.Name} - press 2");
                        Console.WriteLine($"{ProductComparer.ComparerType.Price} - press 3");
                        Console.WriteLine($"{ProductComparer.ComparerType.Weight} - press 4");
                        Console.WriteLine($"Exit - press 0");
                        if (!Int32.TryParse(Console.ReadLine(), out type))
                            Console.WriteLine("Wrong reply. Try again...");
                        else
                            if (type > 0 && type < 5)
                            {
                                storageSum.SetComparer((ProductComparer.ComparerType)(type - 1));
                                storageSum.Sort();
                                Console.WriteLine(storageSum);
                            }
                    } while (type != 0);                }
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