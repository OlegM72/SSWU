using System.Collections.Generic;

namespace Task13
{
    interface IExpresionReader
    {
        List<string> ReadExpresion(string filePath = @"../../../Files/Persons.txt");

    }
}
