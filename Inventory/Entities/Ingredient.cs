using System;

namespace CulinaryCommand.Inventory.Entities
{
    public class Ingredient
    {
        public int Id {get; set;}

        // display name of the ingredient (ex: "flour", "eggs")
        public string Name { get; set; } = string.Empty;

        // fk to the unit used to track stock (ex: g, kg, each).
        public int UnitId { get; set; }

        // how much is currently in inventory, stored in the ingredient's base unit
        public decimal StockQuantity { get; set; }

        // timestamp when the ingredient was created
        public DateTime? CreatedAt { get; set; }

        // optional timestamp when the ingredient was last updated
        public DateTime? UpdatedAt { get; set; }

        // the unit in which this ingredient's stock is tracked
        public Unit? Unit { get; set; }

        // category for the ingredient (ex: "dairy", "baking")
        public string Category { get; set; } = string.Empty;
    }
    
}