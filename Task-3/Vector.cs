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
                if (arr != null && index >= 0 && index < GetLength())
                {
                    return arr[index];
                }
                else
                {
                    throw new Exception($"Index {index} is out of the array's range");
                }
            }
            set
            {
                if (arr != null && index >= 0 && index < GetLength())
                {
                    arr[index] = value;
                }
                else
                {
                    throw new Exception($"Index {index} is out of the array's range");
                }
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

        public Vector() // Create a vector of a random size 1..100 :)
        {
            Random random = new Random();
            arr = new int[random.Next(1, 100)];
        }

        public Vector(Vector source) // makes a copy of the vector
        {
            this.arr = new int[source.GetLength()];
            Array.Copy(source.arr, this.arr, source.GetLength());
        }

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

        public void GetLongestSeq(out int freq, out int number)
            // знаходить найдовшу підпослідовність однакових чисел
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
            freq = max.Freq;
            number = max.Number;
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

        public void MyReverse()
        {
            /* варіант з виділенням пам'яті, оставляє старий масив в пам'яті, поки його CLR не очистить:
               int[] reversed = new int[GetLength()];
               for (int i = 0; i < reversed.Length; i++)
               {
                   reversed[reversed.Length - i - 1] = arr[i];
               }
               arr = reversed;
            */
            // варіант без виділення пам'яті, більш повільний:
            int len = GetLength(); int middle = len / 2;
            len--; // для пришвидшення операцій, щоб не писати (len - 1) - i
            for (int i = 0; i < middle; i++)
            {
                int temp = arr[i];
                arr[i] = arr[len - i];
                arr[len - i] = temp;
            }
        }

        public void StandardReverse()
        {
            Array.Reverse(arr);
        }

        public bool IsEqual(Vector compared)
        {
            if (compared == null) // this != null - it's guaranteed :)
                return false;
            int len = GetLength();
            if (len != compared.GetLength())
                return false;
            for (int i = 0; i < len; i++)
            {
                if (arr[i] != compared[i])
                    return false;
            }
            return true;
        }

        public bool IsPalindrome()
        {
            // a simple method: return IsEqual(this.MyReverse(), this);
            int len = GetLength(); // to speedup the calcultaions
            int middle = len / 2;
            len--;  // to speedup the calcultaions :)
            for (int i = 0; i < middle; i++)
            {
                if (arr[i] != arr[len - i])
                    return false;
            }
            return true;
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

        public Matrix(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
            {
                throw new Exception("Matrix dimensions are not allowed");
            }
            this.cols = cols;
            this.rows = rows;
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
                throw new Exception("Matrix is not square");
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
