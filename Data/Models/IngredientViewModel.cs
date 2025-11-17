using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryCommand.Models
{
    public class IngredientViewModel
    {
        public int IngredientId { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public string DefaultUnit { get; set; } = "";
    }
}