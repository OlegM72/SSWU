using System;
using System.IO;
using System.Collections.Generic;

namespace Task_12_3
{
    internal class Calculation
    {
        public static bool divisionByZeroFlag = false;

        private static double DivisionResult(double num1, double num2)
        {
            if (num2 == 0)
            {
                divisionByZeroFlag = true;
                return 0;
            }
            return num1 / num2;
        }

        private static double TangensResult(double angle)
        {
            // the number is never exact, so we suppose it can be close to 90 or -90
            if (Math.Abs(Math.Abs(angle) - 90) < 0.000001)
            {
                divisionByZeroFlag = true;
                return 0;
            }
            return Math.Tan(DegreesToRadians(angle));
        }

        private static double DegreesToRadians(double angle)
        {
            return angle * Math.PI / 180f;
        }

        public static void Calculate(string expressionsPath, string answersPath)
        {
            try
            {
                // create a list of operators and functions with their calculation delegates
                // this list can be changed on fly, then the next expressions will use the new list
                Dictionary<string, Lexeme> operators_and_functions = new() { };
                operators_and_functions.Add("~", new(4, true, false, (n1, n2) => -n1)); // unary minus ("-1")
                operators_and_functions.Add("sin", new(4, true, false, (n1, n2) => Math.Sin(DegreesToRadians(n1))));
                operators_and_functions.Add("cos", new(4, true, false, (n1, n2) => Math.Cos(DegreesToRadians(n1))));
                operators_and_functions.Add("tg", new(4, true, false, (n1, n2) => TangensResult(n1)));
                operators_and_functions.Add("^", new(3, false, true, (n1, n2) => Math.Pow(n1, n2)));
                operators_and_functions.Add("*", new(2, false, false, (n1, n2) => n1 * n2));
                operators_and_functions.Add("/", new(2, false, false, (n1, n2) => DivisionResult(n1, n2)));
                operators_and_functions.Add("+", new(1, false, false, (n1, n2) => n1 + n2));
                operators_and_functions.Add("-", new(1, false, false, (n1, n2) => n1 - n2));
                operators_and_functions.Add("(", new(0, false, false, (_, _) => 0)); // to exclude errors when parsing

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

                        PolishNotation polishNotation = new(expression, operators_and_functions);
                        Console.WriteLine("It's reverse polish notation is:   " + polishNotation);
                        line = answers.ReadLine();
                        if (line is not null)
                            Console.WriteLine("Correct value of the expression is:    " + line);
                        double calculatedResult = polishNotation.Evaluate();
                        Console.Write("Calculated value of the expression is: ");
                        if (divisionByZeroFlag)
                            Console.WriteLine("Division by zero\r\n");
                        else
                            Console.WriteLine($"{calculatedResult:F11}\r\n");
                    }
                    Console.WriteLine("Done. New functions or operators can be easily added in the program");
                    Console.WriteLine("Changing the list of functions by the user also can be implemented");
                }
            }
            catch { throw; }
        }
    }
}
