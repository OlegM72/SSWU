using System;
using System.IO;
using System.Diagnostics;

namespace Task_5
{
    internal class Program
    {
		static string ReadFileName = "../../../Test-5.txt";
		static string Part1FileName = "../../../SortPart1.txt";
		static string Part2FileName = "../../../SortPart2.txt";
		static string SortedFileName = "../../../SortedResult.txt";
		static string SortedFileName2 = "../../../SortedResult2.txt";

		static public TimeSpan MagicMergeSort() // returns time passed
		// NEW: reading in parts, saving parts, sorting parts and merging to file at once
		{
			try
			{
				// Count the number of lines (numbers) in the file
				int streamSize = (File.ReadAllLines(ReadFileName)).Length;

				// We can work only with half-sized arrays
				int half1Size = streamSize - (streamSize / 2);

				Vector vector1 = new Vector();
				StreamReader reader = new StreamReader(ReadFileName);
				Stopwatch timewatch = new Stopwatch(); // for time counting
				vector1.ReadPart(reader, half1Size); // reading the first half of the vector
				if (vector1.GetLength() == 0) // reading error
					return TimeSpan.MaxValue;
				vector1.SplitMergeSort(); // sorting it in the memory
				vector1.SaveToFile(Part1FileName); // and saving
				
				Vector vector2 = new Vector();
				vector2.ReadPart(reader, streamSize - half1Size); // reading the second half to the memory
				vector2.SplitMergeSort(); // sorting it in the memory
				vector2.MergeSortedFileWithMemory(Part1FileName, SortedFileName2); // merging vector1 file and vector2
				timewatch.Stop();
				return timewatch.Elapsed;
			}
			catch
            {
				throw; // to the Main method
            }
		}

		static void Main(string[] args)
        {
			try
			{
				// Method of reading from file and full sorting
				Vector vector = new Vector(ReadFileName); // reading from the file "FileName"
				if (vector.GetLength() == 0) // reading error
                {
					return;
                }
				Console.WriteLine("The vector fully read from "+ ReadFileName+" (size = "+vector.GetLength()+ "):");
				Console.WriteLine(vector);
				Console.WriteLine();
				Console.WriteLine("Merge Sort:");

				Stopwatch timewatch = new Stopwatch(); // for time counting
				timewatch.Start();
				vector.SplitMergeSort();
				timewatch.Stop();
				TimeSpan mergetime = timewatch.Elapsed;
				Console.WriteLine(vector);
				Console.WriteLine("Time passed: {0} seconds", mergetime.Seconds + mergetime.Milliseconds / 1000m);
				Console.WriteLine();

				// OLD Method of sorting with files (partial read - partial saving - partial sorting - merge)
				vector = new Vector(ReadFileName, Part1FileName, Part2FileName); // reading and saving partial files
				if (vector.GetLength() == 0) // reading error
					return;
				Console.WriteLine("A vector was read from " + ReadFileName + 
					" and saved to partial files " + Part1FileName + " and " + Part2FileName);
				Console.WriteLine("Sorting the partial files and merging them to " + SortedFileName);
				timewatch.Restart();
				vector.SplitMergeSortFiles(Part1FileName, Part2FileName, SortedFileName);
				timewatch.Stop();
				mergetime = timewatch.Elapsed;
				Console.WriteLine("Time passed: {0} seconds", mergetime.Seconds + mergetime.Milliseconds / 1000m);
				Console.WriteLine();


				// ✅ NEW ❗❗ Method of sorting with merging file and memory parts
				mergetime = Program.MagicMergeSort(); // see above
				if (mergetime == TimeSpan.MaxValue) // error when reading
                {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Error reading from " + ReadFileName + ", skipping the procedure");
					Console.ResetColor();
				}
				else
                {
					Console.WriteLine("The first part of a vector was read from " + ReadFileName +
					", sorted and saved to " + Part1FileName);
					Console.WriteLine("The second part of the vector was read from " + ReadFileName + " to the memory");
					Console.WriteLine("Sorting the first half file with the second half in the memory and merging them to "
						+ SortedFileName2);
					Console.WriteLine("Time passed: {0} seconds", mergetime.Seconds + mergetime.Milliseconds / 1000m);
					Console.WriteLine();
				}


				// Merge Sorting usual random vector
				vector = new Vector(35);
				Console.WriteLine("Random vector from 1 to {0} without repeats:", vector.GetLength());
				vector.InitShuffle();
				Console.WriteLine(vector);
				Console.WriteLine();
				Console.WriteLine("Merge Sort:");
				timewatch.Restart();
				vector.SplitMergeSort();
				timewatch.Stop();
				mergetime = timewatch.Elapsed;
				Console.WriteLine(vector);
				Console.WriteLine("Time passed: {0} seconds", mergetime.Seconds + mergetime.Milliseconds / 1000m);
				Console.WriteLine();

				// Heap Sorting a shuffled random vector
				Console.WriteLine("Another random vector from 1 to {0} without repeats:", vector.GetLength());
				vector.InitShuffle();
				Console.WriteLine(vector);
				Console.WriteLine();
				Console.WriteLine("Heap Sort:");
				timewatch.Restart();
				vector.HeapSort();
				timewatch.Stop();
				mergetime = timewatch.Elapsed;
				Console.WriteLine(vector);
				Console.WriteLine("Time passed: {0} seconds", mergetime.Seconds + mergetime.Milliseconds / 1000m);
				Console.WriteLine();

				int[] sizes = { 1000, 10000, 100000, 1000000, 10000000 };
				Vector largeVector;
				Console.WriteLine("Testing HeapSort times for random vectors of sizes from 1,000 to 10,000,000:");
				foreach (int size in sizes)
                {
					largeVector = new Vector(size);
					largeVector.RandomInitialization(0, size-1);
					timewatch.Restart();
					largeVector.HeapSort();
					timewatch.Stop();
					mergetime = timewatch.Elapsed;
					Console.WriteLine("Size {0,8}: {1} seconds", size, mergetime.Seconds + mergetime.Milliseconds / 1000m);
				}
				Console.WriteLine();

				Console.WriteLine("Testing MergeSort times for random vectors of sizes from 1,000 to 10,000,000:");
				foreach (int size in sizes)
				{
					largeVector = new Vector(size);
					largeVector.RandomInitialization(0, size - 1);
					timewatch.Restart();
					largeVector.SplitMergeSort();
					timewatch.Stop();
					mergetime = timewatch.Elapsed;
					Console.WriteLine("Size {0,8}: {1} seconds", size, mergetime.Seconds + mergetime.Milliseconds / 1000m);
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ResetColor();
			}
		}
    }
}
