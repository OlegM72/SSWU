using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Task_14_2
{
    [Serializable]
    public class SerializableStorage : ISerializable, IXmlSerializable
    {
        public int s_count = 0;
        public decimal s_TotalPrice;
        public decimal s_TotalWeight;
        
        public List<IProduct> products;

        public SerializableStorage() // створення порожнього складу, the empty constructor also is needed for XMLSerialization
        {
            products = new List<IProduct>();
            s_count = 0;
            s_TotalPrice = 0;
            s_TotalWeight = 0;
        }

        protected SerializableStorage(SerializationInfo info, StreamingContext context) // ISerialized deserialization constructor
        {
            s_count = info.GetInt32("s_count");
            s_TotalPrice = info.GetDecimal("s_TotalPrice");
            s_TotalWeight = info.GetDecimal("s_TotalWeight");
            products = new List<IProduct>();
            foreach (var item in info)
            {
                string typeStrCut = item.ObjectType.Name;
                if (typeStrCut == "SerializableProduct" || 
                    typeStrCut == "SerializableDairyProduct" || 
                    typeStrCut == "SerializableMeat")
                  AddProduct(info.GetValue(item.Name, item.ObjectType) as IProduct);
            }

        }
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) // ISerialized serialization method
        {
            info.AddValue("s_count", s_count);
            info.AddValue("s_TotalPrice", s_TotalPrice);
            info.AddValue("s_TotalWeight", s_TotalWeight);
            int productNumber = 0;
            foreach (IProduct product in products)
            {
                string typeStr = $"product #{productNumber}: {product.GetType()}";
                info.AddValue(typeStr, product);
                productNumber++;
            }
        }

        public XmlSchema GetSchema() { return (null); }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("s_count", s_count.ToString());
            writer.WriteAttributeString("s_TotalPrice", s_TotalPrice.ToString());
            writer.WriteAttributeString("s_TotalWeight", s_TotalWeight.ToString());
            writer.WriteAttributeString("products", ProductsToSerializableString());
        }

        public void ReadXml(XmlReader reader)
        {
            s_count = Int32.Parse(reader["s_count"]);
            s_TotalPrice = Decimal.Parse(reader["s_TotalPrice"]);
            s_TotalWeight = Decimal.Parse(reader["s_TotalWeight"]);
            products = new List<IProduct>();
            string[] productLines = reader["products"].Split("\\", StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in productLines)
                ParseAndAddProduct(line, false);
        }

        public string ProductsToSerializableString()
        {
            string result = "";
            foreach (IProduct product in products)
            {
                if (product == null)
                    throw new ArgumentException($"Error: A product is empty or the product link is wrong");
                // Name; Price; Weight; 0 or meat category 1 - 3; meat type 1 - 4 or 0 for other[; expiration date]
                result += $"{product.GetName()}; {product.GetPrice()}; {((SerializableProduct)product).GetWeight()}; ";
                if (product.GetType() == typeof(SerializableMeat))
                    result += $"{(int)((SerializableMeat)product).GetCategory()}; {(int)((SerializableMeat)product).GetMeatType()}; ";
                else
                    result += $"0; 0; ";
                if (product.GetType() == typeof(SerializableMeat) ||
                    product.GetType() == typeof(SerializableDairyProduct))
                    result += $"{((SerializableDairyProduct)product).GetDuedate():d}";
                result += "\\";
            }
            return result;
        }


        public void AddProduct(IProduct product) // a template for the future method of adding a product
        {
            if (products is null)
                products = new();
            products.Add(product);
        }

        public SerializableStorage(StreamReader reader) // читання колекції товарів з текстового файлу
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
                        ParseAndAddProduct(line, true);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error (line {currLine}): " + ex.Message);
                    }
                }
            } // while (!EOF)
        }

        private void ParseAndAddProduct(string line, bool changeTotals)
        {
            try
            {
                // Name; Price; Weight; 0 or meat category 1-3; meat type 1-4 or 0 for other;
                // expiration date (days from now)
                string[] words = line.Split(';', StringSplitOptions.RemoveEmptyEntries);
                string name = char.ToUpper(words[0][0]) + words[0][1..]; // uppercase the first char
                decimal price = Convert.ToDecimal(words[1], new CultureInfo("en-US")); // if error, there will be an exception
                decimal weight = Convert.ToDecimal(words[2], new CultureInfo("en-US")); // the same :)
                int catstr = Convert.ToInt32(words[3]);
                SerializableMeat.Category cat;
                if (catstr > 3 || catstr < 0)
                    catstr = 0;
                switch (catstr)
                {
                    case 1:
                        cat = SerializableMeat.Category.HighSort;
                        break;
                    case 2:
                        cat = SerializableMeat.Category.Sort1;
                        break;
                    case 3:
                        cat = SerializableMeat.Category.Sort2;
                        break;
                    default:
                        cat = SerializableMeat.Category.NotMeat;
                        break;
                }
                SerializableMeat.MeatType type = 0;
                if (cat != SerializableMeat.Category.NotMeat)
                {
                    int typestr = Convert.ToInt32(words[4]);
                    if (typestr > 4 || typestr < 1)
                        typestr = 0;
                    switch (typestr)
                    {
                        case 1:
                            type = SerializableMeat.MeatType.Baran; // баранина
                            break;
                        case 2:
                            type = SerializableMeat.MeatType.Telia; // телятина
                            break;
                        case 3:
                            type = SerializableMeat.MeatType.Svini; // свинина
                            break;
                        case 4:
                            type = SerializableMeat.MeatType.Kurcha; // курятина
                            break;
                        default:
                            type = SerializableMeat.MeatType.Other; // other dairy product
                            break;
                    }
                }
                DateTime due = DateTime.MinValue;
                if (words.Length < 6 || words[5] == " ") // empty date - maybe not a DairyProduct
                {
                    if (cat != SerializableMeat.Category.NotMeat)
                        throw new ArgumentException($"Wrong format of the date for the product {name}");
                }
                else
                    if (!DateTime.TryParse(words[5], out due))
                    throw new ArgumentException($"Wrong format of the date for the product {name}");
                if (products != null)
                {
                    SerializableProduct nextProduct;
                    if (cat == SerializableMeat.Category.NotMeat)
                    {
                        if (due == DateTime.MinValue)
                            nextProduct = new SerializableProduct(name, price, weight);
                        else
                            nextProduct = new SerializableDairyProduct(due, name, price, weight);
                    }
                    else
                        nextProduct = new SerializableMeat(due, cat, type, name, price, weight);
                    if (changeTotals)  // used only when reading from the file, not during deserialization
                        nextProduct.PriceChange(0); // встановлення нової ціни згідно сорту та типу м'яса і строку придатності
                    products.Add(nextProduct);
                    if (changeTotals)
                    {
                        s_TotalPrice += nextProduct.GetPrice();
                        s_TotalWeight += nextProduct.GetWeight();
                        s_count++;
                    }
                }
            } // try
            catch { throw; }
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
            foreach (SerializableProduct product in products)
                if (product is SerializableMeat)
                    meatCount++;
            return meatCount;
        }

        public override string ToString()
        {
            string result = "";
            int i = 1;
            foreach (SerializableProduct product in products)
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