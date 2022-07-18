using System.Collections.Generic;
using System.IO;
using System;

namespace Task_14_3
{
    public class Storage   // реалізація Складу як Одинака (Singleton)
    {
        private static Storage instance; // Declaring the instance

        public int s_count = 0;
        public decimal s_TotalPrice;
        public decimal s_TotalWeight;

        public List<IProduct> products;

        private Storage() // створення порожнього складу 
        {
            products = new List<IProduct>();
            s_count = 0;
            s_TotalPrice = 0;
            s_TotalWeight = 0;
        }

        // Initializing the instance on the first call.
        // The Global Access Point to the instance.
        // Warning: Singleton will not work in a multi-threading environment
        public static Storage Instance()
        {
            if (instance == null)
                instance = new Storage(); // creating empty Storage
            return instance;
        }

        public void ProduceProducts()  // creating products and adding to Storage using abstract factory
        {
            NovaLiniaFactory roshen = new();
            EpicentrFactory riaba = new();
            for (int i = 0; i < 5; i++) // create 5 random products of each type and print them
            {
                CreateFactoryProducts(roshen);
                CreateFactoryProducts(riaba);
            }
        }

        internal void CreateFactoryProducts(IAbstractFactory factory)
        {
            IIndustrialProduct industrialProduct = factory.CreateIndustrialProduct();
            Console.WriteLine("Factory '" + factory.Name + "' has produced industrial product: " + industrialProduct);
            AddProduct(industrialProduct);

            IFoodProduct foodProduct = factory.CreateFoodProduct();
            Console.WriteLine("Factory '" + factory.Name + "' has produced food product: " + foodProduct);
            AddProduct(foodProduct);
        }

        public void AddProduct(IProduct product) // a template for the future method of adding a product
        {
            product.PriceChange(0); // встановлення нової ціни згідно сорту та типу м'яса і строку придатності
            if (products is null)
                products = new();
            products.Add(product);
            s_count++;
            s_TotalPrice += product.GetPrice();
            if (product is IFoodProduct)
                s_TotalWeight += ((FoodProduct)product).GetWeight();
        }

        public IProduct this[int index]
        {
            get
            {
                return GetProduct(index); // index is checked in the method
            }
            set
            {
                if (index >= 0 && index < GetCount())
                    products[index] = value;
            }
        }

        public IProduct GetProduct(int n)
        {
            if ((GetCount() < n - 1) || (products == null))
                return null;
            else
                return products[n];
        }

        public int GetCount()
        {
            return s_count;
        }

        public decimal GetTotalPrice()
        {
            return Math.Round(s_TotalPrice, 2);
        }

        public decimal GetTotalWeight()
        {
            return Math.Round(s_TotalWeight, 2);
        }

        public int GetMeatCount()
        {
            int meatCount = 0;
            foreach (IProduct product in products)
                if (product is MeatProduct)
                    meatCount++;
            return meatCount;
        }

        public override string ToString()
        {
            string result = "";
            int i = 1;
            foreach (IProduct product in products)
            {
                if (product == null)
                    throw new ArgumentException($"Error: A product {i} is empty or the product link is wrong");
                result += $"{i}. {product}\r\n";
                i++;
            }
            return result + $"TOTAL: {GetTotalWeight()} kg, {GetTotalPrice()} UAH, meat: {GetMeatCount()} pieces";
        }
    }
}