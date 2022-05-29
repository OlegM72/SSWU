using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_5
{
    class Vector
    {
        int[] arr;
        int size; // actual size of the array

        public enum SortType
        {
            Middle,
            First,
            Last
        }

        public int GetLength()
        {
            return size;
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
            size = arr.Length;
        }

        public Vector(int n)
        {
            arr = new int[n];
            size = n;
        }

        public Vector(string fileName) // reading from a text file
        {
            try
            {
                StreamReader stream = new StreamReader(fileName);

                // Count the number of lines (numbers) in the file
                int streamSize = 0;
                while (!stream.EndOfStream)
                {
                    stream.ReadLine();
                    streamSize++;
                }
                
                // We can work only with half-sized arrays
                int half1Size = streamSize / 2;
                // Vector half1 = new Vector(half1Size);

                var readarray = new int[half1Size + 1]; // + 1 for the second half in the case of an odd size
                // reading the first half of the file
                stream.Close();
                stream = new StreamReader(fileName); // reopening
                int i = 0;
                while (i < half1Size)
                {
                    string line = stream.ReadLine();
                    int curr;
                    if (line == null || !int.TryParse(line, out curr))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        readarray[i] = curr;
                        i++;
                    }
                }
                
                half1Size = i;
                // initializing the array of the Vector
                arr = new int[streamSize];
                Array.Copy(readarray, 0, arr, 0, half1Size);

                // reading the second half of the file
                i = 0;
                while (!stream.EndOfStream)
                {
                    string line = stream.ReadLine();
                    int curr;
                    if (line == null || !int.TryParse(line, out curr))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        readarray[i] = curr;
                        i++;
                    }
                }
                Array.Copy(readarray, 0, arr, half1Size, i);
                
                if (arr == null)
                {
                    throw new ArgumentNullException();
                }

                size = half1Size + i; // + half2Size
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                arr = null;
                size = 0;
            }
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

        void Merge(int l, int q, int r) // Merging two sorted parts of the array: from l to q and from q+1 to r
        {
            int i = l, j = q + 1;
            int[] temp = new int[r - l + 1];
            int k = 0;
            while (i <= q && j <= r)
            {
                if (arr[i] < arr[j])
                {
                    temp[k] = arr[i];
                    i++;
                }
                else
                {
                    temp[k] = arr[j];
                    j++;
                }
                k++;
            }
            if (i > q)
            {
                for (int m = j; m <= r; m++)
                {
                    temp[k] = arr[m];
                    k++;
                }
            }
            else
            {
                while (i <= q)
                {
                    temp[k] = arr[i];
                    i++;
                    k++;
                }
            }
            for (int n = 0; n < temp.Length; n++)
            {
                arr[n + l] = temp[n];
            }
        }

        public void SplitMergeSort() // Merge Sort the whole vector
        {
            SplitMergeSort(0, GetLength()-1);
        }

        public void SplitMergeSort(int start, int end) // Merge Sort the part of the vector
        {
            if (end - start <= 1) // stop condition
            {
                if (end != start && arr[end] < arr[start]) // sort 2-element array
                { 
                   swap(end, start);
                }
                return;
            }
            int middle = (end + start) / 2;
            SplitMergeSort(start, middle);
            SplitMergeSort(middle+1, end);
            Merge(start, middle, end);
        }

        private void swap(int i, int j) // swap elements i and j
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        private void shiftDown(int curr, int n)
        // Shift element number curr down in the current binary sort tree of the size n
        // swapping the maximum of the parent i and its chilren
        // and swithing the current element to the maximum one
        {
            int maxIndex = curr; // index of the maximum element from the previous iteration
            var maxValue = arr[curr]; // the value of the current element

            while (true)
            {
                curr = maxIndex; // current element is the maximum of the previous iteration
                int child = curr * 2 + 1; // left child index

                if ((child < n) && (arr[child] > maxValue)) // if it exists and is more than the current
                    maxIndex = child; // then it becomes maximum

                ++child; // change to the right child index
                if ((child < n) && (arr[child] > arr[maxIndex])) // if it exists and is more than the current
                    maxIndex = child; // then it becomes maximum

                if (maxIndex == curr) // if the current element is the maximum we stop shifting
                    break;

                arr[curr] = arr[maxIndex]; arr[maxIndex] = maxValue; // else swap the current with the maximum

                //  Now the maxValue element has the index maxIndex.
                //  At the next iteration we start from this element shifting it down again if needed
            }
        }

        public void HeapSort()
        // Heap sorting of the array arr. 
        {
            HeapSort(GetLength());
        }

        public void HeapSort(int n)
        // Heap sorting of the array arr. 
        // n is the size of the current tree or array 
        {
            // Building the sort tree from the array
            for (int i = n / 2 - 1; i >= 0; --i)
                shiftDown(i, n);

            // Recursive sorting
            while (n > 1)
            {
                --n; // The last element will be the maximum
                swap(0, n); // Swap the root and the last element
                shiftDown(0, n); // Shift the new root down in the sort tree
            }
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
