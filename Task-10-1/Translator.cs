using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    class Translator
    {
        private Dictionary<string, string> vocabulary;
        private string text;
        private string pathToText;
        private string pathToDictionary;
        private int dictCorrectionsCountAllowed = 3;

        public Translator() : this(Program.textFileName, Program.dictionaryFileName) { } // call the constructor below

        public Translator(string pathToText, string pathToDictionary) :
            this (new Dictionary<string, string>(), "", pathToText, pathToDictionary) { } // call the constructor below :)
        
        public Translator(Dictionary<string, string> vocabulary, string text, string pathToText, string pathToDictionary)
        {
            this.pathToText = pathToText;
            this.pathToDictionary = pathToDictionary;
            this.vocabulary = vocabulary;
            this.text = text;
        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public void SetDictionary(Dictionary<string, string> dictionary)
        {
            this.vocabulary = dictionary;
        }

        private void AddToDictionary(string word)
        {
            Console.WriteLine($"Введiть замiну для слова {word}");
            string value = Console.ReadLine();
            // the word is added if not empty and contains only letters
            if (value == null || value.Length == 0)
                return;
            for (int i = 0; i < value.Length; i++)
                if (!char.IsLetter(value[i]))
                    return;
            vocabulary.Add(word.ToLower(), value.ToLower()); // technically, all words should be lowered
            FileIO.WriteToDictionary(word, value, pathToDictionary); // but the file may contain any register
        }

        public string TranslateWords()
        {
            string result = "";
            var words = text.Split(' '); // split all the text by spaces
            foreach (string word in words)
            {
                if (word == "") // the case if there are several spaces, they should be added as well
                {
                    result += " ";
                    continue; 
                }
                string wordToTranslate = "";     // current word without punctuation marks which may be before and after the word
                string punctuationsBefore = "";  // the part of punctuations before the translated word
                string punctuationsAfter = "";   // the part of punctuations after the translated word
                bool notBeforeWord = false;      // the flag that shows if we already not before the translated word
                bool firstCaps = false;          // specifies if the first letter should be capital
                bool allCaps = false;            // specifies if all letters should be capital - if the first two are capitals
                int dictCorrectionsCount = 0;
                for (int i = 0; i < word.Length; i++)
                {
                    if (Char.IsLetter(word[i])) // found a normal letter (not punctuation or other symbol)
                    {
                        if (!notBeforeWord)
                        {
                            notBeforeWord = true;
                            firstCaps = Char.IsUpper(word[i]);
                        }
                        else
                        {
                            if (firstCaps && Char.IsUpper(word[i]))
                                allCaps = true;
                        }
                        wordToTranslate += word[i];
                    }
                    else
                    {
                        if (!notBeforeWord)
                            punctuationsBefore += word[i];
                        else
                            punctuationsAfter += word[i];
                    }
                }
                if (firstCaps && wordToTranslate.Length == 1)
                    allCaps = true; // special case of one letter words: allCaps was not set in this case
                if (wordToTranslate == "") // only punctuation marks
                    result += (punctuationsBefore + " ");
                else
                {
                    string lowerWord = wordToTranslate.ToLower();
                    while (!vocabulary.ContainsKey(lowerWord) && dictCorrectionsCount < dictCorrectionsCountAllowed)
                    {
                        AddToDictionary(wordToTranslate);
                        dictCorrectionsCount++;
                    }
                    if (!vocabulary.ContainsKey(lowerWord))
                        throw new WordNotFound(wordToTranslate);
                    string translatedWord;
                    try {
                        translatedWord = vocabulary[lowerWord];
                    }
                    catch {
                        throw; // print a error in the Main method
                    }
                    if (allCaps)
                        translatedWord = translatedWord.ToUpper();
                    else
                    {
                        if (firstCaps)
                            translatedWord = char.ToUpper(translatedWord[0]) + translatedWord[1..];
                    }
                    result += (punctuationsBefore + translatedWord + punctuationsAfter + " ");
                }
            }
            return result;
        }
    }
}
