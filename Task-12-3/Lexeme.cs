using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_12_3
{
    internal class Lexeme : IComparable<Lexeme> // class to represent operators, functions, and their corresponding delegates
    {
        public int Priority { get; set; }              // precedence, that is, which operator or function is executed before others
        public bool Unary { get; set; }                // if true, this operation or function requires only the first operand
        public bool RightAssociated { get; set; }      // if true, the calculation is performed from right to left
        public Func<double, double, double> Calculate; // delegate for the value calculation (double operand1, double operand2)

        public Lexeme(int priority, bool unary, bool rightAssociated, Func<double, double, double> func)
        {
            Priority = priority;
            Unary = unary;
            RightAssociated = rightAssociated;
            Calculate = func;
        }

        // the function that calculates the result of applying an operator on two numbers
        // in case of unary operator, only the first number is used
        public double Evaluate(double operand1, double operand2)
        {
            return Calculate(operand1, operand2);
        }

        public int CompareTo(Lexeme other) // compare by Priority
        {
            return Priority.CompareTo(other.Priority);
        }

        public static bool operator >=(Lexeme operand1, Lexeme operand2)
        {
            return operand1.Priority >= operand2.Priority;
        }

        public static bool operator <=(Lexeme operand1, Lexeme operand2)
        {
            return operand1.Priority <= operand2.Priority;
        }
    }
}
