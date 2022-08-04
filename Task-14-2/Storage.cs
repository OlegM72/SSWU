using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;

namespace Task_14_2
{
    [KnownType(typeof(Meat))]
    [KnownType(typeof(Product))]
    [KnownType(typeof(DairyProduct))]
    [DataContract]
    [Serializable]
    public class Storage
    {
        [DataMember]
        public int s_count = 0;
        [DataMember]
        public decimal s_TotalPrice;
        [DataMember]
        public decimal s_TotalWeight;

        [DataMember]
        public List<IProduct> products;

        public Storage() // створення порожнього складу
        {
            products = new List<IProduct>();
            s_count = 0;
            s_TotalPrice = 0;
            s_TotalWeight = 0;
        }

        public void AddProduct(Product product) // a template for the future method of adding a product
        {
            products.Add(product);
        }

        public Storage(StreamReader reader) // читання колекції товарів з текстового файлу
        {
            products = new List<IProduct>();
            s_count = 0;
            s_TotalPrice = 0;
            s_TotalWeight = 0;
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
                        string[] words = line.Split("; ", StringSplitOptions.RemoveEmptyEntries);
                        string name = char.ToUpper(words[0][0]) + words[0][1..]; // uppercase the first char
                        decimal price = Convert.ToDecimal(words[1], new CultureInfo("en-US")); // if error, there will be an exception
                        decimal weight = Convert.ToDecimal(words[2], new CultureInfo("en-US")); // the same :)
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
    }
}