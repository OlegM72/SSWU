using System;
using System.Collections.Generic;
using System.IO;

namespace Task_12_2
{
    public class Storage
    {
        int s_count = 0;
        decimal s_TotalPrice;
        decimal s_TotalWeight;

        public static string utilizationLogName;

        List<Product> products;

        public Storage() // створення порожнього складу
        {
            products = new List<Product>();
            s_count = 0;
            s_TotalPrice = 0;
            s_TotalWeight = 0;
            ExpiredProductFound = RemoveExpiredProduct; // setting event handler methods
            ExpiredProductFound += WriteExpiredProduct;
            SetComparer(ComparerType.Hash);
        }

        private IComparer<Product> currentComparer;

        ProductHashComparer hashComparer;
        ProductNameComparer nameComparer;
        ProductPriceComparer priceComparer;
        ProductWeightComparer weightComparer;

        public enum ComparerType
        {
            Hash = 0,
            Name,
            Price,
            Weight
        }

        private IComparer<Product> GetComparer(ComparerType type)
        {
            switch (type) {
                case ComparerType.Hash: { hashComparer = new(); return hashComparer; }
                case ComparerType.Name: { nameComparer = new(); return nameComparer; }
                case ComparerType.Price: { priceComparer = new(); return priceComparer; }
                case ComparerType.Weight: { weightComparer = new(); return weightComparer; }
                default: throw new ArgumentException($"Wrong comparer type: {type}");
            };
        }

        public void SetComparer(ComparerType type)
        {
            currentComparer = GetComparer(type);
        }

        public void RemoveExpiredProduct(DairyProduct product) // delegate implementation #1: remove the given product from storage
        {
            products.Remove(product);
            s_count--;
        }
        public void WriteExpiredProduct(DairyProduct product) // delegate implementation #2: list the product in the utilization file
        {
            using (StreamWriter writer = new(utilizationLogName, true)) // append mode
                writer.WriteLine($"{DateTime.Now:d}: Removed expired product {product}");
        }

        public event ExpiredProduct<ExpiredProductArgs> ExpiredProductFound; // event declaration

        public void AddProduct(Product product) // a template for the future method of adding a product
        {
            products.Add(product);
            CheckExpiredProducts();
        }

        public void CheckExpiredProducts() // checking the list and finding expired dairy products
        {
            bool found = false;
            do
                foreach (Product product in products)
                {
                    found = false;
                    if ((product is DairyProduct || product is Meat) &&
                        DateTime.Now > ((DairyProduct)product)?.GetDuedate())
                    {
                        found = true;
                        ExpiredProductFound?.Invoke((DairyProduct)product);
                        break; // the collection has changed, need to restart the cycle, or we get an error
                    }
                }
            while (found);
        }

        public Storage(StreamReader reader) // читання колекції товарів з текстового файлу
        {
            products = new List<Product>();
            s_count = 0;
            s_TotalPrice = 0;
            s_TotalWeight = 0;
            ExpiredProductFound = RemoveExpiredProduct; // setting event handler methods
            ExpiredProductFound += WriteExpiredProduct;
            SetComparer(ComparerType.Hash);
            int currLine = 0;
            while (!reader.EndOfStream)
            {
                currLine++;
                string line = reader.ReadLine();
                if (!line.StartsWith('*')) // if comment line: just skip it
                {
                    try
                    {
                        // Name; Price; Weight; 0 or meat category 1-3; meat type 1-4 or 0 for other;
                        // expiration date (days from now)
                        string[] words = line.Split(';', StringSplitOptions.RemoveEmptyEntries);
                        string name = char.ToUpper(words[0][0]) + words[0][1..]; // uppercase the first char
                        decimal price = Convert.ToDecimal(words[1]); // if error, there will be an exception
                        decimal weight = Convert.ToDecimal(words[2]); // the same :)
                        int catstr = Convert.ToInt32(words[3]);
                        Meat.Category cat;
                        if (catstr > 3 || catstr < 0)
                            catstr = 0;
                        switch (catstr)
                        {
                            case 1:
                                cat = Meat.Category.HighSort;
                                break;
                            case 2:
                                cat = Meat.Category.Sort1;
                                break;
                            case 3:
                                cat = Meat.Category.Sort2;
                                break;
                            default:
                                cat = Meat.Category.NotMeat;
                                break;
                        }
                        Meat.MeatType type = 0;
                        if (cat != Meat.Category.NotMeat)
                        {
                            int typestr = Convert.ToInt32(words[4]);
                            if (typestr > 4 || typestr < 1)
                                typestr = 0;
                            switch (typestr)
                            {
                                case 1:
                                    type = Meat.MeatType.Baran; // баранина
                                    break;
                                case 2:
                                    type = Meat.MeatType.Telia; // телятина
                                    break;
                                case 3:
                                    type = Meat.MeatType.Svini; // свинина
                                    break;
                                case 4:
                                    type = Meat.MeatType.Kurcha; // курятина
                                    break;
                                default:
                                    type = Meat.MeatType.Other; // other dairy product
                                    break;
                            }
                        }
                        DateTime due = DateTime.MinValue;
                        if (words.Length < 6) // empty date - maybe not a DairyProduct
                        {
                            if (cat != Meat.Category.NotMeat)
                                throw new ArgumentException($"Wrong format of the date for the product {name}");
                        }
                        else
                            if (!DateTime.TryParse(words[5], out due))
                            throw new ArgumentException($"Wrong format of the date for the product {name}");
                        if (products != null)
                        {
                            Product nextProduct;
                            if (cat == Meat.Category.NotMeat)
                            {
                                if (due == DateTime.MinValue)
                                    nextProduct = new Product(name, price, weight);
                                else
                                    nextProduct = new DairyProduct(due, name, price, weight);
                            }
                            else
                                nextProduct = new Meat(due, cat, type, name, price, weight);
                            nextProduct.PriceChange(0); // встановлення нової ціни згідно сорту та типу м'яса і строку придатності
                            s_TotalPrice += nextProduct.GetPrice();
                            s_TotalWeight += nextProduct.GetWeight();
                            products.Add(nextProduct);
                            s_count++;
                        }
                    } // try
                    catch (Exception ex)
                    {
                        throw new Exception($"Error (line {currLine}): " + ex.Message);
                    }
                }
            } // while (!EOF)
            CheckExpiredProducts();
        }

        public Product this[int index]
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

        public Product GetProduct(int n)
        {
            if ((GetCount() < n - 1) || (products == null))
                return null;
            else
                return products[n];
        }

        public void Sort() // sorting using current comparer
        {
            if (currentComparer is null)
                throw new NullReferenceException("The current comparer is not set");
            products.Sort(currentComparer);
        }

        public static void PrintUtilizationLog()
        {
            using (StreamReader reader = new(utilizationLogName))
            {
               string line;
               while ((line = reader.ReadLine()) != null)
                   Console.WriteLine(line);
            }
        }

        private Storage Union(Storage storage2) // return a Storage with all elements in two storages (each appears once)
        {
            ProductHashComparer comparer = new();
            SortedSet<Product> sortedProducts = new(comparer); // a SortedSet includes each element only once
            decimal totalPrice = 0;
            decimal totalWeight = 0;

            foreach (Product product in products)
            {
                if (sortedProducts.Add(product))
                {
                    totalPrice += product.GetPrice();
                    totalWeight += product.GetWeight();
                }
            }
            foreach (Product product in storage2.products)
            {
                if (sortedProducts.Add(product))
                {
                    totalPrice += product.GetPrice();
                    totalWeight += product.GetWeight();
                }
            }
            Storage result = new(); // empty Storage
            foreach (Product product in sortedProducts)
                result.products.Add(product);
            result.s_count = sortedProducts.Count;
            result.s_TotalPrice = totalPrice;
            result.s_TotalWeight = totalWeight;
            return result;
        }

        private Storage Subtract(Storage storage2) // return a Storage without the elements in the second storage
        {
            decimal totalPrice = s_TotalPrice;
            decimal totalWeight = s_TotalWeight;

            Storage result = new(); // empty Storage
            ProductHashComparer comparer = new();
            // Search only works in sorted lists so we create temporary sorted list
            SortedSet<Product> sortedProducts = new(comparer);
            foreach (Product product in storage2.products)
            {
                sortedProducts.Add(product);
            }

            foreach (Product product in products)
            {
                if (sortedProducts.Contains(product)) // found
                {
                    totalPrice -= product.GetPrice();
                    totalWeight -= product.GetWeight();
                }
                else
                {
                    result.products.Add(product);
                }
            }
            result.s_count = result.products.Count;
            result.s_TotalPrice = totalPrice;
            result.s_TotalWeight = totalWeight;
            return result;
        }

        private Storage Intersect(Storage storage2) // return a Storage with common elements in two storages
        {
            decimal totalPrice = s_TotalPrice;
            decimal totalWeight = s_TotalWeight;

            Storage result = new(); // empty Storage
            ProductHashComparer comparer = new();
            // Search only works in sorted lists so we create temporary sorted list
            SortedSet<Product> sortedProducts = new(comparer);
            foreach (Product product in storage2.products)
            {
                sortedProducts.Add(product);
            }

            foreach (Product product in products)
            {
                if (!sortedProducts.Contains(product)) // not found
                {
                    totalPrice -= product.GetPrice();
                    totalWeight -= product.GetWeight();
                }
                else
                {
                    result.products.Add(product);
                }
            }
            result.s_count = result.products.Count;
            result.s_TotalPrice = totalPrice;
            result.s_TotalWeight = totalWeight;
            return result;
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
            foreach (Product product in products)
                if (product is Meat)
                    meatCount++;
            return meatCount;
        }

        public override string ToString()
        {
            string result = "";
            int i = 1;
            foreach (Product product in products)
            {
                if (product == null)
                    throw new ArgumentException($"Error: A product {i} is empty or the product link is wrong");
                result += $"{i}. {product}\r\n";
                i++;
            }
            return result + $"TOTAL: {GetTotalWeight()} kg, {GetTotalPrice()} UAH, meat: {GetMeatCount()} pieces";
        }

        public static Storage operator +(Storage storage1, Storage storage2)
        {
            return storage1.Union(storage2);
        }

        public static Storage operator -(Storage storage1, Storage storage2)
        {
            return storage1.Subtract(storage2);
        }

        public static Storage operator *(Storage storage1, Storage storage2)
        {
            return storage1.Intersect(storage2);
        }
    }
}