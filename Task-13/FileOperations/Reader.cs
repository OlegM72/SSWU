using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task13
{
    internal class Reader : IExpresionReader
    {
        private string filePath;

        public string FilePath
        { 
            get => filePath;
            set
            {
                if (value != null) filePath = value;
            }
         }

        public Reader(string filePath)
        {
            this.filePath = filePath;
        }

        public Reader()
        {
            filePath = @"../../../Files/Persons.txt";
        }

        public List<string> ReadExpresion(string filePath = @"../../../Files/Persons.txt")
        {
            if (filePath == null || filePath == "" || Directory.Exists(filePath))
                throw new FileNotFoundException();
            if (!File.Exists(filePath)) 
                File.Create(filePath);

            List<string> result = new();
            using(StreamReader sr = new(filePath))
            {
                while (!sr.EndOfStream)
                {
                    result.Add(sr.ReadLine()??""); // if null, add ""
                }
                sr.Close();
            }

            return result;
        }
    }
}
