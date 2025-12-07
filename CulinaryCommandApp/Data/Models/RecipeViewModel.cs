using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryCommand.Models
{
    public class RecipeViewModel
    {
        public int RecipeId { get; set; }
        public int LocationId { get; set; }

        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public string RecipeType { get; set; } = "";

        public decimal? YieldAmount { get; set; }
        public string? YieldUnit { get; set; }

        public List<RecipeIngredientViewModel> Ingredients { get; set; } = new();
        public List<RecipeStepViewModel> Steps { get; set; } = new();
    }
}