using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_9
{
    internal class ProductNotFound : Exception
    {
        private string? _productName;

        public ProductNotFound(string productName) : base("A product not found in the pricecurrent")
        { 
            _productName = productName;
        }

        public string GetProductName()
        {
            return _productName;
        }

    }
}
