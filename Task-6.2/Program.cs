using System;
using System.Collections.Generic;
using System.IO;

namespace Task_6_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sourceFileName = "../../../Source.txt";
            string resultFileName = "../../../Result.txt";

            Text standardText = new Text(); // trying to use the empty text constructor
            Console.WriteLine("This is an empty text:");
            Console.WriteLine(standardText);
            
            Text givenText = new Text("This is an   example   text given by the function... Main");
            Console.WriteLine("\nThis is a given text:");
            Console.WriteLine(givenText);
            Console.WriteLine("\nThis is a given text without the garbage:");
            givenText.RemoveGarbage();
            Console.WriteLine(givenText);

            StreamReader inputFile = new StreamReader(sourceFileName);
            Text readText = new Text(inputFile);
            Console.WriteLine("\nThis is a text from the file " + sourceFileName);
            Console.WriteLine(readText);
            inputFile.Close();
            Console.WriteLine("\nThis is the text without the garbage:");
            readText.RemoveGarbage();
            Console.WriteLine(readText);
            Console.WriteLine("\nThis is the text broken into paragraphs:");
            Text splittedText = new Text(readText); // create a copy of the Text to test its copying feature
            splittedText.SplitToParagraphs();
            Console.WriteLine(splittedText);
            Console.WriteLine("\nIt was also written out to the file " + resultFileName);
            StreamWriter outputFile = new StreamWriter(resultFileName);
            splittedText.WriteToFile(outputFile);
            outputFile.Close();
            Console.WriteLine("\nThe shortest and longest words in the text for each paragraph are:");
            splittedText.FindShortLongWordsInParagraphs();
            splittedText.PrintShortLongWordsInParagraphs();
            Console.WriteLine("Done.");
        }
    }
}
