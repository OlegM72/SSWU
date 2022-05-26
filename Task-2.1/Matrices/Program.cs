using System;

namespace Matrices_2
{
    class Matrix
    {
        private int[,] matr;
        private readonly int rows, cols; // кількість строк та столбців

        public Matrix(int Rows, int Cols)
        {
            if (Rows <= 0 || Cols <= 0)
            {
                Console.WriteLine("Розмірність матриці недопустима");
            }
            cols = Cols;
            rows = Rows;
            matr = new int[rows, cols];
        }

        private void HorisontalSnake() // Заповнення матриці у вигляді горизонтальної змійки
        {
            int curr_no = 1; // поточне значення комірки - починаємо з 1
            for (int i = 0; i < rows; i++) // перебір строк
            {
                if (i % 2 == 0) // якщо поточна строка парна (перша є 0-ю - парна), йдемо вправо
                {
                    for (int j = 0; j < cols; j++)
                        matr[i, j] = curr_no++;
                }
                else // якщо поточна строка непарна, йдемо вліво
                {
                    for (int j = cols - 1; j >= 0; j--)
                        matr[i, j] = curr_no++;
                }
            }
        }

        private void VerticalSnake() // Заповнення матриці у вигляді вертикальної змійки
        {
            int curr_no = 1; // поточне значення комірки - починаємо з 1
            for (int i = 0; i < cols; i++) // перебір стовбців
            {
                if (i % 2 == 0) // якщо поточний стовбець парний (перший є 0-м - парний), йдемо донизу
                {
                    for (int j = 0; j < rows; j++)
                        matr[j, i] = curr_no++;
                }
                else // якщо поточний стовбець непарний, йдемо угору
                {
                    for (int j = rows - 1; j >= 0; j--)
                        matr[j, i] = curr_no++;
                }
            }
        }

        private void SpiralSnake()
        {
            int counter = 1; // поточне число для комірки
            int direct_x = 1, direct_y = 1; // напрямки (спочатку вниз та вправо)
            int x = 0, y = 0; // y - строка, x - стовбець
            Clear(); // заповнення нулями
            do
            {
                while ((y >= 0) && (y < rows) && (matr[y, x] == 0))
                {
                    matr[y, x] = counter++;
                    y += direct_y;
                }
                y -= direct_y; // повернутись на шаг назад
                direct_y = -direct_y;
                x += direct_x; // перехід до наступного стовбця

                while ((x >= 0) && (x < cols) && (matr[y, x] == 0))
                {
                    matr[y, x] = counter++;
                    x += direct_x;
                }
                x -= direct_x; // повернутись на шаг назад
                direct_x = -direct_x;
                y += direct_y; // перехід до наступної колонки

            } while (counter <= cols * rows);
        }

        private void DiagonalSnake()
        {
            if (rows != cols)
            {
                Console.WriteLine("Матриця не квадратна");
                return;
            }

            int counter = 1; // поточне число для комірки
            for (int level = 0; level < cols; level++) // заповнення до головної діагоналі.
                                                       // заповнення після головної діагоналі здійснюється паралельно
            {
                if (level % 2 == 0) // level - номер поточної діагоналі (0...cols-1)
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

        private void Show()
        {
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    Console.Write($"{matr[i, j],4}");
                }
                Console.WriteLine();
            }
        }

        private void Clear()
        {
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    matr[i, j] = 0;
                }
            }
        }
        public void FillAndShow()
        {
            if (rows != cols)
            {
                Console.WriteLine("Матриця не квадратна. Деякі види заповнювання не будуть здійснюватись");
            }
            Console.WriteLine("Горизонтальна змійка:");
            HorisontalSnake();
            Show();
            Console.WriteLine();
            Console.WriteLine("Вертикальна змійка:");
            VerticalSnake();
            Show();
            Console.WriteLine();
            Console.WriteLine("Спіральна змійка:");
            SpiralSnake();
            Show();
            if (rows != cols) return;
            Console.WriteLine();
            Console.WriteLine("Діагональна змійка:");
            DiagonalSnake();
            Show();
        }
    }

    class Prog
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введіть ціле число - кількість строк матриці");
            int size_y = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введіть ціле число - кількість столбців матриці");
            int size_x = Convert.ToInt32(Console.ReadLine());
            Matrix m = new Matrix(size_y, size_x);
            m.FillAndShow();
        }
    }
}