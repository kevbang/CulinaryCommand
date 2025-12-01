using System.ComponentModel.DataAnnotations;

namespace CulinaryCommand.Inventory.DTOs
{
    public class CreateIngredientDTO
    {
        [Required]
        public string Name { get; set; } = default!;
        public string? SKU { get; set; }
        public decimal CurrentQuantity { get; set; } = 0;
        public decimal Price { get; set; } = 0m;
        public string? Category { get; set; }
        // used to determine if restock is needed
        public decimal ReorderLevel { get; set; } = 0m;
        // Use an integer Id to reference the Unit (ex: a foreign key to the Units table)
        [Range(1, int.MaxValue, ErrorMessage = "UnitId must be a positive integer.")]
        public int UnitId { get; set; }
    }
}