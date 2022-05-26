using System;

namespace Task_4
{
	internal class Program
    {	
		public static void Main()
        {
			try
			{
				Vector vector = new Vector(20);
				Console.WriteLine("Random vector from 1 to 10:");
				vector.RandomInitialization(1, 10);
				Console.WriteLine(vector);
				Console.WriteLine();

				Console.WriteLine("Quick sort: split by the middle element");
				vector.QuickSort(Vector.SortType.Middle);
				Console.WriteLine(vector);
				Console.WriteLine();

				Console.WriteLine("Random vector from 1 to 20 without repeats:");
				vector.InitShuffle();
				Console.WriteLine(vector);
				Console.WriteLine("Quick sort: split by the first element");
				vector.QuickSort(Vector.SortType.First);
				Console.WriteLine(vector);
				Console.WriteLine("");

				Console.WriteLine("Reverse of the last vector:");
				vector.StandardReverse();
				Console.WriteLine(vector);
				Console.WriteLine("Quick sort: split by the last element");
				vector.QuickSort(Vector.SortType.Last);
				Console.WriteLine(vector);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
