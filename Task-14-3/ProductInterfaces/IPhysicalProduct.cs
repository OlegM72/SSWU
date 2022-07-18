using System;

namespace Task_14_3
{
    public interface IPhysicalProduct : IIndustrialProduct // a physical product (not food). May have weight and size
    {
        static decimal weight;

        static decimal width;

        static decimal length;

        static decimal height;

        public decimal GetWeight() => Math.Round(weight, 2);
        public decimal GetWidth() => width;
        public decimal GetLength() => length;
        public decimal GetHeight() => height;
    }

}