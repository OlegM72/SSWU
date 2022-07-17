using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_14_2
{
    public interface IProduct
    {
        static string name;
        static decimal price;

        public void PriceChange(int percent); // зміна ціни на задану кількість відсотків (+/-)
        public string GetName();
        public decimal GetPrice();
    }
}
