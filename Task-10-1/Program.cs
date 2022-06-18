using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Translator
{
    class Program
    {
        // The main changes in the implementation that was made at the lecture:
        // 1. Translator now counts and keeps multiple punctuation marks and spaces BEFORE and AFTER words
        // 2. UTF-8 encoding used, so Cyrillic letters allowed
        // 3. Chars register is kept
        // 4. Multiple strings are allowed in the text
        // 5. New type of Exception defined to handle "Word not found" event
        // 6. The translation is also written to the text file

        static public string textFileName = "../../../Text.txt";
        static public string dictionaryFileName = "../../../Dictionary.txt";
        static public string translationFileName = "../../../Translation.txt";

        static void Main(string[] args)
        {
            Dictionary<string, string> dictionary;
            List<string> text;
            try
            {
                if (!File.Exists(textFileName))
                    throw new FileNotFoundException(textFileName);
                text = FileIO.ReadText(textFileName);

                if (!File.Exists(dictionaryFileName))
                    throw new FileNotFoundException(dictionaryFileName);
                dictionary = FileIO.ReadDictionary(dictionaryFileName);

                Console.WriteLine("The text was read from " + textFileName + ". Here is the translation:\r\n");
                Translator translator = new Translator();
                translator.SetDictionary(dictionary);
                foreach (string line in text)
                {
                    translator.SetText(line);
                    string translation = translator.TranslateWords();
                    Console.WriteLine(translation);
                    FileIO.WriteTranslation(translation, translationFileName);
                }
                Console.WriteLine("\r\nThe translation was also written to " + translationFileName);
            }
            catch (FileNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File not found: " + ex.Message);
                Console.ResetColor();
                return;
            }
            catch (WordNotFound ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ex.Message + ": " + ex.GetWord());
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }
}