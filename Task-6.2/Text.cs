using System;
using System.IO;
using System.Collections.Generic;

namespace Task_6_2
{
    public class Text
    {
        #region variables
        const string separators = ".!?";  // mark the end of a sentense (dot may be inside a word) 
        const string allowedChars = ".-"; // characters recorgnised as a part of the word. Examples: result.txt, так-от

        List<string> text;              // strings are read from file but not concatenated to a single string
        List<string> paragraphs = null; // text may contain several paragraphs divided by ".", "!", "?"
        List<string> shortestAndLongestWords = null;
        #endregion

        #region constructors
        
        public Text() // default is creating an empty text
        {
            text = new();
            paragraphs = null;
            shortestAndLongestWords = null;
        }

        public Text(StreamReader reader) : this() // reading text from the file
        {
            text = new();
            if (reader == null)
                throw new Exception("File not opened or unknown file error");
            try
            {
                while (!reader.EndOfStream)
                {
                    string read = reader.ReadLine(); // the next line; line ends are cut out
                    if (read.Length > 0)
                        text.Add(read);
                }
            }
            catch
            {
                throw; // to the Main method
            }
        }

        public Text(Text text) // create a copy of another Text
        {
            this.text = text.text; // :)
            paragraphs = text.paragraphs;
            shortestAndLongestWords = text.shortestAndLongestWords;
        }

        public Text(string line) // create a text with a single line
        {
            text = new();
            if (text != null)
                text.Add(line);
            paragraphs = null;
            shortestAndLongestWords = null;
        }
        #endregion

        #region methods

        public void RemoveGarbage() // correct unneeded spaces and change three dots to ellipsis in all the text
        {
            for (int i = 0; i < text.Count; i++)
                text[i] = RemoveGarbage(text[i]);
        }

        public string RemoveGarbage(string line) // correct unneeded spaces and change three dots to ellipsis
        {
            string[] words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string compressedLine = "";
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].EndsWith("...")) // correct "..." in the end of the word with ellipsis
                    words[i] = words[i][0..^3] +'…'; // means indices from beginning (0) to #3 from the end
                if (i < words.Length - 1)  // the last word without the space
                    words[i] += " ";
                compressedLine += words[i];
            }
            return compressedLine;
        }

        int FindASeparator(string line) // find the index of a first separator in the line
        {
            for (int i = 0; i < line.Length; i++)
                for (int j = 0; j < separators.Length; j++)
                    if (line[i] == separators[j])
                    {
                        // dot inside a word is not an end of a sentense. ! and ? inside a word is not allowed
                        if (line[i] != '.' || i + 1 >= line.Length || line[i + 1] == ' ')
                            return i; // return the index of the first occurence
                    }
            return -1; // if not found any!
        }

        public void SplitToParagraphs() // make a separate paragraph from each sentense
        {
            paragraphs = new();
            string currParagraph = "";
            string nextLineConsidered = ""; // the string where search for a separator
            if (text == null)
            {
                throw new NullReferenceException("The text was not initialized");
            }
            int lineNumber = 0;
            while (lineNumber < text.Count)
            {
                if (nextLineConsidered == "")
                    nextLineConsidered = text[lineNumber];
                int indexOfSeparator = FindASeparator(nextLineConsidered);
                if (indexOfSeparator < 0) // a separator not found, append the next line
                {
                    currParagraph += (nextLineConsidered + " ");
                    nextLineConsidered = "";
                    lineNumber++;
                }
                else
                {   // the part before the separator is added to the current paragraph
                    currParagraph += nextLineConsidered.Substring(0, indexOfSeparator + 1);
                    // add to the collection of paragraphs
                    paragraphs.Add(currParagraph);
                    // exclude the space after the separator if it is not the end of the line
                    if (indexOfSeparator != nextLineConsidered.Length)
                        indexOfSeparator++;
                    // the the part after the separator is the new line to consider
                    if (indexOfSeparator != nextLineConsidered.Length)
                        nextLineConsidered = nextLineConsidered.Substring(indexOfSeparator + 1);
                    else
                    { // the end of line: go to the next line
                        nextLineConsidered = "";
                        lineNumber++;
                    }
                    currParagraph = "";
                }
            }
        }
        public void FindShortLongWordsInParagraphs()
        {
            if (paragraphs == null)
            {
                throw new NullReferenceException("The text was not broken into paragraphs");
            }
            shortestAndLongestWords = new List<string>(paragraphs.Count);
            if (shortestAndLongestWords == null)
            {
                throw new NullReferenceException("Could not initialize the ShortestAndLongestWords array");
            }
            for (int currParagraph = 0; currParagraph < paragraphs.Count; currParagraph++)
            {
                // 1. Calculate the minimum and maximum word lengths
                int shortestWordLength = Int32.MaxValue;
                int longestWordLength = 0;
                string[] words = paragraphs[currParagraph].Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    // leave only letters in the words. If there is a "." or "-" INSIDE a word, count it as a word part
                    string word = "";
                    for (int j = 0; j < words[i].Length; j++)
                    {
                        bool specialChar = allowedChars.Contains(words[i][j]);
                        if (specialChar || Char.IsLetter(words[i][j]))
                        {
                            if (!specialChar || j != words[i].Length - 1) // special char in the end of the word is not added
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
                string currentShortestAndLongestWords = "";
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
                            currentShortestAndLongestWords += words[i] + " ";
                    }
                }
                // 3. Find all words of the maximum length in the current paragraph
                if (shortestWordLength != longestWordLength)
                    currentShortestAndLongestWords += "/ ";
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
                            currentShortestAndLongestWords += words[i] + " ";
                    }
                }
                shortestAndLongestWords.Add(currentShortestAndLongestWords);
            }
        }

        public string ShortLongWordsInParagraphs()
        {
            string result = "";
            if (shortestAndLongestWords == null)
            {
                throw new NullReferenceException("ShortestAndLongestWords array was not initialized");
            }
            for (int i = 0; i < shortestAndLongestWords.Count; i++)
                result += (shortestAndLongestWords[i] + "\r\n");
            return result;
        }

        public void WriteToFile(StreamWriter writer)
        {
            if (writer == null)
                throw new Exception("File not opened or unknown file error");
            try {
                // we can use ToString() method but it is not good, we avoid putting all the text to a single string
                // writer.Write(this);
                if (paragraphs != null)
                {
                    for (int i = 0; i < paragraphs.Count; i++)
                        writer.WriteLine(paragraphs[i]);
                }
                else
                {
                    if (text != null)
                    {
                        for (int i = 0; i < text.Count; i++)
                            writer.WriteLine(text[i]);
                    }
                    else
                        throw new NullReferenceException("The text is not initialized");
                }
            }
            catch {
                throw; // resolve in the Main method
            }
        }

        public override string ToString() // return paragraphs if they are defined or an original text if not.
                                          // Text is put out as a single string (but it is not a normal situation!)
        {
            string result = "";
            if (paragraphs != null)
            {
                for (int i = 0; i < paragraphs.Count; i++)
                    result += (paragraphs[i] + "\r\n");
            }
            else
            {
                if (text != null)
                {
                    for (int i = 0; i < text.Count; i++)
                        result += (text[i] + "\r\n");
                }
                else
                    throw new NullReferenceException("The text is not initialized");
            }
            return result;
        }
        #endregion

    }
}
