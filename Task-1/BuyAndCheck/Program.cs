using System;

namespace BuyAndCheck
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
            this.price = Price< 0 ? 0 : Price;
            this.weight = Weight< 0 ? 0 : Weight;
        }

        public string GetName()
        {
            return this.name;
        }

        public decimal GetPrice()
        {
            return this.price;
        }

        public decimal GetWeight()
        {
            return this.weight;
        }
    }

    public class Buy
    {
        static int s_count = 0;
        static decimal s_TotalPrice;
        static decimal s_TotalWeight;

        static Product[] Products;
        static Product[] NewProducts;

        public Buy()
        {
            s_count++;
            Console.WriteLine("Введить назву товару #" + s_count);
            string name = Console.ReadLine();
            Console.WriteLine("Введить ціну товару #" + s_count);
            decimal price = Convert.ToDecimal(Console.ReadLine());
            s_TotalPrice += price;
            Console.WriteLine("Введить вагу товару #" + s_count);
            decimal weight = Convert.ToDecimal(Console.ReadLine());
            s_TotalWeight += weight;
            NewProducts = new Product[s_count]; 
            if (Products != null)
            {
                Array.Copy(Products, NewProducts, s_count-1); // s_count == Products.Length + 1
            }
            Products = NewProducts;
            Products[s_count-1] = new Product(name, price, weight);
        }

        public static int GetCount()
        {
            return s_count;
        }
        public static decimal GetTotalPrice()
        {
            return s_TotalPrice;
        }
        public static decimal GetTotalWeight()
        {
            return s_TotalWeight;
        }
        public static Product GetProduct(int n)
        {
            if ((GetCount() < n-1) || (Products == null)) return null;
            else return Products[n];
        }
    }


public class Check
{
    public static void Print()
    {
        Console.WriteLine("Список купленных продуктов");
        for (int i = 0; i < Buy.GetCount(); ++i)
        {
            Product p = Buy.GetProduct(i);
            if (p == null) {
                Console.WriteLine("Error: пустой список");
                break;
            }
            Console.WriteLine(i + ". " + p.GetName() + " - " + p.GetWeight() + " кг - " + p.GetPrice()+" грн");
        }
        Console.WriteLine("Всего: " + Buy.GetTotalWeight() + " кг, " + Buy.GetTotalPrice() + " грн");
    }
}

class Prog
{
        static void Main(string[] args)
        {
            string answer = "N";
            do
            {
                Buy b;
                Console.WriteLine("Введите Y или y, если будете вводить следующий купленный товар");
                answer = Console.ReadLine();
                if ((answer == "Y") || (answer == "y"))
                     b = new Buy();
                else break;
            } while (true);
            Check.Print();
        }
    }
}
