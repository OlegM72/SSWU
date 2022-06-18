using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    class FileIO
    {
        public static List<string> ReadText(string filePath)
        {
            List<string> result = new List<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                while(!reader.EndOfStream)
                    result.Add(reader.ReadLine());
            }
            return result;
        }

        public static Dictionary<string, string> ReadDictionary(string filePath)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Not found the dictionary: " + filePath);
            using(StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string temp = reader.ReadLine();
                    try
                    {
                        var str = temp.Split('-');
                        if (str.Length != 2)
                            throw new ArgumentException("Incorrect words pair in the vocabulary: " + temp);
                        result.Add(str[0].ToLower(), str[1].ToLower()); // all words in the vocabulary are lowercase
                    }
                    catch (ArgumentException)
                    {
                        throw;
                    }
                }
            }
            return result;
        }

        public static void WriteToDictionary(string key, string value, string filePath)
        {
            using(StreamWriter writer = new(filePath, true, Encoding.UTF8)) // append mode
            {
                writer.Write($"\n{key}-{value}");
            }
        }

        public static void WriteTranslation(string line, string filePath)
        {
            using (StreamWriter writer = new(filePath, true, Encoding.UTF8)) // append mode
            {
                writer.WriteLine(line);
            }
        }
    }
}
