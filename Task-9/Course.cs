using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Task_9
{
    internal class Course
    {
        public enum Currency
        {
            UAH = 0,
            USD = 1,
            EUR = 2
        }

        public static Currency currentCurrency = Currency.UAH;
        Dictionary<Currency, decimal> _courses; // Pairs of currency and price for 1 unit of money

        public Course()
        {
            _courses = new();
            _courses[Currency.UAH] = 1.0m; // UAH is the default currency. If redefined, error message will appear
        }

        public Course(string fileName) : this() // the Course() constructor will be called before this method
        {
            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException();
                string? line;
                using (StreamReader reader = new(fileName, Encoding.UTF8))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Length != 0 && line[0] == '*') // skip a comment
                            continue;
                        // Course.txt line is like this: EUR = 30.85
                        int idx;
                        if ((idx = line.IndexOf('=')) < 0)
                            continue; // error in the line, we just skip it
                        Currency currency = (Currency)Enum.Parse(typeof(Currency), line.Substring(0, idx - 1));
                        decimal course = decimal.Parse(line.Substring(idx + 2));
                        _courses.Add(currency, course); // only one line is allowed for each currency
                    }
                }
            }
            catch (Exception ex)
            {
                // any formatting or file error goes to Main method and terminates the program
                throw new Exception("Courses file: " + ex.Message);
            }
        }

        public override string ToString()
        {
            string result = "";
            foreach (KeyValuePair<Currency, decimal> currency in _courses)
            {
                result += $"1 {currency.Key} = {currency.Value} UAH\r\n";
            }
            return result;
        }

        public decimal GetCourse(Currency currency)
        {
            if (_courses == null || !_courses.ContainsKey(currency))
            {
                throw new ArgumentException("No course defined for the currency " + currency.ToString());
            }
            return _courses[currency];
        }

    }
}
