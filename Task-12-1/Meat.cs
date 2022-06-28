using System;
using System.Collections.Generic;
using System.IO;

namespace Task_12_1
{
    public class Meat : DairyProduct
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

        MeatType type { get; set; }
        Category category { get; set; }

        public Meat(DateTime due, Category cat, MeatType typ, string nam, decimal pric, decimal weig) :
                    base(due, nam, pric, weig)
        {
            category = cat;
            type = typ;
        }
        public Category GetCategory()
        {
            return this.category;
        }

        public MeatType GetMeatType()
        {
            return this.type;
        }

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