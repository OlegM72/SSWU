using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Task_14_2
{
    [Serializable]
    public class SerializableMeat : SerializableDairyProduct, IProduct, ISerializable, IXmlSerializable
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

        public MeatType type { get; set; }
        public Category category { get; set; }

        public SerializableMeat() : base() { } // the empty constructor is for XMLSerialization

        public SerializableMeat(DateTime due, Category cat, MeatType typ, string nam, decimal pric, decimal weig) :
                    base(due, nam, pric, weig)
        {
            category = cat;
            type = typ;
        }

        protected SerializableMeat(SerializationInfo info, StreamingContext context) // ISerialized deserialization constructor
              : base(info, context)
        {
            type = (MeatType)info.GetValue("type", typeof(MeatType));
            category = (Category)info.GetValue("category", typeof(Category));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) // ISerialized serialization method
        {
            base.GetObjectData(info, context);
            info.AddValue("type", type);
            info.AddValue("category", category);
        }

        public XmlSchema GetSchema() { return (null); }

        public void ReadXml(XmlReader reader)
        {
            category = (Category)Enum.Parse(typeof(Category), reader["category"]);
            type = (MeatType)Enum.Parse(typeof(MeatType), reader["type"]);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("category", category.ToString());
            writer.WriteAttributeString("type", type.ToString());
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
}