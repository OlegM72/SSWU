using System;
using System.IO;

namespace Task_14_3
{
    internal class Program
    {
        static void PatternsTest()
        {
            try
            {
                Storage storage = Storage.Instance(); // creating empty Storage as Singleton
                Storage secondStorageInstance = Storage.Instance();
                if (storage == secondStorageInstance)
                    Console.WriteLine("Great, there is only a single instance of Storage!");
                else
                    Console.WriteLine("Singleton has failed!");

                storage.ProduceProducts(); // storage uses Abstract Factories as a client class

                Console.WriteLine("\r\nThe list of products in the storage:");
                if (storage.GetCount() == 0)
                    Console.WriteLine("The list is empty.");
                else
                    Console.WriteLine(storage);
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
            PatternsTest();
        }
    }
}
