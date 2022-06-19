using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Task_9
{
    internal class Program
    {
        const string priceCurrentFileName = "../../../Prices.txt";
        const string menuFileName = "../../../Menu.txt";
        const string courseFileName = "../../../Course.txt";
        const string resultFileName = "../../../Result.txt";
        
        static void Main(string[] args)
        {
            try
            {
                Course course = new(courseFileName);
                PriceCurrent priceCurrent = new(priceCurrentFileName);
                Menu menu = new(menuFileName);

                // interaction with the user, responding to different requests
                int respond = 0;
                do
                {
                    Console.WriteLine("\r\nEnter the number of the action you want:\r\n" +
                                      "1. Print the total price of menu (all dishes in one copy)\r\n" +
                                      "2. Print the list of dishes in menu with prices\r\n" +
                                      "3. Print all dishes in menu with descriptions but without prices\r\n" +
                                      "4. Print the pricecurrent\r\n" +
                                      "5. Print the currency courses\r\n" +
                                      "6. Change the currency (the current currency is " + Course.currentCurrency +")\r\n" +
                                      "0. Exit\r\n");
                    Console.Write("Your choice: ");
                    if (!Int32.TryParse(Console.ReadLine(), out respond) || respond < 0 || respond > 6)
                    {
                        Console.WriteLine("Incorrect action number!");
                        continue;
                    }
                    switch (respond)
                    {
                        case 0: return;
                        case 1:
                            {
                                try
                                {
                                    if (!MenuService.TryGetMenuTotalSum(menu, priceCurrent, course, out decimal menuPrice))
                                        Console.WriteLine("Cannot calculate the total price of the menu.");
                                    else
                                    {
                                        string result = $"The total price of the menu is {menuPrice:f2} {Course.currentCurrency}";
                                        Console.WriteLine(result);
                                        using (StreamWriter writer = new(resultFileName, false, Encoding.UTF8))
                                        {
                                            writer.WriteLine(result);
                                        }
                                    }
                                }
                                catch (ProductNotFound ex)
                                {
                                    Console.WriteLine($"The price of product {ex.GetProductName()} is not known. Exiting...");
                                    return;
                                }
                                break; 
                            }
                        case 2:
                            {
                                try
                                {
                                    string dishesList = MenuService.PrintListOfDishesWithPrices(menu, priceCurrent, course, out decimal menuPrice);
                                    if (dishesList == "")
                                        Console.WriteLine("Cannot calculate the total price of the menu, or some error occured");
                                    else
                                    {
                                        Console.WriteLine(dishesList);
                                        string result = $"The total price of the menu is {menuPrice:f2} {Course.currentCurrency}";
                                        Console.WriteLine(result);
                                        using (StreamWriter writer = new(resultFileName, false, Encoding.UTF8))
                                        {
                                            writer.WriteLine(dishesList);
                                            writer.Write(result);
                                        }
                                    }
                                }
                                catch (ProductNotFound ex)
                                {
                                    Console.WriteLine($"The price of product {ex.GetProductName()} is not known. Exiting...");
                                    return;
                                }
                                break;
                            }
                        case 3: { Console.WriteLine(menu); break; }
                        case 4: { Console.WriteLine(priceCurrent.PrintPrices(course)); break; }
                        case 5: { Console.WriteLine(course); break; }
                        case 6: 
                            {
                                int curr = (int)Course.currentCurrency;
                                Console.Write("Enter the new current currency (1 = UAH, 2 = USD, 3 = EUR): ");
                                if (!Int32.TryParse(Console.ReadLine(), out curr) || curr < 1 || curr > 3)
                                    Console.WriteLine("Incorrect currency number! The currency was not changed");
                                else
                                {
                                    Course.currentCurrency = (Course.Currency)(curr - 1);
                                    Console.WriteLine("Current currency is: " + Course.currentCurrency);
                                }
                                break; 
                            }
                        default: continue; // not needed in fact
                    }
                }
                while (true) ;

            }
            catch (ArgumentException ex)
            // Possible ArgumentException for Dictionary: "The same key already added"
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        } 
    }
}
