using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryCommand.Models
{
    public class RecipeIngredientViewModel
    {
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public int UnitId { get; set; }
        public string? PrepNote { get; set; }
    }
}