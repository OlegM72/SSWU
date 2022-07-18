using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_14_3
{
    public interface IFoodProduct : IProduct // a product that can be food. Usually has weight
    {
        static decimal weight;
        public decimal GetWeight() => weight;
    }
}
