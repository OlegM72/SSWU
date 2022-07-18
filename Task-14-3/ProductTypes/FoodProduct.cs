using System;
using System.Collections.Generic;
using System.IO;

namespace Task_14_3
{
    public class FoodProduct : IFoodProduct // a food product that is not meat or dairy product, just other
    {
        public string name;
        public decimal price;
        public decimal weight;

        public FoodProduct() // empty constructor creates random product for an abstract factory
        {
            Random random = new Random();
            name = "Food_" + random.Next(1, 50).ToString();
            price = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 150, 2);
            weight = (decimal)Math.Round(Math.Abs(random.NextDouble()) * 2, 2);
        }

        public FoodProduct(string Name, decimal Price, decimal Weight)
        {
            SetProduct(Name, Price, Weight);
        }

        public void SetProduct(string name, decimal price, decimal weight)
        {
            this.name = name;
            this.price = price < 0 ? 0 : price;
            this.weight = weight < 0 ? 0 : weight;
        }

        public string GetName() => name;

        public decimal GetPrice() => Math.Round(price, 2);

        public decimal GetWeight() => Math.Round(weight, 2);

        public virtual void PriceChange(int percent)
        {
            price = price * (100 + percent) / 100;
        }

        public override int GetHashCode()
        {
            return GetName().GetHashCode() ^ GetWeight().GetHashCode() ^ GetPrice().GetHashCode();
            // ^ base.GetHashCode(); // adding this includes internal object information that may be different
        }

        public override string ToString()
        {
            return GetName() + ", " + GetWeight() + " kg, " + GetPrice() + " UAH";
        }

     }
}