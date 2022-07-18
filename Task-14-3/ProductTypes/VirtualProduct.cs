using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_14_3
{
    internal class VirtualProduct : IVirtualProduct // for music, videos, files, etc
    {
        public enum Category
        {
            Movie = 0,
            Video = 1,
            Music = 2,
            File = 3
        }

        string name;
        decimal price;
        decimal length; // in minutes (for music and videos) or bytes for files
        string genre;
        Category category;
        
        public string GetGenre() => genre; 
        public string GetName() => name;
        public decimal GetPrice() => Math.Round(price, 2);
        public decimal GetLength() => Math.Round(length, 2);
        public Category GetCategory() => category;

        public VirtualProduct() // empty constructor creates random product for an abstract factory
        {
            Random random = new Random();
            category = (Category)random.Next(1, Enum.GetValues(typeof(Category)).Length); // 0-3
            name = category.ToString() + "_" + random.Next(1, 50).ToString();
            genre = "Genre_" + random.Next(1, 10).ToString();
            length = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 200, 2);
            price = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 5 * (double)length, 2);
        }

        public virtual void PriceChange(int percent)
        {
            price = price * (100 + percent) / 100;
        }

        public override int GetHashCode()
        {
            return GetName().GetHashCode() ^ GetPrice().GetHashCode() ^ GetLength().GetHashCode() ^
                   GetGenre().GetHashCode() ^ GetCategory().GetHashCode();
        }

        public override string ToString()
        {
            return GetCategory() + " " + GetName() + ", Genre: " + GetGenre() + ", Price: " 
                 + GetPrice() + " UAH, Length: " + GetLength();
        }
    }
}
