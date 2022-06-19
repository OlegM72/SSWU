using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_9
{
    internal class PriceCurrent
    {
        private Dictionary<string, decimal> _productPrices; // Pairs of: Product name, Price for 1 kg in UAH
        private string currentFileName; // for appending

        public PriceCurrent()
        {
            _productPrices = new();
        }
        public PriceCurrent(Dictionary<string, decimal> productPrices)
        {
            _productPrices = productPrices;
        }

        public PriceCurrent(string fileName) : this() // the PriceCurrent() constructor will be called before this method
        {
            try
            {
                string? line;
                currentFileName = fileName;
                using (StreamReader reader = new(fileName, Encoding.UTF8))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Length != 0 && line[0] == '*') // skip a comment
                            continue;
                        // Prices.txt line is like this: Product name - Price for 1 kg in UAH
                        int idx;
                        if ((idx = line.LastIndexOf('-')) < 0) // search the last "-", others may be inside the product name
                            continue; // error in the line, we just skip it
                        if (line[idx-1] != ' ' || line[idx + 1] != ' ') // " - " needed
                            continue; // error in the line, we just skip it
                                      // (including cases if a price not specified or a format is incorrect)
                        string productName = line.Substring(0, idx - 1);
                        if (!decimal.TryParse(line.Substring(idx + 2), out decimal productPrice))
                            continue; // error in the format, we just skip the line, no exception needed at this stage
                        _productPrices.Add(productName, productPrice); // only one line is allowed for each product
                    }
                }
            }
            catch (Exception ex)
            {
                // any file error goes to Main method
                throw new Exception("Prices file: " + ex.Message);
            }
        }

        private void AppendToPriceCurrent(string productName, decimal price)
        {
            // appending to the collection
            _productPrices.Add(productName, price);
            // appending to the file
            using (StreamWriter writer = new(currentFileName, true, Encoding.UTF8)) // append mode
            {
                writer.WriteLine($"{productName} - {price:f2}");
            }
        }

        public bool TryGetProductPrice(string productName, out decimal price)
        {
            try
            {
                if (!_productPrices.TryGetValue(productName, out decimal result))
                {
                    price = 0.0m;
                    throw new ProductNotFound(productName);
                }
                price = result;
                return true;
            }
            catch (ProductNotFound ex)
            {
                Console.WriteLine("Enter price (in UAH) for the product: " + ex.GetProductName());
                bool OK = decimal.TryParse(Console.ReadLine(), out decimal productPrice);
                if (!OK)
                    throw; // go to the Main method to terminate the program
                AppendToPriceCurrent(productName, productPrice);
                price = productPrice;
                return true;
            }
        }

        public string PrintPrices(Course course)
        {
            string result = "";
            foreach (KeyValuePair<string, decimal> product in _productPrices)
            {
                // this is internal function
                result += 
                    ($"1 kg of {product.Key} costs " + 
                     $"{(product.Value / course.GetCourse(Course.currentCurrency)):f2} {Course.currentCurrency}\r\n");
            }
            return result;
        }


        public override string ToString() // out the prices of the products in UAH
        {
            string result = "";
            foreach (KeyValuePair<string, decimal> product in _productPrices)
            {
                // this is internal function
                result += $"1 kg of {product.Key} costs {product.Value} UAH\r\n";
            }
            return result;
        }

    }
}
