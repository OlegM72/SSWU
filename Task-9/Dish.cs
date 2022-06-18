using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_9
{
    internal class Dish
    {
        private string _dishName;
        private Dictionary<string, decimal> _ingredients; // Pairs of product name and its weight in grams

        public decimal this[string key] => _ingredients[key]; // = { get { return _ingredients[key] } } - return weight

        public IEnumerable<string> keys => _ingredients.Keys;

        public int Length => _ingredients.Count;

        public Dish()
        {
            _ingredients = new();
        }

        public Dish(string dishName, Dictionary<string, decimal> ingredients)
        {
            _dishName = dishName;
            _ingredients = ingredients;
        }
        
        public void AddIngredient(string productName, decimal weight)
        {
            _ingredients.Add(productName, weight);
        }

        public bool TryGetPrice(PriceCurrent priceCurrent, out decimal sumPrice)
        {
            sumPrice = 0.0m;
            foreach (KeyValuePair<string, decimal> ingredient in _ingredients)
            {
                if (!priceCurrent.TryGetProductPrice(ingredient.Key, out decimal productPrice))
                {
                    return false;
                }
                sumPrice += productPrice * ingredient.Value;
            }
            return true;
        }

        public string GetDishName()
        { 
            return _dishName;
        }

        public override string ToString()
        {
            string result = _dishName + "\r\n";
            foreach (KeyValuePair<string, decimal> product in _ingredients)
            {
                result += $"  {product.Key} - {product.Value} grams\r\n";
            }
            return result;
        }
    }
}
