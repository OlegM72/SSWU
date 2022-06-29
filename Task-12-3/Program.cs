using System;
using System.IO;

namespace Task_12_3
{
    internal class Program
    {
        static void Calculate(string expressionsPath, string answersPath)
        {
            try
            {
                using (StreamReader reader = new(expressionsPath))
                using (StreamReader answers = new(answersPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) is not null) // while not end of stream
                    {
                        MathExpression expression = new(line);
                        if (expression == null)
                            throw new ArgumentException("Could not read the expression");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("The expression read from the file: " + expression);
                        Console.ResetColor();
                        PolishNotation polishNotation = new(expression);
                        Console.WriteLine("It's reverse polish notation is:   " + polishNotation);
                        line = answers.ReadLine();
                        if (line is not null)
                            Console.WriteLine("Correct value of the expression is:    " + line);
                        double calculatedResult = polishNotation.Evaluate();
                        Console.Write("Calculated value of the expression is: ");
                        if (polishNotation.divisionByZeroFlag)
                            Console.WriteLine("Division by zero\r\n");
                        else 
                            Console.WriteLine($"{calculatedResult:F11}\r\n");
                    }
                    Console.WriteLine("Done. New functions or operators can be easily added in the program");
                    Console.WriteLine("Adding them by the user also can be easily implemented but not yet the logic of their calculation");
                }
            }
            catch { throw; }
        }

        static void Main(string[] args)
        {
            try
            {
                Calculate("../../../Expressions.txt", "../../../CorrectResultsForCheck.txt");
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
