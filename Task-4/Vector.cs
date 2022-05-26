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
                if (index >= 0 && index < GetLength())
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
            for (int i = 0; i < GetLength(); i++)
            {
                arr[i] = 0;
            }

            Random random = new Random();
            int x;
            for (int i = 0; i < GetLength(); i++)
            {
                while (arr[i] == 0)
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

        private int Partition(int left, int right, SortType sortType) // returns the index of the pivot (base element)
        {
            switch (sortType)
            {
                case SortType.First:
                    return left;
                case SortType.Last:
                    return right;
                case SortType.Middle:
                    return left + ((right - left + 1) / 2); // if (0, 1), need to be 1
                default:
                    return 0;
            }
        }

        private void QuickSortPart(int left, int right, SortType sortType) // sorting the array part from "left" to "right"
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

        public void QuickSort(SortType sortType) // sorting the array part from "left" to "right"
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
