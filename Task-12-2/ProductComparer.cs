using System;
using System.Collections.Generic;
using System.IO;

namespace Task_12_2
{
    public class ProductHashComparer : IComparer<Product> // hash codes comparator 
    {
        public int Compare(Product product1, Product product2) // we need only equality comparing in fact
        {
            return product1.GetHashCode().CompareTo(product2.GetHashCode());
        }
    }

    public class ProductNameComparer : IComparer<Product> // names comparator
    {
        public int Compare(Product product1, Product product2)
        {
            return product1.GetName().CompareTo(product2.GetName());
        }
    }

    public class ProductPriceComparer : IComparer<Product> // prices comparator
    {
        public int Compare(Product product1, Product product2)
        {
            return product1.GetPrice().CompareTo(product2.GetPrice());
        }
    }

    public class ProductWeightComparer : IComparer<Product> // weights comparator
    {
        public int Compare(Product product1, Product product2)
        {
            return product1.GetWeight().CompareTo(product2.GetWeight());
        }
    }

}