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
    public class SerializableProduct : IProduct, ISerializable, IXmlSerializable
    {
        public string name;
        public decimal price;
        public decimal weight;

        public SerializableProduct() { } // the empty constructor is for XMLSerialization
        public SerializableProduct(string Name, decimal Price, decimal Weight)
        {
            SetProduct(Name, Price, Weight);
        }
        protected SerializableProduct(SerializationInfo info, StreamingContext context) // ISerialized deserialization constructor
        {
            name = info.GetString("name");
            price = info.GetDecimal("price");
            weight = info.GetDecimal("weight");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) // ISerialized serialization method
        {
            info.AddValue("name", name);
            info.AddValue("price", price);
            info.AddValue("weight", weight);
        }
        public XmlSchema GetSchema() { return (null); }

        public void ReadXml(XmlReader reader)
        {
            name = reader["name"];
            price = Decimal.Parse(reader["price"]);
            weight = Decimal.Parse(reader["weight"]);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name", name);
            writer.WriteAttributeString("price", price.ToString());
            writer.WriteAttributeString("weight", weight.ToString());
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