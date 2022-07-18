using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_14_3
{
    public interface IVirtualProduct : IIndustrialProduct // a virtual product (movies, books, files). May have genre and length
    {
        static decimal length;

        static string genre;

        public decimal GetLength() => length;
        public string GetGenre() => genre;
    }

}