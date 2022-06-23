using System;
using System.IO;

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
            if (arr == null)
                size = 0;
            else
                size = arr.Length;
        }

        public Vector(int n)
        {
            arr = new int[n];
            size = n;
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

        // Partial reading from the file and saving parts to two files
        public Vector(string readFileName, string part1FileName, string part2FileName)
        {
            try
            {
                using (StreamReader reader = new StreamReader(readFileName))
                {
                    // Count the number of lines (numbers) in the file
                    int streamSize = (File.ReadAllLines(readFileName)).Length;

                    // We can work only with half-sized arrays
                    int half1Size = streamSize - (streamSize / 2);

                    // reading the first half of the file and saving it to the first partial file
                    // it would be more efficient to read, SORT and save,
                    // but I want to have these functions separately for convenience
                    int i = 0;
                    StreamReader stream = new StreamReader(readFileName);
                    using (StreamWriter writer = new StreamWriter(part1FileName))
                    {
                        while (i < half1Size)
                        {
                            string line = reader.ReadLine();
                            int curr;
                            if (line == null || !int.TryParse(line, out curr))
                            {
                                throw new ArgumentException();
                            }
                            else
                            {
                                writer.WriteLine(curr);
                                i++;
                            }
                        }
                    }
                    // reading the second half of the file and saving it to the second partial file
                    using (StreamWriter writer = new StreamWriter(part2FileName))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            int curr;
                            if (line == null || !int.TryParse(line, out curr))
                            {
                                throw new ArgumentException();
                            }
                            else
                            {
                                writer.WriteLine(curr);
                                i++;
                            }
                        }
                    }
                    size = i;
                }
            }
            catch
            {
                size = 0;
                arr = null;
                throw;
            }
            arr = null; // all the data are in the files
        }

        public void ReadPart(StreamReader reader, int linesCount) // reading linesCount lines from the file to the current vector
		{
            if (linesCount <= 0)
                throw new ArgumentException("Wrong number of lines to read");
            try
            {
                arr = new int[linesCount];
                int i = 0;
                while (i < linesCount && !reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    int curr;
                    if (line == null || !int.TryParse(line, out curr))
                    {
                        throw new FormatException($"Cannot read the file or a number format error, line #{i+1}");
                    }
                    else
                    {
                        arr[i] = curr;
                        i++;
                    }
                }
                size = i;
            }
            catch
            {
                size = 0;
                arr = null;
                throw;
            }
        }

        public bool SaveToFile(string fileName) // fully saving the array to the text file, 1 line per number
        // saving as a single string seems to be better, it will be implemented in the next versions :-)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    for (int i = 0; i < GetLength(); i++)
                    {
                        writer.WriteLine(arr[i]);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public Vector(string fileName) // FULL reading from a text file
        {
            try
            {
                // reading the file to find its size
                int streamSize = (File.ReadAllLines(fileName)).Length;
                
                size = streamSize;
                // initializing the array of the Vector
                arr = new int[size];
                if (arr == null)
                {
                    throw new ArgumentNullException();
                }

                // reading again the file to the array arr
                using (StreamReader stream = new StreamReader(fileName))
                {
                    int i = 0;
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
                            arr[i] = curr;
                            i++;
                        }
                    }
                }
            }
            catch
            {
                arr = null;
                size = 0;
                throw;
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

        // Sorting with files (merge sort partial files and merge them to the sortedFileName)
        public void SplitMergeSortFiles(string part1FileName, string part2FileName, string sortedFileName)
        {
            Vector Part = new Vector(part1FileName); // fully reading the part 1
            Part.SplitMergeSort();
            if (!Part.SaveToFile(part1FileName))
               return;

            Part = new Vector(part2FileName); // fully reading the part 2
            Part.SplitMergeSort();
            if (!Part.SaveToFile(part2FileName))
                return;

            MergeFiles(part1FileName, part2FileName, sortedFileName); // merging two sorted files
        }

        public void MergeSortedFileWithMemory(string part1FileName, string sortedFileName)
        // NEW method of merging a sorted file with a current vector from memory into a bigger sorted file
        {
            try
            {
                using (StreamReader reader = new StreamReader(part1FileName))
                using (StreamWriter writer = new StreamWriter(sortedFileName))
                {
                    bool read1 = true; // true if we need to read the next value from the file 1
                    bool read2 = true; // true if we need to read the next value from the file 2
                    int curr1 = -1; // the number read last from reader1
                    int curr2 = -1; // the number read last from reader2
                    string line1 = null;
                    int index2 = 0;
                    do
                    {
                        if (read1 && !reader.EndOfStream)
                        {
                            line1 = reader.ReadLine();
                            if (line1 == null || !int.TryParse(line1, out curr1))
                            {
                                line1 = null;
                                throw new FormatException("Cannot read the file or a number format error");
                            }
                        }
                        if (read2 && index2 < GetLength())
                        {
                            curr2 = arr[index2];
                            index2++;
                        }

                        int currOut; // the number to write next
                        if (line1 != null && index2 <= GetLength())
                        {
                            if (curr1 < curr2)
                            {
                                currOut = curr1;
                                read1 = true;
                                read2 = false;
                            }
                            else
                            {
                                currOut = curr2;
                                read1 = false;
                                read2 = true;
                            }
                            writer.WriteLine(currOut);
                        }
                    }
                    while ((read1 && !reader.EndOfStream) ||
                           (read2 && index2 < GetLength()));

                    if (reader.EndOfStream)
                    {
                        if (!read1) // we have not yet written the last value from the first file
                            writer.WriteLine(curr1);
                        if (!read2) // we have not yet written the last value from the second file
                            writer.WriteLine(curr2);
                        while (index2 < GetLength())
                        {
                            writer.WriteLine(arr[index2]);
                            index2++;
                        }
                    }
                    else
                    {
                        if (!read2) // we have not yet written the last value from the second file
                            writer.WriteLine(curr2);
                        if (!read1) // we have not yet written the last value from the first file
                            writer.WriteLine(curr1);
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            writer.WriteLine(line);
                        }
                    }
                }
            }
            catch
            {
                throw; // to the Main method
            }
        }

        public void MergeFiles(string part1FileName, string part2FileName, string sortedFileName)
        // merging two sorted files into bigger sorted file
        {
            try
            {
                using (StreamReader reader1 = new StreamReader(part1FileName))
                using (StreamReader reader2 = new StreamReader(part2FileName))
                using (StreamWriter writer = new StreamWriter(sortedFileName))
                {
                    bool read1 = true; // true if we need to read the next value from the file 1
                    bool read2 = true; // true if we need to read the next value from the file 2
                    int curr1 = -1; // the number read last from reader1
                    int curr2 = -1; // the number read last from reader2
                    string line1 = null;
                    string line2 = null;
                    do
                    {
                        if (read1 && !reader1.EndOfStream)
                        {
                            line1 = reader1.ReadLine();
                            if (line1 == null || !int.TryParse(line1, out curr1))
                            {
                                line1 = null;
                                throw new ArgumentException();
                            }
                        }
                        if (read2 && !reader2.EndOfStream)
                        {
                            line2 = reader2.ReadLine();
                            if (line2 == null || !int.TryParse(line2, out curr2))
                            {
                                line2 = null;
                                throw new ArgumentException();
                            }
                        }

                        int currOut; // the number to write next
                        if (line1 != null && line2 != null)
                        {
                            if (curr1 < curr2)
                            {
                                currOut = curr1;
                                read1 = true;
                                read2 = false;
                            }
                            else
                            {
                                currOut = curr2;
                                read1 = false;
                                read2 = true;
                            }
                            writer.WriteLine(currOut);
                        }
                    }
                    while ((read1 && !reader1.EndOfStream) ||
                           (read2 && !reader2.EndOfStream));

                    if (reader1.EndOfStream)
                    {
                        if (!read1) // we have not yet written the last value from the first file
                            writer.WriteLine(curr1);
                        if (!read2) // we have not yet written the last value from the second file
                            writer.WriteLine(curr2);
                        while (!reader2.EndOfStream)
                        {
                            string line = reader2.ReadLine();
                            writer.WriteLine(line);
                        }
                    }
                    else
                    {
                        if (!read2) // we have not yet written the last value from the second file
                            writer.WriteLine(curr2);
                        if (!read1) // we have not yet written the last value from the first file
                            writer.WriteLine(curr1);
                        while (!reader1.EndOfStream)
                        {
                            string line = reader1.ReadLine();
                            writer.WriteLine(line);
                        }
                    }
                }
            }
            catch
            {
                throw; // to the Main method
            }
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
                while (j <= r)
                {
                    temp[k] = arr[j];
                    j++;
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
            if (arr == null)
                return ""; // some error;

            string str = "";
            for (int i = 0; i < GetLength(); i++)
            {
                str += arr[i] + " ";
            }
            return str;
        }
    }

}
