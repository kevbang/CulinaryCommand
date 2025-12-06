using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryCommand.Models
{
    public class RecipeStepViewModel
    {
        public int StepNumber { get; set; }
        public string Instructions { get; set; } = "";
    }
}