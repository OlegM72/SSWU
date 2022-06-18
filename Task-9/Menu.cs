using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_9
{
    internal class Menu
    {
        private List<Dish> _dishes;

        public Dish this[int index] => _dishes[index]; // = { get { return _dishes[index]; } }

        public int Length => _dishes.Count;

        public Menu()
        {
            _dishes = new List<Dish>();
        }

        public Menu(List<Dish> dishes)
        {
            _dishes = dishes;
        }

        public Menu(string fileName) : this() // the Menu() constructor will be called before this method
        {
            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException();
                string? line;
                bool firstLineOfDish = true;
                Dish currentDish = null;
                using (StreamReader reader = new(fileName, Encoding.UTF8))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Length != 0 && line[0] == '*') // skip a comment
                            continue;
                        // Menus.txt should have such lines: Dish name / Ingredient, weight in grams (several lines) / Empty line
                        
                        if (firstLineOfDish)
                        {
                            currentDish = new Dish(line, new Dictionary<string, decimal>()); // Dish name = line
                            firstLineOfDish = false;
                        }
                        else
                        {
                            if (line.Length == 0)
                            {
                                if (currentDish != null)
                                    _dishes.Add(currentDish);
                                currentDish = null;
                                firstLineOfDish = true;
                            }
                            else
                            {
                                int idx;
                                if ((idx = line.LastIndexOf(',')) < 0)
                                    continue; // error in the line, we just skip it
                                if (line[idx + 1] != ' ') // ", " needed. If not, the format is incorrect
                                    continue; // error in the line, we just skip it
                                              // (including cases if a weight not specified or a format is incorrect)
                                string productName = line.Substring(0, idx);
                                if (!decimal.TryParse(line.Substring(idx + 2), out decimal productWeight))
                                    continue; // error in the format, we just skip the line, no exception needed at this stage
                                currentDish.AddIngredient(productName, productWeight); // only one line is allowed for each product
                            }

                        }
                    }
                }
                if (currentDish != null) // this is for the case if there is no empty line in the end of the file
                    _dishes.Add(currentDish);
            }
            catch (Exception ex)
            {
                // any formatting or file error goes to Main method
                throw new Exception("Menu file: " + ex.Message);
            }
        }

        public override string ToString()
        {
            string result = "";
            foreach (Dish dish in _dishes)
            {
                result += ("Dish: " + dish.ToString() + "\r\n");
            }
            return result + "\r\n";
        }
    }
}
