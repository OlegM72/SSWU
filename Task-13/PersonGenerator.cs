using System;
using System.Collections.Generic;

namespace Task13
{
    internal class PersonGenerator
    {
        Random random = new Random();

        public List<Person> Generate() // generate persons and add them to the persons read from persons.txt file
        {
            Reader reader = new Reader();
            List<Person> persons = new List<Person>();
            List<string> readPersons = reader.ReadExpresion(); // read the persons from the file, default file path is used

            foreach (var item in readPersons)
                persons.Add(PersonsParser.Parse(item));  // add the persons after parsing

            return persons;
        }

        int Get1000Randoms()
        {
            int res = 0;
            for (int i=0; i<1000; i++) res += random.Next(0, 2);
            return res;
        }

        public void WriteRandomGenerate(int UpRandomNumber) // write UpRandomNumber persons to default persons file
        {
            Writer writer = new Writer();
            for (int i = 0; i < UpRandomNumber; i++)
            {
                int age = random.Next(10, 90);
                PersonStatus status;
                if (age < 18) status = PersonStatus.Child;
                else if (age >= 65) status = PersonStatus.Pensioner;
                else status = random.Next(0, 4) < 3 ? PersonStatus.Ordinary : PersonStatus.Disabled; // let only 1 of 4 be disabled
                writer.WritePerson(new Person(
                    status,                                        // status
                    $"Client_{Guid.NewGuid().ToString()[33..]}",   // name
                    age,                                           // age
                    Math.Round(random.NextDouble(), 2),            // coordinate
                    random.Next(3, UpRandomNumber)));              // serviceTime
            }
        }
    }
}
