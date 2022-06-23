using System;
using System.IO;

namespace Task_8_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string productsListName1 = "../../../Database1.txt";
            string productsListName2 = "../../../Database2.txt";

            // trying to open the database for reading
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(productsListName1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening file for reading: " + productsListName1 + "\r\n" + ex);
            } 

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
            reader.Close();

            try
            {
                reader = new StreamReader(productsListName2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening file for reading: " + productsListName2 + "\r\n" + ex);
            }

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

            Console.WriteLine("\r\nList of all products in two storages (each appears once):");
            Console.WriteLine(storage1 + storage2);

            Console.WriteLine("\r\nList of all products in the first storage without the elements in the second storage:");
            Console.WriteLine(storage1 - storage2);

            Console.WriteLine("\r\nList of all common products in two storages:");
            Console.WriteLine(storage1 * storage2);

        }
    }
}
