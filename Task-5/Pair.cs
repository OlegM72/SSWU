using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_5
{
    class Pair : IComparable<Pair>
    {
        private int number;
        private int freq;

        public Pair(int number, int freq)
        {
            this.Number = number;
            this.Freq = freq;
        }

        public int Number { get => number; set => number = value; }
        public int Freq { get => freq; set => freq = value; }

        public override string ToString()
        {
            return $"{number} - {freq} times";
        }

        public int CompareTo(Pair second) // pairs are compared by their freqs
        {
            return Freq.CompareTo(second.Freq);
        }
    }
}
