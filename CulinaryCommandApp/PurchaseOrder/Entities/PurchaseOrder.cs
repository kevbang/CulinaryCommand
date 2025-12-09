using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinaryCommand.Data.Entities;

namespace CulinaryCommand.PurchaseOrder.Entities;

public class PurchaseOrder
{
    [Key]
    public int PurchaseOrderId { get; set; }

    [Required]
    [StringLength(50)]
    public string Reference { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public int SupplierId { get; set; }

    [ForeignKey(nameof(SupplierId))]
    public virtual Supplier? Supplier { get; set; }

    [StringLength(100)]
    public string? SupplierReference { get; set; }

    [StringLength(50)]
    public string? ProjectCode { get; set; }

    [StringLength(10)]
    public string Currency { get; set; } = "USD";

    public DateTime? StartDate { get; set; }

    public DateTime? TargetDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Placed, Complete, Requires Approval, Cancelled

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    public int? LocationId { get; set; }

    [ForeignKey(nameof(LocationId))]
    public virtual Location? Location { get; set; }

    [StringLength(500)]
    public string? Link { get; set; }

    public int? ResponsibleUserId { get; set; }

    [ForeignKey(nameof(ResponsibleUserId))]
    public virtual User? ResponsibleUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
}
