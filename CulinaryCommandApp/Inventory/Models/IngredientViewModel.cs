using System.ComponentModel.DataAnnotations;

namespace CulinaryCommand.Inventory.Models
{
    public class IngredientViewModel
    {
        // Use IngredientId to match existing DB column mapping, or change everywhere to Id.
        public int IngredientId { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        // Abbreviation (ex: "g", "kg") for display
        public string DefaultUnit { get; set; } = string.Empty;

        // Useful for form binding to a unit dropdown
        public int? DefaultUnitId { get; set; }

        // Optional starting stock value shown/edited in the UI
        [Range(0, double.MaxValue)]
        public decimal? StockQuantity { get; set; }
    }
}