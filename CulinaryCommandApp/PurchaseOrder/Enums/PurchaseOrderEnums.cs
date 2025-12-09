namespace CulinaryCommand.PurchaseOrder.Enums;

public enum PurchaseOrderStatus
{
    Pending,
    Placed,
    Complete,
    RequiresApproval,
    Cancelled
}

public enum PurchaseOrderItemStatus
{
    Pending,
    Received,
    Partial
}
