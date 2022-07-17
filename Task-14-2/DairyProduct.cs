using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Task_14_2
{
    [DataContract]
    [Serializable]
    public class DairyProduct : Product, IProduct
    {
        [DataMember]
        public DateTime duedate { get; set; } // термін придатності

        public DairyProduct(DateTime due, string nam, decimal pric, decimal weig) :
                                         base(nam, pric, weig)
        {
            duedate = due;
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