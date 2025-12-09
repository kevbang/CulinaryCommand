using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryCommand.PurchaseOrder.Entities;

public class Supplier
{
    [Key]
    public int SupplierId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? ContactPerson { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
