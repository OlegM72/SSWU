using System;
using System.Collections;
using System.Collections.Generic;

namespace Task_10_2
{
    class Matrix
    {
        private int[,] matr;
        private readonly int rows, cols;

        public IEnumerator GetEnumerator() // default enumerator: output the elements in vertical order (columns first)
        {
            for (int column = 0; column < cols; column++)
                for (int row = 0; row < rows; row++)
                    yield return matr[row, column];
        }

        public IEnumerable<int> HorizontalSnakeEnumerator() // output the elements in order of horizontal snake
        {
            for (int row = 0; row < rows; row++) // перебір строк
            {
                if (row % 2 == 0) // якщо поточна строка парна (перша є 0-ю - парна), йдемо вправо
                    for (int col = 0; col < cols; col++)
                        yield return matr[row, col];
                else // якщо поточна строка непарна, йдемо вліво
                    for (int col = cols - 1; col >= 0; col--)
                        yield return matr[row, col];
            }
        }

        public IEnumerable<int> DiagonalSnakeEnumerator() // output the elements in order of diagonal snake
        {
            if (rows != cols)
                throw new Exception("Matrix is not square");
            // напрямок: вправо - вниз
            for (int level = 0; level < cols; level++) // прохід до головної діагоналі (не можна йти паралельно нижню частину)
            {
                if (level % 2 != 0)
                    for (int i = 0; i <= level; i++) // донизу
                        yield return matr[i, level - i];
                else
                    for (int i = level; i >= 0; i--) // угору
                        yield return matr[i, level - i];
            }
            for (int level = cols; level <= (cols - 1) * 2; level++) // прохід після головної діагоналі
            {
                if (level % 2 != 0)
                    for (int i = level - cols + 1; i <= cols - 1; i++) // угору
                        yield return matr[i, level - i];
                else
                    for (int i = cols - 1; i >= level - cols + 1; i--) // донизу
                        yield return matr[i, level - i];
            }
        }

        public Matrix(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
                throw new Exception("Not allowed matrix dimensions");
            this.cols = cols;
            this.rows = rows;
            matr = new int[rows, cols];
        }

        public void HorisontalSnake() // Заповнення матриці у вигляді горизонтальної змійки
        {
            int curr_no = 1; // поточне значення комірки - починаємо з 1
            for (int i = 0; i < rows; i++) // перебір строк
            {
                if (i % 2 == 0) // якщо поточна строка парна (перша є 0-ю - парна), йдемо вправо
                    for (int j = 0; j < cols; j++)
                        matr[i, j] = curr_no++;
                else // якщо поточна строка непарна, йдемо вліво
                    for (int j = cols - 1; j >= 0; j--)
                        matr[i, j] = curr_no++;
            }
        }
        public enum Direction
        {
            Vniz_Vpravo = 1,
            Vpravo_Vniz = -1
        }

        public void DiagonalSnake(Direction direction = Direction.Vniz_Vpravo)
        {
            if (rows != cols)
                throw new Exception("Matrix is not square");

            if (direction != Direction.Vniz_Vpravo)
                direction = Direction.Vpravo_Vniz;
            int counter = 1; // поточне число для комірки
            for (int level = 0; level < cols; level++) // заповнення до головної діагоналі.
                                                       // заповнення після головної діагоналі здійснюється паралельно
            {
                if (((direction == Direction.Vniz_Vpravo) && (level % 2 == 0)) ||
                   ((direction == Direction.Vpravo_Vniz) && (level % 2 != 0)))
                {
                    for (int i = 0; i <= level; i++) // донизу
                    {
                        matr[i, level - i] = counter;
                        matr[cols - i - 1, cols - (level - i) - 1] = cols * cols - counter + 1;
                        counter++;
                    }
                }
                else
                {
                    for (int j = level; j >= 0; j--) // угору
                    {
                        matr[j, level - j] = counter;
                        matr[cols - j - 1, cols - (level - j) - 1] = cols * cols - counter + 1;
                        counter++;
                    }
                }
            }
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    result += $"{matr[i, j],4}";
                }
                result += "\r\n";
            }
            return result;
        }
    }
}
