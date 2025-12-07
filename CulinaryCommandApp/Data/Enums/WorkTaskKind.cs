using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryCommand.Data.Enums
{
    public enum WorkTaskKind
    {
        Generic = 0,       // clean fryer, sweep walk-in, etc.
        PrepFromRecipe = 1 // make food based on recipe/ingredients
    }
}