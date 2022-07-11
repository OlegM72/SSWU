using System;
using System.Collections.Generic;

namespace Task_12_3
{
    internal class PolishNotation // Polish postfix (Reverse Polish) expression
    {
        public string Expression { get; set; }

        Dictionary<string, Lexeme> Lexemes;  // Operators and functions

        public PolishNotation(string expression, Dictionary<string, Lexeme> lexemes) // take reverse polish notation from a string
        {
            Expression = expression;
            SetOperatorsPrecedence(lexemes); // needed only to convert to a polish notation but used to avoid null reference error
        }

        void SetOperatorsPrecedence(Dictionary<string, Lexeme> lexemes)
        {
            Lexemes = lexemes; // we want a user to be able to change lexemes from outside, so we do not use a deep copy
        }

        public PolishNotation(MathExpression expression, Dictionary<string, Lexeme> lexemes) // take an expression and convert it to a polish notation
        {
            SetOperatorsPrecedence(lexemes);
            Expression = ConvertToPolishNotation(expression);
        }

        string GetDoubleNumber(string expression, ref int pos) // find a double number from poistion pos and return it as a string
        {
            string strNumber = "";
            while (pos < expression.Length) // starting from the given position pos
            {
                char chr = expression[pos];
                if (chr == '.' || Char.IsDigit(chr))
                    strNumber += chr;
                else { // found non digit and not a dot, going left by 1 char and finishing
                    pos--;
                    break;
                }
                pos++;
            }
            return strNumber;
        }

        string GetFunctionName(string expression, ref int pos) // find a function from poistion pos and return it as a string
        {
            string strFunc = "";
            while (pos < expression.Length) // starting from the given position pos
            {
                char chr = expression[pos];
                if (Char.IsLetter(chr))
                    strFunc += chr;
                else { // found non letter, going left by 1 char and finishing
                    pos--;
                    break;
                }
                pos++;
            }
            return strFunc;
        }

        internal string RemoveSpaces(string input)
        {
            string result = "";
            for (int i = 0; i < input.Length; i++)
                if (input[i] != ' ')
                    result += input[i];
            return result;
        }

        public string ConvertToPolishNotation(MathExpression expression) // convert expression to a polish notation
            // Shunting-yard algorithm by Edsger W.Dijkstra
            // Not implementing composite functions, functions with variable number of arguments, and unary operators.
        {
            Stack<string> operators_functions = new(); // operators and functions are pushed to the stack (LIFO list)
            string output = ""; // output expression, numbers are pushed right to the output which is queue (FIFO list)
            string input = RemoveSpaces(expression.Expression); // input expression to parse

            // Procedure: the input is processed one symbol at a time.
            // 1) If a variable or number is found, it is copied directly to the output
            // 2) If the symbol is an operator, it is pushed onto the operator stack.
            // 3) If the operator's precedence is lower than that of the operators at the top of the stack
            // 4) or the precedences are equal and the operator is left associative,
            // 5) then that operator is popped off the stack and added to the output.
            // 6) Finally, any remaining operators are popped off the stack and added to the output.
            for (int idx = 0; idx < input.Length; idx++)
            {
                char c = input[idx];
                if (Char.IsDigit(c)) // the current token is a number
                    { output += GetDoubleNumber(input, ref idx) + " "; } // push it to output queue
                else if (c == '(')
                    { operators_functions.Push("("); } // push it to the stack
                else if (c == ')')
                {
                    //	add from the top of the stack to output string in backward order everything till the "("
                    while (operators_functions.Count > 0 && (operators_functions.Peek() != "("))
                        output += operators_functions.Pop() + " ";
                    if (operators_functions.Count > 0) // if 0, then there is no "(", so ")" is a mismatch, skip it
                        operators_functions.Pop(); // remove the "(" from the top of the stack without adding to the output
                    // if an operator remained in the stack we have to pop it too - it was before the "("
                    // if (operators_functions.Count > 0 && (operators_functions.Peek() != "("))
                    //    output += operators_functions.Pop() + " ";
                }
                else if (Lexemes.ContainsKey(c.ToString())) // check if a symbol is a known operator
                {
                    char oper = c; // if yes, also check if it is unary minus
                    if (oper == '-' && (idx == 0 || (idx > 1 && 
                        !Char.IsDigit(input[idx-1]) && !(input[idx - 1] == ')'))))
                        oper = '~'; // mark it as unary minus operator (will be parsed later)

                    // in case of right associated operator "^" after the number we just put it to the stack (^2^3 -> ^ ^ 2 3)
                    if (!Lexemes[oper.ToString()].RightAssociated || (idx > 1 && !Char.IsDigit(input[idx - 1])))
                        //	put to output string all operators from the stack that have HIGHER priority, dictionary compares by Value
                        while (operators_functions.Count > 0 && 
                              (Lexemes[operators_functions.Peek()] >= Lexemes[oper.ToString()]))
                            output += operators_functions.Pop() + " ";
                    operators_functions.Push(oper.ToString()); // push the current operator to stack
                }
                else // here starts some function or an unknown operator
                {
                    if (!Char.IsLetter(c))
                        operators_functions.Push(c.ToString()); // unknown operator, just push it to stack
                    else
                        operators_functions.Push(GetFunctionName(input, ref idx)); // push the function to stack, even if it is
                                                                                  // an unknown function. Will parse at evaluation
                }
            }
            //	flush all remained operators from the stack to the output string
            foreach (string oper in operators_functions)
                output += oper + " ";
            return output; // output contains the Reverse Polish notation
        }

        // the function that calculates the result of applying an operator on two numbers
        // in case of unary operator, only the first number is used
        private double EvaluateOperationOnNumbers(Lexeme oper, double operand1, double operand2)
        {
            return oper.Evaluate(operand1, operand2);
        }

        public double Evaluate() // find a value of the expression in reverse polish notation
        {
            Stack<double> numbers = new(); // stack for numbers or partial expressions values (we parse them in the reverse order)
            string[] words = Expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Calculation.divisionByZeroFlag = false;

            for (int word = 0; word < words.Length; word++)
            {
                string currWord = words[word];
                if (Char.IsDigit(currWord[0])) // this is a number, push it all to stack
                    numbers.Push(Convert.ToDouble(currWord));
                else if (Lexemes.ContainsKey(currWord)) // if it is a known operator
                {
                    if (Lexemes[currWord].Unary)    // "~" || "sin" || "cos" || "tg"
                    {
                        // case of known UNARY operator
                        // check if the stack is empty: if so, the result of the partial expression is zero
                        // if not, we pop the last calculated value or number from stack
                        double lastValue = (numbers.Count > 0) ? numbers.Pop() : 0;

                        //	get the result of the operation and push it to the stack
                        numbers.Push(EvaluateOperationOnNumbers(Lexemes[currWord], lastValue, 0)); // second operand is not important
                        
                        // check division by zero (for tangens function)
                        if (Calculation.divisionByZeroFlag)
                            return 0;
                    }
                    else
                    {
                        // case of known NON UNARY operator
                        // return two numbers from stack in reverse order
                        double operand2 = numbers.Count > 0 ? numbers.Pop() : 0;
                        double operand1 = numbers.Count > 0 ? numbers.Pop() : 0;

                        //	get the result of the operation and push it to the stack
                        numbers.Push(EvaluateOperationOnNumbers(Lexemes[currWord], operand1, operand2));
                        
                        // check division by zero (for division operator)
                        if (Calculation.divisionByZeroFlag)
                            return 0;
                    }
                }
                else
                    throw new FormatException("Unknown operator of function in the parsed expression");
            } // words cycle

            return numbers.Pop(); // the stack top is the result of the whole expression
        }
        public override string ToString()
        {
            return Expression; //.Replace('~', '-'); - replacing hides the unary minuses, this is not good
        }
    }
}
