using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_14_3
{
    public interface IProduct // base interface for all types of products
    {
        static string name;

        static decimal price;

        public void PriceChange(int percent); // зміна ціни на задану кількість відсотків (+/-)
            
        public string GetName() => name;
        public decimal GetPrice() => Math.Round(price, 2);
    }
}
