using System;
using System.Text;
using System.IO;

namespace Task_6_2
{
    internal class Program
    {
        static void Execution()
        {
            string sourceFileName = "../../../Source.txt";
            string resultFileName = "../../../Result.txt";

            try
            {
                Text standardText = new Text(); // trying to use the empty text constructor
                Console.WriteLine("This is an empty text:");
                Console.WriteLine(standardText);

                Text givenText = new Text("This is an   example   text given in the function... \"Execution\"");
                Console.WriteLine("\r\nThis is a given text:");
                Console.WriteLine(givenText);
                Console.WriteLine("\r\nThis is a given text without the garbage:");
                givenText.RemoveGarbage();
                Console.WriteLine(givenText);

                Text readText;
                using (StreamReader inputFile = new(sourceFileName, Encoding.UTF8))
                    readText = new Text(inputFile);
                Console.WriteLine("\r\nThis is a text from the file " + sourceFileName);
                Console.WriteLine(readText);
                Console.WriteLine("\r\nThis is the text without the garbage:");
                readText.RemoveGarbage();
                Console.WriteLine(readText);
                Console.WriteLine("\r\nThis is the text broken into paragraphs:");
                Text splittedText = new Text(readText); // create a copy of the Text to test its copying feature
                splittedText.SplitToParagraphs();
                Console.WriteLine(splittedText);

                using (StreamWriter outputFile = new(resultFileName, false, Encoding.UTF8))
                    splittedText.WriteToFile(outputFile);
                Console.WriteLine("\r\nIt was also written out to the file " + resultFileName);
                
                Console.WriteLine("\r\nThe shortest and longest words in the text for each paragraph are:");
                splittedText.FindShortLongWordsInParagraphs();
                Console.WriteLine(splittedText.ShortLongWordsInParagraphs());
                Console.WriteLine("\r\nDone.");
            }
            catch
            {
                throw; // to the Main method
            }
        }

        static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            try { Execution(); }
            catch (Exception ex) when (ex.Message != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                return -1;
            }
            return 0;
        }
    }
}
