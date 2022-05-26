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
                Console.WriteLine("It is " + (vector.isPalindrome()?"":"NOT ") + "a palindrome");
                Console.WriteLine("Longest sequence: " + vector.GetLongestSeq());

                Console.WriteLine("Frequences:");
                Pair[] pairs = vector.CalculateFreq();

                for (int i = 0; i < pairs.Length; i++)
                {
                    Console.Write(pairs[i] + "\n");
                }
                Console.WriteLine();

                Console.WriteLine("Random vector from 1 to 20 without repeats:");
                vector.InitShuffle();
                Console.WriteLine(vector);

                Console.WriteLine("My reverse:");
                Vector reversed = vector.MyReverse();
                Console.WriteLine(reversed);

                Console.WriteLine("Standard reverse of the original vector:");
                vector.StandardReverse();
                Console.WriteLine(vector);

                Matrix matr = new Matrix(9, 9);
                Console.WriteLine("\nДіагональна змійка (вниз та вправо):");
                matr.DiagonalSnake(Matrix.Direction.Vniz_Vpravo);
                matr.Show();
                Console.WriteLine("\nДіагональна змійка (вправо та вниз):");
                matr.DiagonalSnake(Matrix.Direction.Vpravo_Vniz);
                matr.Show();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
