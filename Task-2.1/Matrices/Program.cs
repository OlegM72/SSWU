using System;

namespace Matrices_2
{
    class Matrix
    {
        private int[,] matr;
        private readonly int rows, cols; // кількість строк та столбців

        public Matrix(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
            {
                throw new Exception("Розмірність матриці недопустима");
            }
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

        public void VerticalSnake() // Заповнення матриці у вигляді вертикальної змійки
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

        public void SpiralSnake()
        {
            int counter = 1; // поточне число для комірки
            int direct_x = 1, direct_y = 1; // напрямки (спочатку вниз та вправо)
            int x = 0, y = 0; // y - строка, x - стовбець
            Clear(); // заповнення нулями - робиться тому, що ми потім будемо йти тільки по нульовим
                     // значенням, та тому, що поточна матриця вже могла бути заповненою не дефолтними числами
                     // так, можливо, є більш простий варіант реалізації з номерами вже заповнених стовпцов
                     // та рядків, але це, на мою думку, теж заплутано. В Інтернеті не шукав рішення.
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

        public void DiagonalSnake()
        {
            if (rows != cols)
            {
                throw new Exception("Матриця не квадратна");
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
    } // class Matrix

    class Prog
    {
        static int Main(string[] args)
        {
            int size_x = 0;
            int size_y = 0;
            try
            {
                Console.WriteLine("Введіть ціле число - кількість строк матриці");
                size_y = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введіть ціле число - кількість столбців матриці");
                size_x = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine("Format exception: " + e.Message);
                return -1;
            }
            Matrix m = new Matrix(size_y, size_x);
            if (size_y != size_x)
            {
                Console.WriteLine("Матриця не квадратна. Деякі види заповнювання не будуть здійснюватись");
            }
            Console.WriteLine("Горизонтальна змійка:");
            m.HorisontalSnake();
            Console.WriteLine(m); // call ToString() method
            Console.WriteLine("Вертикальна змійка:");
            m.VerticalSnake();
            Console.WriteLine(m);
            Console.WriteLine("Спіральна змійка:");
            m.SpiralSnake();
            Console.WriteLine(m);
            if (size_y != size_x)
                return 0;
            Console.WriteLine("Діагональна змійка:");
            m.DiagonalSnake();
            Console.WriteLine(m);

            return 0;
        }
    }
}