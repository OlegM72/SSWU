using System;
using System.Collections.Generic;
using System.IO;

namespace Task_6_1
{
    public class Flat // data for a single flat
    {
        public int flatNumber;         // flat number
        public string owner;           // flat.owner name

        public Flat()
        {
            flatNumber = 0;
            owner = "";
        }
    }

    public class FlatData // accounting data for a single flat
    {
        public Flat flat;              // flat number and flat.owner
        public int prevReading;        // previous quarter electricity meter reading
        public int[] currReading;      // current quarter monthly meter readings
        public DateTime[] takingDates; // taking date for each month of the quarter
        public double totalDebt;       // calculated as total kilowatts spend in the quarter * price
        public TimeSpan periodTaken;   // difference between today and the last taking date

        public FlatData()
        {
            flat = new Flat();
        }

        public void Parse(string line) // trying to find all fields at the current string
        {
            try
            {
                string[] words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (words.Length < 9)
                    throw new FormatException("Wrong format of a flat record: less than 9 data values");
                if (!int.TryParse(words[0], out flat.flatNumber))
                    throw new FormatException("Wrong format of a flat number: " + words[0]);
                flat.owner = words[1]; // now we believe it is a correct name
                if (!int.TryParse(words[2], out prevReading))
                    throw new FormatException("Wrong format of a previous quarter reading: " + words[2]);
                currReading = new int[3];
                if (!int.TryParse(words[3], out currReading[0]))
                    throw new FormatException("Wrong format of the 1st reading: " + words[3]);
                if (!int.TryParse(words[5], out currReading[1]))
                    throw new FormatException("Wrong format of the 2nd reading: " + words[5]);
                if (!int.TryParse(words[5], out currReading[2]))
                    throw new FormatException("Wrong format of the 3rd reading: " + words[7]);
                takingDates = new DateTime[3];
                if (!DateTime.TryParse(words[4], out takingDates[0]))
                    throw new FormatException("Wrong format of the 1st reading date: " + words[4]);
                if (!DateTime.TryParse(words[6], out takingDates[1]))
                    throw new FormatException("Wrong format of the 1st reading date: " + words[6]);
                if (!DateTime.TryParse(words[8], out takingDates[2]))
                    throw new FormatException("Wrong format of the 1st reading date: " + words[8]);
            }
            catch
            {
                throw; // to the Accounting method
            }
        }
    }

    public class Accounting  // ведення обліку витрат електроенергії власниками квартир
    {
        string[,] months = new string[4, 3] {
            { "January", "February", "March" },
            { "April", "May", "June" },
            { "July", "August", "September" },
            { "October", "November", "December" } };

        int flatsCount; // number of flats in the report
        int quarter; // number of accounting quarter
        int maxownerLength = 5; // maximum length of a family name: minimum 5 for the column
        double price = 1.44; // grivnas for kilowatt
        DateTime lastDateTaken = DateTime.MinValue; // we will find the maximal of takingDates
        double maxDebt = 0; // we will find the maximal debt
        int maxDebtFlat = 0; // the number of flat with the maximal debt

        List<FlatData> data; // database

        public Accounting() // create an empty list
        {
            CreateEmptyData();
        }

        public void CreateEmptyData()
        {
            flatsCount = 0;
            data = new List<FlatData>();
            if (data == null)
            {
                throw new OutOfMemoryException("The accounting database could not be created");
            }
        }

        public Accounting(StreamReader reader) // reading from a text file
        {
            CreateEmptyData();
            try
            {
                bool firstLine = true;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.StartsWith('*')) // comments line
                        continue; // skip the line
                    if (firstLine) // read the data common for all flats
                    {
                        int spaceIdx = line.IndexOf(' ');
                        if (spaceIdx < 0) // no spaces in the string
                            throw new FormatException("Wrong format of the common line");
                        string str = line.Substring(0, spaceIdx);
                        if (!int.TryParse(str, out flatsCount))
                            throw new FormatException("Wrong format of the flats count");
                        str = line.Substring(spaceIdx + 1);
                        if (!int.TryParse(str, out quarter))
                            throw new FormatException("Wrong format of the quarter");
                        if (quarter < 1 || quarter > 4)
                            throw new FormatException("Quarter number can only from 1 to 4");
                        firstLine = false;
                    }
                    else // usual data line
                    {
                        FlatData currData = new FlatData(); // data for a single flat
                        currData.Parse(line); // if error then we have an Exception
                        if (currData.flat.owner.Length > maxownerLength)
                            maxownerLength = currData.flat.owner.Length;
                        if (currData.takingDates[2] > lastDateTaken)
                            lastDateTaken = currData.takingDates[2];
                        currData.totalDebt = (currData.currReading[2] - currData.prevReading) * price;
                        if (currData.totalDebt > maxDebt)
                        {
                            maxDebt = currData.totalDebt;
                            maxDebtFlat = currData.flat.flatNumber;
                        }
                        currData.periodTaken = DateTime.Now - currData.takingDates[2];
                        // everything is correct, adding a new flat to the collection
                        data.Add(currData);
                    }
                } // while not EOF
                if (data.Count != flatsCount)
                {
                    flatsCount = data.Count; // correcting and finishing
                    throw new ArgumentException("Flats count in the header does not correspond " +
                        "to the actual number of flats. Corrected");
                }
            } // try
            catch
            {
                data.Clear();
                flatsCount = 0;
                throw; // to the Main method
            }
        }

        private string RepeatChar(char c, int len)
        {
            // return "".PadLeft(len, c);
            string str = "";
            for (int i = 0; i < len; i++)
                str += c;
            return str;
        }

        public string PrintFlatByNumber(int flatNumber)
        {
            string error = "There is no flat with the number " + flatNumber;
            if (flatNumber > flatsCount)
                return error;
            bool found = false;
            int i = 0;
            while (i < flatsCount)
            {
                if (data[i].flat.flatNumber == flatNumber)
                {
                    found = true;
                    break;
                }
                i++;
            }
            if (found)
            {
                FlatData flat = data[i];
                return PrintHeader(1) + PrintFlat(flat) + PrintFooter();
            }
            else
                return error;
        }

        private string PrintHeader(int count) // count is the number of flats in the report
        {
            string horizont = RepeatChar('-', 91 + maxownerLength) + "\r\n";
            string ownerPadded = "flat.owner".PadRight(maxownerLength, ' ');
            return horizont + $"| Report: {count, 3} flats, quarter: {quarter}" + 
                   RepeatChar(' ', 59 + maxownerLength) + "|\r\n" + horizont +
                   $"|  #  | {ownerPadded} | Qua.{(quarter == 1 ? 4 : quarter - 1)} | {months[quarter - 1, 0],-16} " +
                   $"| {months[quarter - 1, 1],-16} | {months[quarter - 1, 2],-16} " +
                    "| Debt    | DAT |\r\n" +
                   horizont;
        }
        
        private string PrintFooter()
        {
            return RepeatChar('-', 91 + maxownerLength) + "\r\n" +
               "*DAT = Days after last date the readings were taken\r\n";
        }

        private string PrintSummary() // print totals after the footer, used only for all flats report
        {
            string result = $"The last date the readings were taken is: {lastDateTaken:dd MMMM}\r\n";
            if (maxDebt == 0)
                result += "Maximum debt was not yet calculated\r\n";
            else
                result += $"The maximum debt ({maxDebt:F2} UAH) was found in flat number {maxDebtFlat}\r\n";
            string flatsWithoutDebt = "";
            for (int i = 0; i < flatsCount; i++)
            {
                if (data[i].totalDebt < 1E-6) // count as zero
                {
                    flatsWithoutDebt += $"{data[i].flat.flatNumber} ";
                }
            }
            if (flatsWithoutDebt != "")
                result += "Flats with zero consumption are: " + flatsWithoutDebt;
            return result;
        }

        public string PrintFlat(FlatData flatdata)
        {
            string ownerPadded = flatdata.flat.owner.PadRight(maxownerLength, ' ');
            // string flat.ownerPadded = flat.flat.owner;
            // if (flat.ownerPadded.Length < maxflat.ownerLength) {
            //    flat.ownerPadded += RepeatChar(' ', maxflat.ownerLength - flat.ownerPadded.Length); }
            return $"| {flatdata.flat.flatNumber,3} | {ownerPadded} | {flatdata.prevReading,5} " +
                   $"| {flatdata.currReading[0],5} | {flatdata.takingDates[0]:dd.MM.yy} | {flatdata.currReading[1],5} " +
                   $"| {flatdata.takingDates[1]:dd.MM.yy} | {flatdata.currReading[2],5} | {flatdata.takingDates[2]:dd.MM.yy} " +
                   $"| {flatdata.totalDebt,7:F2} | {flatdata.periodTaken.Days,3} |\r\n";
        }

        public override string ToString()
        {
            if (flatsCount == 0)
                return ("Empty database, sorry");
            // form a report header
            string result = PrintHeader(flatsCount);
            // the lines for each flat
            foreach (FlatData curr in data)
                result += PrintFlat(curr);
            // and the footer
            return result + PrintFooter() + PrintSummary();
        }
    }
}


