using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task13
{
    internal static class PersonsParser
    {

        public static Person Parse(string text)
        {
            Random random = new Random();
            string[] atributes = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    
            return new Person(
                 (PersonStatus)PersonStatus.Parse(typeof(PersonStatus), atributes[0]), // status
                 atributes[1],               // name, not related to Id already
                 int.Parse(atributes[4]),    // age
                 double.Parse(atributes[3]), // coordinate
                 int.Parse(atributes[2]));   // serviceTime
        }
    }
}
