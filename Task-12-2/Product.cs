using System;
using System.Collections.Generic;
using System.IO;

namespace Task_12_2
{
    public class Product
    {
        string name;
        decimal price;
        decimal weight;
        
        public Product(string Name, decimal Price, decimal Weight)
        {
            SetProduct(Name, Price, Weight);
        }

        public void SetProduct(string Name, decimal Price, decimal Weight)
        {
            this.name = Name;
            this.price = Price < 0 ? 0 : Price;
            this.weight = Weight < 0 ? 0 : Weight;
        }

        public virtual void PriceChange(int percent) // зміна ціни на задану кількість відсотків (+/-)
        {
            price = price * (100 + percent) / 100;
        }

        public string GetName()
        {
            return this.name;
        }

        public decimal GetPrice()
        {
            return Math.Round(this.price, 2);
        }

        public decimal GetWeight()
        {
            return Math.Round(this.weight, 2);
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