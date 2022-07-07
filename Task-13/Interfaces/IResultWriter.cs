using System.Collections.Generic;

namespace Task13
{
    internal interface IResultWriter
    {
        void WritePerson(List<string> calculateExpressions,
            string filePath = @"../../../Files/Result.txt");
    }

}
