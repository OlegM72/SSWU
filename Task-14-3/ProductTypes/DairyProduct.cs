using System;
using System.Collections.Generic;
using System.IO;

namespace Task_14_3
{
    public class DairyProduct : FoodProduct, IProduct
    {
        public DateTime duedate { get; set; } // термін придатності

        public DairyProduct() : base() // empty constructor creates random product for an abstract factory
        {
            Random random = new Random();
            duedate = DateTime.Today + TimeSpan.FromDays(10);
            name = "Dairy_" + random.Next(1, 50).ToString();
            price = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 250, 2);
            weight = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 3, 2);
        }

        public DairyProduct(DateTime due, string nam, decimal pric, decimal weig) :
                                         base(nam, pric, weig)
        {
            duedate = due;
        }

        public DateTime GetDuedate()
        {
            return duedate;
        }

        public override void PriceChange(int percent) // зміна ціни на задану кількість відсотків + згідно терміну придатності
        {
            base.PriceChange(percent);
            if (DateTime.Now > GetDuedate())
                base.PriceChange(-100);
            else if (DateTime.Now - GetDuedate() < TimeSpan.FromDays(2))
                base.PriceChange(-30);
        }

        public override int GetHashCode()
        {
            return GetDuedate().GetHashCode() ^ base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() + $", expires {GetDuedate():d}";
        }

    }
}