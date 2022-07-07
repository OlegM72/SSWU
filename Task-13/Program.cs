using System;
using System.Collections.Generic;

namespace Task13
{
    internal class Program
    {
        public static void Message(string? message, ConsoleColor color = ConsoleColor.White)
        // replacement for Console.WriteLine - to change the informative method if needed
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            try
            {
                // open the shop with three cashiers and max size of queue = 10
                Shop threeCashiersShop = new(10, new List<Cashier>() {
                    (new Cashier (1, 0.25)),
                    (new Cashier (2, 0.50)),
                    (new Cashier (3, 0.75))
                });
                // start the service
                threeCashiersShop.ExecuteShopProcess();
            }
            catch (Exception ex)
            {
                Message(ex.Message, ConsoleColor.Red);
                Message(ex.StackTrace, ConsoleColor.DarkYellow);
            }
        }
    }
}