using System;
using System.Collections.Generic;
using System.IO;

namespace Task_12_2
{
    public delegate int Comparator(Product product1, Product product2);

    public class ProductComparer : IComparer<Product> 
    {
        
        Comparator currentCompareMethod = null;

        public ProductComparer()
        {
            SetCompareMethod(ComparerType.Hash);
        }

        public enum ComparerType
        {
            Hash = 0,
            Name,
            Price,
            Weight
        }

        // choosing a delegate for comparing
        private Comparator GetCompareMethod(ComparerType type) => type switch
        {
            ComparerType.Hash   => HashCompare,
            ComparerType.Name   => NameCompare,
            ComparerType.Price  => PriceCompare,
            ComparerType.Weight => WeightCompare,
            _ => throw new ArgumentException($"Wrong comparer type: {type}"),
        };

        public void SetCompareMethod(ComparerType type)
        {
            currentCompareMethod = GetCompareMethod(type);
        }

        // common method for IComparer Interface - called by Sort methods
        public int Compare(Product product1, Product product2) 
        {
            return currentCompareMethod(product1, product2);
        }

        // hash codes comparator - if we need only equality comparing
        public int HashCompare(Product product1, Product product2)
        {
            return product1.GetHashCode().CompareTo(product2.GetHashCode());
        }
    
        // names comparator
        public int NameCompare(Product product1, Product product2)
        {
            return product1.GetName().CompareTo(product2.GetName());
        }

        // prices comparator
        public int PriceCompare(Product product1, Product product2)
        {
            return product1.GetPrice().CompareTo(product2.GetPrice());
        }

        // weights comparator
        public int WeightCompare(Product product1, Product product2)
        {
            return product1.GetWeight().CompareTo(product2.GetWeight());
        }
    }
}