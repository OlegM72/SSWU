using System;

namespace Task_10_2
{
    internal class Program
    {
        static void Write(string text)
        {
            Console.Write(text);
        }

        static void WriteLine(string text)
        {
            Write(text + "\r\n");
        }
        static void WriteYellowLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteLine(text);
            Console.ResetColor();
        }

        static int Main(string[] args)
        {
            int size_x = 0;
            int size_y = 0;
            try
            {
                Write("Enter integer number of matrix rows: ");
                size_y = Convert.ToInt32(Console.ReadLine());
                Write("Enter integer number of matrix columns: ");
                size_x = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException e)
            {
                WriteYellowLine("Format exception: " + e.Message);
                return -1;
            }
            Matrix m = new Matrix(size_y, size_x);
            if (size_y != size_x)
            {
                WriteLine("Matrix is not square. Some fillings will not be done.");
            }
            WriteYellowLine("Horizontal snake:");
            m.HorisontalSnake();
            WriteLine(m.ToString());

            WriteYellowLine("Using default (order by columns) enumerator:");
            foreach (var item in m)
                Write($"{item} ");
            WriteLine("");

            if (size_y != size_x)
                return 0;

            WriteYellowLine("\r\nDiagonal snake (down and right):");
            m.DiagonalSnake(Matrix.Direction.Vniz_Vpravo);
            WriteLine(m.ToString());

            WriteYellowLine("Diagonal snake(right and down):");
            m.DiagonalSnake(Matrix.Direction.Vpravo_Vniz);
            WriteLine(m.ToString());

            WriteYellowLine("Using default (order by columns) enumerator:");
            foreach (var item in m)
                Write($"{item} ");
            WriteLine(""); WriteLine("");

            WriteYellowLine("Using horizontal snake enumerator:");
            foreach (int item in m.HorizontalSnakeEnumerator())
                Write($"{item} ");
            WriteLine(""); WriteLine("");

            WriteYellowLine("Using diagonal snake enumerator:");
            foreach (int item in m.DiagonalSnakeEnumerator())
                Write($"{item} ");

            return 0;
        }
    }
}
