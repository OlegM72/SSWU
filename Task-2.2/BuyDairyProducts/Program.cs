using System;

namespace BuyDairyProducts
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
            return GetName() + ", " + GetWeight() + " кг, " + GetPrice() + " грн";
        }

    }

    public class Dairy_products : Product
    {
        int duedate { get; set; } // термін придатності (днів)

        public Dairy_products(int due, string nam, double pric, double weig) :
                                         base(nam, pric, weig)
        {
            if (due <= 0)
                throw new Exception("Ненормальний термін придатності");
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
            string dueStr = "Придатність: " + GetDuedate() + " днів. ";
            return dueStr + base.ToString();
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
            NotMeat = 0, // не мясо
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
                meatStr = "М'ясо, сорт " + GetCategory() + ", тип " + GetMeatType() + ". ";
            }
            return meatStr + base.ToString();
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

        static Meat[] Products;
        private static Meat[] NewProducts;
        public Meat this[int index]
        {
            get { return GetProduct(index); }
            set
            {
                if (index < GetCount())
                    Products[index] = value;
            }
        }

        public static Meat GetProduct(int n)
        {
            if ((GetCount() < n - 1) || (Products == null)) return null;
            else return Products[n];
        }

        public Storage(int count) // метод ініціалізації через кількість потрібних товарів
        {
            s_count = count;
            Products = new Meat[s_count];
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
                    typ = Meat.MeatType.NotMeat;
                    name = "Not meat";
                }
                else
                {
                    typ = (Meat.MeatType)r3.Next(1, 4);
                    name = "Meat";
                }
                Products[i] = new Meat(r2.Next(1,20), cat, typ, name, r4.Next(10, 200), r5.Next(1, 5));
                Products[i].PriceChange(0); // встановлення нової ціни згідно сорту та типу м'яса і строку придатності
                s_TotalPrice += Products[i].GetPrice();
                s_TotalWeight += Products[i].GetWeight();
            }

        }

        public Storage() // діалог з користувачем
        {
            s_count++;
            Console.WriteLine("Введить назву товару #" + s_count);
            string name = Console.ReadLine();
            Console.WriteLine("Введить ціну товару #" + s_count);
            double price = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Введить вагу товару #" + s_count);
            double weight = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Якщо це м'ясо, введить категорію (не м'ясо - 0, інакше 1-3)");
            int catstr = Convert.ToInt32(Console.ReadLine());
            Meat.Category cat;
            if (catstr > 3 || catstr < 0)
                catstr = 0;
            switch (catstr) {
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
            Meat.MeatType type;
            int due;
            if (cat == 0)
            {
                type = Meat.MeatType.NotMeat;
                due = 30;
            }
            else
            {
                Console.WriteLine("Введить тип м'яса: 1- баранина, 2 - телятина, 3 - свинина, 4 - курятина:");
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
                        type = Meat.MeatType.NotMeat;
                        break;
                }
                Console.WriteLine("Введить строк придатності (днів):");
                due = Convert.ToInt32(Console.ReadLine());
                if (due < 0) 
                    due = 1;
            }

            NewProducts = new Meat[s_count]; // створюємо новий масив продуктів та копіюємо у нього спочатку старий
            if (Products != null)
            {
                Array.Copy(Products, NewProducts, s_count - 1); // s_count == Products.Length + 1
            }
            Products = NewProducts;
            Products[s_count - 1] = new Meat(due, cat, type, name, price, weight); // додаємо новий продукт останнім
            Products[s_count - 1].PriceChange(0); // встановлення нової ціни згідно сорту та типу м'яса і строку придатності
            s_TotalPrice += Products[s_count - 1].GetPrice();
            s_TotalWeight += Products[s_count - 1].GetWeight();
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
                if (Products[i].GetMeatType() != Meat.MeatType.NotMeat)
                    meatCount++;
            }
            return meatCount;
        }

        public void Print()
        {
            Console.WriteLine("Список куплених продуктів");
            for (int i = 0; i < GetCount(); ++i)
            {
                Product p = GetProduct(i);
                if (p == null)
                {
                    Console.WriteLine("Error: пустий список");
                    break;
                }
                Console.Write(i + 1); Console.WriteLine(". " + p.ToString());
            }
            Console.WriteLine("Всього: " + GetTotalWeight() + " кг, " + GetTotalPrice() + " грн, м'ясо: " +
                GetMeatCount() + " одиниць");
        }
    }

    class Prog
    {
        static void Main(string[] args)
        {
            string answer = "N";
            Storage s = null;
            do
            {
                Console.WriteLine("Введите Y или y, если будете вводить следующий купленный товар");
                answer = Console.ReadLine();
                if ((answer == "Y") || (answer == "y"))
                    s = new Storage();
                else break;
            } while (true);
            if (s != null)
               s.Print();

            Console.WriteLine("\nТепер ініціалізований список покупок:");
            s = new Storage(10);
            s.Print();
        }
    }
}
