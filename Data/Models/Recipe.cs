using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace CulinaryCommand.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string Restaurant { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string RecipeType { get; set; }
        public string Cuisine { get; set; }
        public Yield Yield { get; set; }
        public Portion Portion { get; set; }
        public Times Times { get; set; }
        public string Station { get; set; }
        public string Par { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Step> Steps { get; set; }
        public string Notes { get; set; }
        public List<string> Allergens { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Equipment { get; set; }
        public Storage Storage { get; set; }
        public Costing Costing { get; set; }
        public Image Image { get; set; }
        public Source Source { get; set; }
    }

    public class Yield
    {
        public double? Amount { get; set; }
        public string Unit { get; set; }
    }

    public class Portion
    {
        public double? Size { get; set; }
        public string Unit { get; set; }
    }

    public class Times
    {
        public int? PrepMinutes { get; set; }
        public int? CookMinutes { get; set; }
        public int? TotalMinutes { get; set; }
    }

    public class Ingredient
    {
        public Guid IngredientId { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public string PrepNote { get; set; }
    }

    public class Step
    {
        public int Order { get; set; }
        public string Instruction { get; set; }
    }

    public class Storage
    {
        public int? ShelfLifeDays { get; set; }
        public double? HoldTempF { get; set; }
        public string Container { get; set; }
    }

    public class Costing
    {
        public double? BatchCost { get; set; }
        public double? PortionCost { get; set; }
    }

    public class Image
    {
        public string Url { get; set; }
        public string Id { get; set; }
    }

    public class Source
    {
        public string Pdf { get; set; }
        public int? Page { get; set; }
    }
}
