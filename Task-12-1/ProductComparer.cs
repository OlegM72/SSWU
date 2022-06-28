using System;
using System.Collections.Generic;
using System.IO;

namespace Task_12_1
{
    public class ProductComparer : IComparer<Product>
    {
        public int Compare(Product product1, Product product2) // we need only equality comparing in fact
        {
            return product1.GetHashCode().CompareTo(product2.GetHashCode());
        }
    }
}