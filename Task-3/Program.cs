using System;

namespace Task_3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Vector vector = new Vector(20);
                Console.WriteLine("Random vector from 1 to 5:");
                vector.RandomInitialization(1, 5);
                Console.WriteLine(vector);
                Console.WriteLine("It is " + (vector.IsPalindrome()?"":"NOT ") + "a palindrome");

                int maxseqsize = 0; int maxseqnumber = 0;
                vector.GetLongestSeq(out maxseqsize, out maxseqnumber);
                Console.WriteLine("Longest sequence: " + maxseqsize + " repeats of " + maxseqnumber);

                Console.WriteLine("Frequences:");
                Pair[] pairs = vector.CalculateFreq();

                for (int i = 0; i < pairs.Length; i++)
                {
                    Console.Write(pairs[i] + "\n");
                }
                Console.WriteLine();

                Console.Write("A special vector: ");
                int[] specarray = new int[7] { 1, 2, 3, 4, 3, 2, 1 };
                vector = new Vector(specarray);
                Console.WriteLine(vector);
                Console.WriteLine("It is " + (vector.IsPalindrome() ? "" : "NOT ") + "a palindrome");

                Console.WriteLine($"Random vector from 1 to {vector.GetLength()} without repeats:");
                vector.InitShuffle();
                Console.WriteLine(vector);

                Console.WriteLine("Create a copy of the last vector:");
                Vector reversed = new Vector(vector);
                Console.WriteLine("My reverse:");
                reversed.MyReverse();
                Console.WriteLine(reversed);

                Console.WriteLine("Standard reverse of the original vector:");
                vector.StandardReverse();
                Console.WriteLine(vector);

                Console.WriteLine("Those vectors are " + (vector.IsEqual(reversed) ? "" : "NOT ") + "equal");
                Console.WriteLine();

                Matrix matr = new Matrix(9, 9);
                Console.WriteLine("Diagonal snake (down and right):");
                matr.DiagonalSnake(Matrix.Direction.Vniz_Vpravo);
                Console.WriteLine(matr);
                Console.WriteLine("Diagonal snake (right and down):");
                matr.DiagonalSnake(Matrix.Direction.Vpravo_Vniz);
                Console.WriteLine(matr);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
