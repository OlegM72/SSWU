using System;
using System.Collections.Generic;
using System.IO;

namespace Task_8_2
{
    public struct IPLogRecord
    {
        public string IP;              // IP-address
        public int[] hours;            // numbers of attending at each hour of the day
        public int[] daysOfWeek; // numbers of attending at each day of the week
    }

    internal class IPLogRecordComparator : IComparer<IPLogRecord>
    {
        public int Compare(IPLogRecord rec1, IPLogRecord rec2)
        {
            return rec1.IP.CompareTo(rec2.IP); // compare by IP-addresses (strings). Exact sorting by numbers not needed
        }
    }
    
    public class IPCollection
    {
        SortedSet<IPLogRecord> list = null;

        public IPCollection() // create an empty collection
        {
            IPLogRecordComparator comparator = new();
            list = new SortedSet<IPLogRecord>(comparator);
        }

        public IPCollection(StreamReader reader) // reading the collection from the file, all IPs will be unique
        {
            IPLogRecordComparator comparator = new();
            list = new SortedSet<IPLogRecord>(comparator);
            if (reader == null)
                throw new Exception("File not opened or unknown file error");
            try
            {
                while (!reader.EndOfStream)
                {
                    string read = reader.ReadLine(); // the next line
                    if (read[0] == '*') continue; // skip a comment
                    IPLogRecord rec = new IPLogRecord();
                    int spaceIdx1 = read.IndexOf(' ', 0);
                    if (spaceIdx1 == -1)
                        throw new Exception("Wrong format of an input string (IP)");
                    rec.IP = read.Substring(0, spaceIdx1);
                    int spaceIdx2 = read.IndexOf(' ', spaceIdx1 + 1);
                    DateTime d;
                    if (spaceIdx2 == -1 || !DateTime.TryParse(read.Substring(spaceIdx1 + 1, spaceIdx2 - spaceIdx1 - 1), 
                                                              out d))
                        throw new Exception("Wrong format of an input string (Time)");
                    if (!DayOfWeek.TryParse(char.ToUpper(read[spaceIdx2 + 1]) + read.Substring(spaceIdx2 + 2),
                                            out DayOfWeek dayOfWeek))
                        throw new Exception("Wrong format of an input string (DayOfWeek)");
                    if (list.TryGetValue(rec, out IPLogRecord existed)) // if IP exists in the list
                    {
                        existed.hours[d.Hour]++;
                        existed.daysOfWeek[(int)dayOfWeek]++;
                    }
                    else // new record
                    {
                        rec.hours = new int[24];
                        rec.hours[d.Hour]++;
                        rec.daysOfWeek = new int[7];
                        rec.daysOfWeek[(int)dayOfWeek]++;
                        list.Add(rec);
                    }
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

        public void WriteToFile(StreamWriter writer)
        {
            if (list == null)
                throw new NullReferenceException("List is not initialized");
            if (writer == null)
                throw new Exception("File not opened or unknown file error");
            try
            {
                writer.Write(list); // use ToString method
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

        public override string ToString() // output full list with unique IPs and all week summary statistics 
        {
            string result = "";
            if (list == null)
            {
                throw new NullReferenceException("The list is not initialized");
            }
            foreach (IPLogRecord rec in list)
            {
                result += $"{rec.IP,-15}: hours: ";
                for (int i = 0; i < 24; i++)
                {
                    if (rec.hours[i] != 0)
                        result += $"{i} ({rec.hours[i]}x), ";
                }
                result += "days of week: ";
                for (int i = 0; i < 7; i++)
                {
                    if (rec.daysOfWeek[i] != 0)
                        result += $"{(DayOfWeek)i} ({rec.daysOfWeek[i]}x) ";
                }
                result += "\r\n";
            }
            return result;
        }

        public string PrintIPsSummary() // output list of IPs with their attendings count,
                                        // most popular day of week and hour of day
        {
            string result = "";
            int totalCount = 0;
            int[] totalHour = new int[24];
            int maxTotalHour = 0;
            int[] totalDOW = new int[7];
            int maxTotalDOW = 0;
            if (list == null)
                throw new NullReferenceException("List is not initialized");
            foreach (IPLogRecord rec in list)
            {
                result += $"{rec.IP,-15}: ";
                int count = 0; int maxHour = 0;
                for (int i = 0; i < 24; i++)
                {
                    if (rec.hours[i] != 0)
                    {
                        count += rec.hours[i];
                        if (maxHour < rec.hours[i])
                            maxHour = rec.hours[i];
                        totalHour[i] += rec.hours[i];
                        if (maxTotalHour < totalHour[i])
                            maxTotalHour = totalHour[i];
                    }
                }
                totalCount += count;
                result += $"count: {count,3}, most popular hour(s) of day: ";
                for (int i = 0; i < 24; i++)
                {
                    if (rec.hours[i] == maxHour)
                    {
                        result += $"{i}, ";
                    }
                }
                int maxDOW = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (rec.daysOfWeek[i] != 0)
                    {
                        if (maxDOW < rec.daysOfWeek[i])
                            maxDOW = rec.daysOfWeek[i];
                        totalDOW[i] += rec.daysOfWeek[i];
                        if (maxTotalDOW < totalDOW[i])
                            maxTotalDOW = totalDOW[i];
                    }
                }
                result += "most popular day(s) of week: ";
                for (int i = 0; i < 7; i++)
                {
                    if (rec.daysOfWeek[i] == maxDOW)
                        result += $"{(DayOfWeek)i} ";
                }
                result += "\r\n";
            }
            result += $"Total Summary: {list.Count} IPs in the list, total {totalCount} attendings, most popular hour(s) of day: ";
            for (int i = 0; i < 24; i++)
            {
                if (totalHour[i] == maxTotalHour)
                {
                    result += $"{i}, ";
                }
            }
            result += "most popular day(s) of week: ";
            for (int i = 0; i < 7; i++)
            {
                if (totalDOW[i] == maxTotalDOW)
                    result += $"{(DayOfWeek)i} ";
            }
            result += $"\r\n";
            return result;
        }
    }
}
