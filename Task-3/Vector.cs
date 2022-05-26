using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    class Vector
    {
        int[] arr;

        public int GetLength()
        {
            return arr.Length;
        }

        public int this[int index]
        {
            get
            { 
                if(index >= 0 && index < GetLength())
                {
                    return arr[index];
                }
                else
                {
                    throw new Exception("Index out of the array's range");
                }
            }
            set 
            {
                arr[index] = value;
            }
        }

        public Vector(int[] arr)
        {
            this.arr = arr;
        }

        public Vector(int n)
        {
            arr = new int[n];
        }

        public Vector() { }

        public void RandomInitialization(int a, int b)
        {
            Random random = new Random();
            for (int i = 0; i < GetLength(); i++)
            {
                arr[i] = random.Next(a, b);
            }
        }

        public void InitShuffle()
        {
            //int index = Array.IndexOf(arr, 2);
            //Console.WriteLine(index);

            for (int i = 0; i < GetLength(); i++)
            {
                arr[i] = 0;
            }

            Random random = new Random();
            int x;
            for (int i = 0; i < GetLength(); i++)
            {
                while(arr[i] == 0)
                {
                    x = random.Next(1, GetLength() + 1);
                    if (Array.IndexOf(arr, x) < 0) // x not present
                    {
                        arr[i] = x;
                        break;
                    }
                }
            }
        }

        public string GetLongestSeq() // знаходить найдовшу підпослідовність однакових чисел.
        {
            Pair max = new Pair(0, 0);
            Pair curr = new Pair(0, 0);

            for (int i = 0; i < GetLength(); i++)
            {
                if (arr[i] == curr.Number)
                {
                    curr.Freq++;
                    if (max.Freq < curr.Freq)
                    {
                        max.Freq = curr.Freq;
                        max.Number = curr.Number;
                    }
                }
                else
                { 
                    if (curr.Number != 0)
                    {
                        if (max.Freq < curr.Freq)
                        {
                            max.Freq = curr.Freq;
                            max.Number = curr.Number;
                        }
                    }
                    curr.Number = arr[i];
                    curr.Freq = 1;
                }
            }
            return $"{max.Freq} repeats of {max.Number}";
        }

        public Pair[] CalculateFreq()
        {
            
            Pair[] pairs = new Pair[GetLength()];

            for (int i = 0; i < GetLength(); i++)
            {
                pairs[i] = new Pair(0,0);

            }
            int countDifference = 0;

            for (int i = 0; i < GetLength(); i++)
            {
                bool isElement = false;
                for (int j = 0; j < countDifference; j++)
                {
                    if(arr[i] == pairs[j].Number)
                    {
                        pairs[j].Freq++;
                        isElement = true;
                        break;
                    }
                }
                if (!isElement)
                {
                    pairs[countDifference].Freq++;
                    pairs[countDifference].Number = arr[i];
                    countDifference++;
                }
            }

            Pair[] result = new Pair[countDifference];
            for (int i = 0; i < countDifference; i++)
            {
                result[i] = pairs[i];
            }

            return result;
        }

        public Vector MyReverse()
        {
            Vector new_vector = new Vector(GetLength());
            for (int i = 0; i < GetLength(); i++)
            {
                new_vector[GetLength() - i - 1] = arr[i];
            }
            return new_vector;
        }

        public void StandardReverse()
        {
            Array.Reverse(arr);
        }

        public bool IsEqual(Vector vector1, Vector vector2)
        {
            if (vector1 == null || vector2 == null)
                return false;
            int len = vector1.GetLength();
            if (len != vector2.GetLength())
                return false;
            for (int i = 0; i < len; i++)
            {
                if (vector1[i] != vector2[i])
                    return false;
            }
            return true;
        }

        public bool isPalindrome()
        {
            return IsEqual(this.MyReverse(), this);
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < GetLength(); i++)
            {
                str += arr[i] + " ";
            }
            return str;
        }
    }

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

        public enum Direction
        {
            Vniz_Vpravo = 1,
            Vpravo_Vniz = -1
        }

        public void DiagonalSnake(Direction direction = Direction.Vniz_Vpravo)
        {
            if (rows != cols)
            {
                Console.WriteLine("Матриця не квадратна");
                return;
            }

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

        public void Show()
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
    }
}
