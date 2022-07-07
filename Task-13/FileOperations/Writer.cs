using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task13
{
    internal class Writer
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

        public Writer(string filePath)
        {
            this.filePath = filePath;
        }

        public Writer()
        {
            filePath = @"../../../Files/Persons.txt";
        }

        public void WritePerson(Person person, string filePath = @"../../../Files/Persons.txt")
        {
            if (filePath == null || filePath == "" || Directory.Exists(filePath))
                throw new FileNotFoundException();
            if (!File.Exists(filePath))
                File.Create(filePath);

            using (StreamWriter sw = new(filePath, true)) // append mode
            {
                sw.WriteLine(person.ToString());
                sw.Close();
            }
        }
    }

    public class ResultWriter : IResultWriter
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

        public ResultWriter(string filePath)
        {
            this.filePath = filePath;
        }

        public ResultWriter()
        {
            filePath = @"../../../Files/Result.txt";
        }

        public void WritePerson(List<string> resultStrings, string filePath = @"../../../Files/Result.txt")
        {
            if (filePath == null || filePath == "" || Directory.Exists(filePath))
                throw new FileNotFoundException();
            if (!File.Exists(filePath))
                File.Create(filePath);

            using (StreamWriter sw = new(filePath, true)) // append mode
            {
                foreach(string line in resultStrings)
                    sw.WriteLine(line);
                sw.Close();
            }
        }
    }
}
