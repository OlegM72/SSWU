using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_14_3
{
    internal class HouseholdChemicalProduct : IPhysicalProduct
    {
        string name;
        decimal price;
        decimal weight;
        string characteristics;   // for example, volume, hardness

        public string GetName() => name;
        public decimal GetPrice() => Math.Round(price, 2);
        public decimal GetWeight() => Math.Round(weight, 2);
        public string GetCharacteristics() => characteristics;

        public HouseholdChemicalProduct() // empty constructor creates random product for an abstract factory
        {
            Random random = new Random();
            name = "Chemical_" + random.Next(1, 50).ToString();
            weight = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 20, 2);
            price = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 50 * (double)weight, 2);
            characteristics = "Volume " + (Math.Round(weight * 4, 0, MidpointRounding.ToZero) / 4).ToString() + " l";
        }

        public virtual void PriceChange(int percent)
        {
            price = price * (100 + percent) / 100;
        }

        public override int GetHashCode()
        {
            return GetName().GetHashCode() ^ GetWeight().GetHashCode() ^ GetPrice().GetHashCode() ^
                   GetCharacteristics().GetHashCode();
        }

        public override string ToString()
        {
            return GetName() + ", " + GetCharacteristics() + ", Weight: " + GetWeight() + " kg, " +
                "Price: " + GetPrice() + " UAH";
        }
    }
}
