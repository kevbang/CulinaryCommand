namespace CulinaryCommand.PurchaseOrder.DTOs;

public class PurchaseOrderDto
{
    public int PurchaseOrderId { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? SupplierReference { get; set; }
    public string? ProjectCode { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime? StartDate { get; set; }
    public DateTime? TargetDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string Status { get; set; } = "Pending";
    public decimal TotalPrice { get; set; }
    public int? LocationId { get; set; }
    public string? LocationName { get; set; }
    public string? Link { get; set; }
    public int? ResponsibleUserId { get; set; }
    public string? ResponsibleUserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int TotalItems { get; set; }
    public int ReceivedItems { get; set; }
    public List<PurchaseOrderItemDto> Items { get; set; } = new();
}

public class PurchaseOrderItemDto
{
    public int PurchaseOrderItemId { get; set; }
    public int PurchaseOrderId { get; set; }
    public int? IngredientId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = "units";
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal QuantityReceived { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime? ReceivedDate { get; set; }
}

public class SupplierDto
{
    public int SupplierId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreatePurchaseOrderRequest
{
    public string Reference { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SupplierId { get; set; }
    public string? SupplierReference { get; set; }
    public string? ProjectCode { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime? StartDate { get; set; }
    public DateTime? TargetDate { get; set; }
    public int? LocationId { get; set; }
    public string? Link { get; set; }
    public int? ResponsibleUserId { get; set; }
}
