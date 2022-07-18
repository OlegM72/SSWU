using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Task_14_3
{
    public class MeatProduct : DairyProduct, IFoodProduct
    {
        public enum Category
        {
            NotMeat = 0, // не мясо
            HighSort = 1,
            Sort1 = 2,
            Sort2 = 3
        }

        public enum MeatType
        {
            Other = 0, // другое (кролик, индейка, ...)
            Baran = 1, // баранина
            Telia = 2, // телятина
            Svini = 3, // свинина
            Kurcha = 4 // курятина
        }

        public MeatType type { get; set; }
        public Category category { get; set; }

        public MeatProduct() : base()  // empty constructor creates random product for an abstract factory
        {
            Random random = new Random();
            duedate = DateTime.Today + TimeSpan.FromDays(10);
            category = (Category)random.Next(1, Enum.GetValues(typeof(Category)).Length); // 1-3, since 0 is not meat
            type = (MeatType)random.Next(0, Enum.GetValues(typeof(MeatType)).Length); // 0-4
            name = type.ToString() + "_" + random.Next(1, 50).ToString();
            price = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 350, 2);
            weight = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 3, 2);
        }

        public MeatProduct(DateTime due, Category cat, MeatType typ, string nam, decimal pric, decimal weig) :
                    base(due, nam, pric, weig)
        {
            category = cat;
            type = typ;
        }

        public Category GetCategory() => category;

        public MeatType GetMeatType() => type;
        
        public override int GetHashCode()
        {
            return GetMeatType().GetHashCode() ^ GetCategory().GetHashCode() ^ base.GetHashCode();
        }

        public override string ToString()
        {
            string meatStr = "";
            if (GetCategory() != Category.NotMeat)
            {
                meatStr = ". A meat of sort " + GetCategory() + ", type " + GetMeatType();
            }
            return base.ToString() + meatStr;
        }

        public override void PriceChange(int percent) // зміна ціни на задану кількість відсотків + згідно категорії
        {
            base.PriceChange(percent);
            switch (GetCategory())
            {
                case Category.HighSort:
                    base.PriceChange(25);
                    break;
                case Category.Sort1:
                    base.PriceChange(10);
                    break;
                default: // Sort2 - не змінюємо
                    break;
            }
        }
    }
}