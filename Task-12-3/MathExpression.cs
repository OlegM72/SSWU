using System;

namespace Task_12_3
{
    internal class MathExpression // temporary class for the future: now it just contains a correct
                                  // infix expression string but later its calculation method may be added
    {
        public string Expression { get; set; }

        public MathExpression(string expression)
        {
            Expression = expression;
        }

        public double Evaluate()
        {
            throw new NotImplementedException(
                "This method can be implemented later, for example, to compare the evaluation results");
        }

        public override string ToString()
        {
            return Expression;
        }
    }
}
