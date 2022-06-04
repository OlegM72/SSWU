using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Task_6_2
{
    public class Text
    {
        #region variables
        string text;
        string[] paragraphs = null;
        string[] shortestAndLongestWords = null;
        #endregion

        #region constructors
        public Text(string source)
        {
            text = source;
        }
        public Text() // default is creating an empty text
        {
            text = "";
        }

        public Text(StreamReader reader) // reading text from the file
        {
            text = "";
            if (reader == null)
                throw new Exception("File not opened or unknown file error");
            try
            {
                while (!reader.EndOfStream)
                {
                    string read = reader.ReadLine(); // the next line; line ends are cut out
                    if (read.Length > 0 && !reader.EndOfStream)
                    {
                        read += ' '; // always add a space between lines except for the end of file
                    }
                    text += read;
                }
            }
            catch (Exception ex) when (ex.Message != null)
            {
                Console.WriteLine(ex.Message); // "registering a message in journal"
            }
            finally
            {
                reader.Close();
            }
        }

        public Text(Text text) // create a copy of another Text
        {
            this.text = text.text; // :)
        }

        #endregion

        #region methods

        public void RemoveGarbage() // correct unneeded spaces and three dots
        {
            string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string compressedText = "";
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].EndsWith("...")) // correct "..." in the end of the word with ellipsis
                    words[i] = words[i][0..^3] +'…'; // means indices from beginning (0) to #3 from the end
                if (i < words.Length - 1)  // the last word without the space
                    words[i] += " ";
                compressedText += words[i];
            }
            text = compressedText;
        }

        public void SplitToParagraphs() // make a separate paragraph from each sentense
        {
            paragraphs = text.Split('.');
            if (paragraphs == null)
            {
                throw new NullReferenceException("The text was not broken into paragraphs");
            }
            text = "";
            for (int i = 0; i < paragraphs.Length; i++)
            {
                if (paragraphs[i] != "" && paragraphs[i][0] == ' ')
                {   // replace space with line end in the beginning of each paragraph
                    paragraphs[i] = "\r\n" + paragraphs[i].Substring(1);
                }
                // if a dot was not followed by space then it is not a new paragraph, just concatenate
                text += ( (i == 0?"":".") + paragraphs[i] ); // dot was deleted by Split method
            }
        }
        public void FindShortLongWordsInParagraphs()
        {
            if (paragraphs == null)
            {
                throw new NullReferenceException("The text was not broken into paragraphs");
            }
            shortestAndLongestWords = new string[paragraphs.Length];
            if (shortestAndLongestWords == null)
            {
                throw new NullReferenceException("Could not initialize the ShortestAndLongestWords array");
            }
            for (int currParagraph = 0; currParagraph < paragraphs.Length; currParagraph++)
            {
                // 1. Calculate the minimum and maximum word lengths
                int shortestWordLength = Int32.MaxValue;
                int longestWordLength = 0;
                string[] words = paragraphs[currParagraph].Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    // leave only letters in the words
                    string word = "";
                    for (int j = 0; j < words[i].Length; j++)
                    {
                        if (Char.IsLetter(words[i][j]))
                        {
                            word += words[i][j];
                        }
                    }
                    words[i] = word; // save to words array
                    if (word.Length < shortestWordLength)
                        shortestWordLength = word.Length;
                    if (word.Length > longestWordLength)
                        longestWordLength = word.Length;
                }
                // 2. Find all words of the minimum length in the current paragraph
                shortestAndLongestWords[currParagraph] = "";
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Length == shortestWordLength)
                    {
                        // check if the word was already found, add it to the list only once
                        bool exists = false;
                        for (int j = 0; j < i - 1; j++)
                        {
                            if (words[j] == words[i])
                            {
                                exists = true;
                                break;
                            }
                        }
                        if (!exists)
                            shortestAndLongestWords[currParagraph] += words[i] + " ";
                    }
                        
                }
                // 3. Find all words of the maximum length in the current paragraph
                if (shortestWordLength != longestWordLength)
                    shortestAndLongestWords[currParagraph] += "/ ";
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Length == longestWordLength)
                    {
                        // check if the word was already found, add it to the list only once
                        bool exists = false;
                        for (int j = 0; j < i - 1; j++)
                        {
                            if (words[j] == words[i])
                            {
                                exists = true;
                                break;
                            }
                        }
                        if (!exists)
                            shortestAndLongestWords[currParagraph] += words[i] + " ";
                    }
                }
                // the result is in shortestAndLongestWords variable
            }
        }

        public void PrintShortLongWordsInParagraphs()
        {
            if (shortestAndLongestWords == null)
            {
                throw new NullReferenceException("ShortestAndLongestWords array was not initialized");
            }
            for (int i = 0; i < shortestAndLongestWords.Length; i++)
                Console.WriteLine(shortestAndLongestWords[i]);
        }


        public void WriteToFile(StreamWriter writer)
        {
            if (text == null)
                throw new NullReferenceException("Text is not initialized");
            if (writer == null)
                throw new Exception("File not opened or unknown file error");
            try
            {
                writer.Write(text);
            }
            catch (Exception ex) when (ex.Message != null)
            {
                Console.WriteLine(ex.Message); // "registering a message in journal"
            }
            finally
            {
                writer.Close();
            }
        }

        public override string ToString()
        {
            if (text == null)
            {
                throw new NullReferenceException("The text is not initialized");
            }
            return text;
        }
        #endregion

    }
}
