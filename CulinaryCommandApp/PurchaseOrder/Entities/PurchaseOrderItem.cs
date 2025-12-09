using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinaryCommand.Data.Entities;

namespace CulinaryCommand.PurchaseOrder.Entities;

public class PurchaseOrderItem
{
    [Key]
    public int PurchaseOrderItemId { get; set; }

    [Required]
    public int PurchaseOrderId { get; set; }

    [ForeignKey(nameof(PurchaseOrderId))]
    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    public int? IngredientId { get; set; }

    [ForeignKey(nameof(IngredientId))]
    public virtual Ingredient? Ingredient { get; set; }

    [Required]
    [StringLength(200)]
    public string ItemName { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [StringLength(50)]
    public string Unit { get; set; } = "units";

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal QuantityReceived { get; set; } = 0;

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Received, Partial

    public DateTime? ReceivedDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
