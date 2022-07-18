using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_14_3
{
    internal class TechnicProduct : IPhysicalProduct
    {
        string name;
        decimal price;
        decimal weight;
        decimal width;
        decimal length;
        decimal height;
        string characteristics;   // for example, CPU type, screen resolution, volume, number of wheels

        public string GetName() => name;
        public decimal GetPrice() => Math.Round(price, 2);
        public decimal GetWeight() => Math.Round(weight, 2);
        public decimal GetWidth() => Math.Round(width, 2);
        public decimal GetHeight() => Math.Round(height, 2);
        public decimal GetLength() => Math.Round(length, 2);
        public string GetCharacteristics() => characteristics;

        public TechnicProduct() // empty constructor creates random product for an abstract factory
        {
            Random random = new Random();
            name = "Technic_" + random.Next(1, 50).ToString();
            width = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 1.20, 2);
            length = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 2, 2);
            height = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 0.8, 2);
            weight = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 25 * (double)width * (double)height * (double)length, 2);
            price = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 2000 * (double)weight, 2);
            characteristics = "220 V, 50 Hz";
        }

        public virtual void PriceChange(int percent)
        {
            price = price * (100 + percent) / 100;
        }

        public override int GetHashCode()
        {
            return GetName().GetHashCode() ^ GetWeight().GetHashCode() ^ GetPrice().GetHashCode() ^
                   GetWidth().GetHashCode() ^ GetHeight().GetHashCode() ^ GetLength().GetHashCode() ^
                   GetCharacteristics().GetHashCode();
        }

        public override string ToString()
        {
            return GetName() + ", " + GetCharacteristics() + ", Weight: " + GetWeight() + " kg, " + 
                "Price: " + GetPrice() + " UAH, Size (LxWxH): " + 
                GetLength() + " x " + GetWidth() + " x " + GetHeight();
        }
    }
}
