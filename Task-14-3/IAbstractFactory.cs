using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_14_3
{
    // Factories name space

    public interface IAbstractFactory
    {
        public string Name { get; set; }
        IFoodProduct CreateFoodProduct();
        IIndustrialProduct CreateIndustrialProduct();
    }

    class NovaLiniaFactory : IAbstractFactory  // here we create only food type products
                                         // but the pattern may also allow both industrial
                                         // and food products in both factories
    {
        public string Name { get; set; }
        public NovaLiniaFactory() => Name = "Нова лiнiя";

        public IFoodProduct CreateFoodProduct()
        {
            switch (DateTime.Now.Millisecond % 2)
            {
                case 0:  return new MeatProduct();
                case 1:  return new DairyProduct();
                default: return null;
            }
        }

        public IIndustrialProduct CreateIndustrialProduct()
        {
            return new VirtualProduct();
        }
    }

    class EpicentrFactory : IAbstractFactory
    {
        public string Name { get; set; }
        public EpicentrFactory() => Name = "Епiцентр-К";

        public IFoodProduct CreateFoodProduct()
        {
            return new FoodProduct();
        }

        public IIndustrialProduct CreateIndustrialProduct()
        {
            switch (DateTime.Now.Millisecond % 2)
            {
                case 0:  return new TechnicProduct();
                case 1:  return new HouseholdChemicalProduct();
                default: return null;
            }
        }
    }
}
