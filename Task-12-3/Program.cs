using System;
using System.Collections.Generic;
using System.IO;

namespace Task_12_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Calculation.Calculate("../../../Expressions.txt", "../../../CorrectResultsForCheck.txt");
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
