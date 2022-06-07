using System;
using System.Collections.Generic;
using System.IO;

namespace Task_7
{
    public class Product
    {
        string name;
        double price;
        double weight;
        
        public Product(string Name, double Price, double Weight)
        {
            SetProduct(Name, Price, Weight);
        }
        public void SetProduct(string Name, double Price, double Weight)
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

        public double GetPrice()
        {
            return Math.Round(this.price, 2);
        }

        public double GetWeight()
        {
            return Math.Round(this.weight, 2);
        }

        public override string ToString()
        {
            return GetName() + ", " + GetWeight() + " kg, " + GetPrice() + " UAH";
        }

    }

    public class Dairy_products : Product
    {
        int duedate { get; set; } // термін придатності (днів)

        public Dairy_products(int due, string nam, double pric, double weig) :
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

        public Meat(int due, Category cat, MeatType typ, string nam, double pric, double weig) :
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


    public class Storage
    {
        static int s_count = 0;
        static double s_TotalPrice;
        static double s_TotalWeight;

        static List<Product> products;
        static public string productsListName = "../../../Database.txt";

        public static Log log = null; // link to the log class instance

        public Product this[int index]
        {
            get { return GetProduct(index); }
            set
            {
                if (index < GetCount())
                    products[index] = value;
            }
        }

        public static Product GetProduct(int n)
        {
            if ((GetCount() < n - 1) || (products == null))
                return null;
            else 
                return products[n];
        }

        public Storage(int count) // метод ініціалізації через кількість потрібних товарів
        {
            s_count = count;
            products = new List<Product>();
            var r1 = new Random();
            var r2 = new Random();
            var r3 = new Random();
            var r4 = new Random();
            var r5 = new Random();
            Meat.MeatType typ;
            string name;
            for (int i = 0; i < s_count; i++)
            {
                Meat.Category cat = (Meat.Category)r1.Next(0, 3);
                if (cat == Meat.Category.NotMeat)
                {
                    name = "Not meat";
                    Dairy_products nextProduct = new Dairy_products(r2.Next(1, 20), name, r4.Next(10, 200), r5.Next(1, 5));
                    nextProduct.PriceChange(0); // встановлення нової ціни згідно сорту та типу м'яса і строку придатності
                    s_TotalPrice += nextProduct.GetPrice();
                    s_TotalWeight += nextProduct.GetWeight();
                    products.Add(nextProduct);
                }
                else
                {
                    typ = (Meat.MeatType)r3.Next(1, 4);
                    name = "Meat";
                    Meat nextProduct = new Meat(r2.Next(1, 20), cat, typ, name, r4.Next(10, 200), r5.Next(1, 5));
                    nextProduct.PriceChange(0); // встановлення нової ціни згідно сорту та типу м'яса і строку придатності
                    s_TotalPrice += nextProduct.GetPrice();
                    s_TotalWeight += nextProduct.GetWeight();
                    products.Add(nextProduct);
                }
            }
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
                    double price = Convert.ToDouble(words[1]); // if error, there will be an exception
                    double weight = Convert.ToDouble(words[2]); // the same :)
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
                    if (log != null) log.PutRecord(message);
                    else Console.WriteLine();
                }
            } // while (!EOF)
        }

        public Storage() // створення колекції товарів через діалог з користувачем
        {
            string answer;
            if (products != null)
            {
                products.Clear(); // removing the old products list
            }
            s_count = 0;
            products = new List<Product>();
            do
            {
                Console.WriteLine("Enter Y or y if you will enter the next bought product");
                answer = Console.ReadLine();
                if (answer.ToUpper() == "Y")
                    EnterProduct();
                else
                    break;
            } while (true);
        }

        public void EnterProduct() // створення ОДНОГО товару через діалог з користувачем та додавання до статичної колекції
        {
            try
            {
                Console.Write($"Enter the NAME of the product # {s_count+1} :");
                string name = Console.ReadLine();
                Console.Write($"Enter the PRICE of the product  # {s_count+1} :");
                double price = Convert.ToDouble(Console.ReadLine());
                Console.Write($"Enter the WEIGHT of the product  # {s_count + 1} :");
                double weight = Convert.ToDouble(Console.ReadLine());
                Console.Write("If it is not a meat then enter 0 else enter its category/sort (1..3): ");
                int catstr = Convert.ToInt32(Console.ReadLine());
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
                    Console.Write("Enter the meat type: 1- mutton, 2 - veal, 3 - pork, 4 - chicken: ");
                    int typestr = Convert.ToInt32(Console.ReadLine());
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
                Console.WriteLine("Enter the expiration date (days from now):");
                due = Convert.ToInt32(Console.ReadLine());
                if (due < 0)
                    due = 1;
                if (products != null)
                {
                    if (cat == Meat.Category.NotMeat)
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
                string message = $"Error (line {s_count}): " + ex.Message;
                if (log != null) log.PutRecord(message);
                else Console.WriteLine();
            }
        }

        public static int GetCount()
        {
            return s_count;
        }

        public static double GetTotalPrice()
        {
            return Math.Round(s_TotalPrice, 2);
        }

        public static double GetTotalWeight()
        {
            return Math.Round(s_TotalWeight, 2);
        }

        public static int GetMeatCount()
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
                    if (log != null) log.PutRecord(message);
                    else Console.WriteLine();
                    return "";
                }
                result += $"{i + 1}. " + 
                    ((p is Meat) ? (p as Meat).ToString() : (p as Dairy_products).ToString()) + "\r\n";
            }
            return result + $"TOTAL: {GetTotalWeight()} kg, {GetTotalPrice()} UAH, meat: {GetMeatCount()} pieces";
        }
    }

}