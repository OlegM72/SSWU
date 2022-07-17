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
    public class SerializableDairyProduct : SerializableProduct, IProduct, ISerializable, IXmlSerializable
    {
        public DateTime duedate { get; set; } // термін придатності

        public SerializableDairyProduct() : base() { } // the empty constructor is for XMLSerialization
        public SerializableDairyProduct(DateTime due, string nam, decimal pric, decimal weig) :
                                         base(nam, pric, weig)
        {
            duedate = due;
        }

        protected SerializableDairyProduct(SerializationInfo info, StreamingContext context) // ISerialized deserialization constructor
            : base(info, context)
        {
            duedate = info.GetDateTime("duedate");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) // ISerialized serialization method
        {
            base.GetObjectData(info, context);
            info.AddValue("duedate", duedate);
        }

        public XmlSchema GetSchema() { return (null); }

        public void ReadXml(XmlReader reader)
        {
            duedate = DateTime.Parse(reader["duedate"]);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("duedate", duedate.ToString());
        }

        public DateTime GetDuedate()
        {
            return duedate;
        }

        public override void PriceChange(int percent) // зміна ціни на задану кількість відсотків + згідно терміну придатності
        {
            base.PriceChange(percent);
            if (DateTime.Now > GetDuedate())
                base.PriceChange(-100);
            else if (DateTime.Now - GetDuedate() < TimeSpan.FromDays(2))
                base.PriceChange(-30);
        }

        public override int GetHashCode()
        {
            return GetDuedate().GetHashCode() ^ base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() + $", expires {GetDuedate():d}";
        }

    }
}