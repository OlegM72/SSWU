using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4
{
    class Vector
    {
        int[] arr;

        public enum SortType
        {
            Middle,
            First,
            Last
        }

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

        public Vector(Vector source) // makes a copy of the vector
        {
            this.arr = new int[source.GetLength()];
            Array.Copy(source.arr, this.arr, source.GetLength());
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
            if (arr == null)
                return; // some error;

            Pair[] indices = new Pair[GetLength()]; // array of consequitive numbers in pairs with random ¨indices"
            Random random = new Random();
            
            // generate pairs with large "indices"
            for (int i = 0; i < GetLength(); i++)
            {
                indices[i] = new Pair(i + 1, (int)(random.NextDouble()*Int32.MaxValue)); // NextDouble gives numbers 0..1
            }

            // pairs are now compared by freqs using IComparable interface so we can sort them by Freqs
            Array.Sort(indices);

            // now the indices array contains numbers in random order, save them to Vector's array
            for (int i = 0; i < GetLength(); i++)
            {
                arr[i] = indices[i].Number;
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

        private int Partition(int left, int right, SortType sortType) // returns the index of the pivot (base element)
        {
            switch (sortType)
            {
                case SortType.First:
                    return left;
                case SortType.Last:
                    return right;
                case SortType.Middle:
                    return (left + right) / 2 + 1; // + 1 because in case (0, 1) we need middle = 1
                default:
                    return 0;
            }
        }

        private void QuickSortPart(int left, int right, SortType sortType)
            // sorting the array part from "left" to "right"
        {
            if (left >= right)
                return; // recursion stop condition
            int temp;
            int i = left;
            int j = right;
            int pivot = arr[Partition(left, right, sortType)];

            while (i <= j)
            {
                while (arr[i] < pivot) ++i; // i = the first element larger or equal to pivot
                while (arr[j] > pivot) --j; // j = the last element smaller or equal to pivot

                if (i <= j)
                {   // swap i-th and j-th elements
                    temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    ++i;
                    --j;
                }
            }
            QuickSortPart(left, j, sortType); // if (j > left) -- sort the left part
            QuickSortPart(i, right, sortType); // if (i < right) -- sort the right part
        }

        public void QuickSort(SortType sortType) // sorting the full array
        {
            QuickSortPart(0, GetLength()-1, sortType);
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

}
