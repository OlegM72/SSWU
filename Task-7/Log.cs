using System;
using System.Collections.Generic;
using System.IO;

namespace Task_7
{
    public class Log // error logging subsystem
    {
        public static StreamWriter errorLog;
        public static string errorLogName = "../../../ErrorLog.txt";

        private List<string> text = null;

        public Log(string fileName) // open fileName for writing
        {
            errorLogName = fileName;
            try
            {
                errorLog = new StreamWriter(errorLogName, true); // open for appending (if not exists, creating)
            }
            catch
            {
                throw; // catch it again in the Main routine
            }
            // finally { Close(); }
        }

        public void ReadLog()
        {
            if (text != null && text.Count > 0)
                text.Clear();
            else
                text = new List<string>();

            Close(); // close log temporarily
            StreamReader reader = new StreamReader(errorLogName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                text.Add(line);
            }
            reader.Close();
            errorLog = new StreamWriter(errorLogName, true); // open for appending again
        }

        public override string ToString()
        {
            string result = "";
            if (text == null || text.Count <= 0)
            {
                PutRecord("Log was not read, or no memory for the log text, or log reading error");
                return "";
            }
            for (int lineNo = 0; lineNo < text.Count; lineNo++)
            {
                result += $"{lineNo + 1}: {text[lineNo]}\r\n";
            }
            return result;
        }

        public bool CorrectLine(int lineNo, string change) // allow user to change the line #lineNo in the text list and append it to the log
        {
            if (text == null || lineNo <= 0 || text.Count <= lineNo - 1)
            {
                PutRecord($"There is no line # {lineNo} in the log");
                return false;
            }
            PutRecord($"Log line # {lineNo} change fulfilled. The old line was: " + text[lineNo - 1]);
            text[lineNo - 1] = change;
            PutRecord($"The new line is:" + change);
            return true;
        }

        public void Close() // close the underlying stream
        {
            errorLog.Close();
            text.Clear();
        }

        public static void PutRecord(string logString)
        {
            DateTime d = new DateTime();
            d = DateTime.Now;
            string line = $"{d:dd.MM.yyyy HH.mm.ss} " + logString;
            errorLog.WriteLine(line);
            // additionally show on the screen - can be changed
            Console.WriteLine(line);
        }
            

    }
}
