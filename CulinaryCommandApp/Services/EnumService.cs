using CulinaryCommandApp.Data.Enums;

namespace CulinaryCommand.Services
{
    public class EnumService
    {
        public List<string> GetCategories()
        {
            return new List<string>
            {
                Category.Produce,
                Category.Dairy,
                Category.Meat,
                Category.DryGoods,
                Category.Beverages,
                Category.Condiments,
                Category.Spices
            };
        }

        public List<string> GetRecipeTypes()
        {
            return new List<string>
            {
                RecipeType.Appetizer,
                RecipeType.Entree,
                RecipeType.Side,
                RecipeType.Dessert,
                RecipeType.Sauce,
                RecipeType.PrepItem
            };
        }

        public List<string> GetUnits() => new()
        {
            Units.Percent,
            Units.Each,
            Units.Grams,
            Units.Kilograms,
            Units.Ounces,
            Units.Pounds,
            Units.Milliliters,
            Units.Liters,
            Units.Teaspoon,
            Units.Tablespoon,
            Units.Cup,
            Units.Quart,
            Units.Gallon,
            Units.Serving
        };
    }
}