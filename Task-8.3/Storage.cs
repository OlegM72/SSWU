using System;
using System.Collections.Generic;
using System.IO;

namespace Task_8_3
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

    public class Dairy_products : Product
    {
        int duedate { get; set; } // термін придатності (днів)

        public Dairy_products(int due, string nam, decimal pric, decimal weig) :
                                         base(nam, pric, weig)
        {
            if (due <= 0)
                throw new Exception("Not normal expiration term.");
            else
                duedate = due;
        }

        public int GetDuedate()
        {
            return this.duedate;
        }

        public override void PriceChange(int percent) // зміна ціни на задану кількість відсотків + згідно категорії
        {
            base.PriceChange(percent);
            if (GetDuedate() < 2)
            {
                base.PriceChange(-30);
            }
        }

        public override int GetHashCode()
        {
            return GetDuedate().GetHashCode() ^ base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() + $", expires in {GetDuedate()} days";
        }

    }

    public class Meat : Dairy_products
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

        public Meat(int due, Category cat, MeatType typ, string nam, decimal pric, decimal weig) :
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

    public class ProductComparer : IComparer<Product>
    {
        public int Compare(Product product1, Product product2) // we need only equality comparing in fact
        {
            return product1.GetHashCode().CompareTo(product2.GetHashCode());
        }
    }

    public class Storage
    {
        int s_count = 0;
        decimal s_TotalPrice;
        decimal s_TotalWeight;

        List<Product> products;

        public Storage() // створення порожнього складу
        {
            products = new List<Product>();
            s_count = 0;
            s_TotalPrice = 0;
            s_TotalWeight = 0;
        }

        public Storage(StreamReader reader) // читання колекції товарів з текстового файлу
        {
            if (products != null)
            {
                products.Clear(); // removing the old products list
            }
            s_count = 0;
            int currLine = 0;
            products = new List<Product>();
            while (!reader.EndOfStream)
            {
                currLine++;
                string line = reader.ReadLine();
                if (line.StartsWith('*'))
                    continue; // comment line: just skip it
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
                    int due;
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
                                type = Meat.MeatType.Other;
                                break;
                        }
                    }
                    due = Convert.ToInt32(words[5]);
                    if (due < 0)
                        due = 1;
                    if (products != null)
                    {
                        if (cat != Meat.Category.NotMeat)
                        {
                            Meat nextProduct = new Meat(due, cat, type, name, price, weight);
                            nextProduct.PriceChange(0); // встановлення нової ціни згідно сорту та типу м'яса і строку придатності
                            s_TotalPrice += nextProduct.GetPrice();
                            s_TotalWeight += nextProduct.GetWeight();
                            products.Add(nextProduct);
                            s_count++;
                        }
                        else
                        {
                            Dairy_products nextProduct = new Dairy_products(due, name, price, weight);
                            nextProduct.PriceChange(0); // встановлення нової ціни згідно строку придатності
                            s_TotalPrice += nextProduct.GetPrice();
                            s_TotalWeight += nextProduct.GetWeight();
                            products.Add(nextProduct);
                            s_count++;
                        }
                    }
                } // try
                catch (Exception ex)
                {
                    string message = $"Error (line {currLine}): " + ex.Message;
                    ReportError(message);
                }
            } // while (!EOF)
        }

        public Product this[int index]
        {
            get {
                return GetProduct(index); // index is checked in the method
            }
            set {
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

        public void ReportError(string message)
        {
            Console.WriteLine(message); // can be changed
        }

       private Storage Union(Storage storage2) // return a Storage with all elements in two storages (each appears once)
        {
            ProductComparer comparer = new();
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
            ProductComparer comparer = new();
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
            ProductComparer comparer = new();
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
            for (int i = 0; i < s_count; i++)
            {
                if (products[i] is Meat)
                    meatCount++;
            }
            return meatCount;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < GetCount(); ++i)
            {
                var p = GetProduct(i);
                if (p == null)
                {
                    string message = $"Error: The product {i} is empty or the product link is wrong";
                    ReportError(message);
                    return "";
                }
                result += $"{i + 1}. " + 
                    ((p is Meat) ? (p as Meat).ToString() : (p as Dairy_products).ToString()) + "\r\n";
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