using CulinaryCommand.PurchaseOrder.DTOs;

namespace CulinaryCommand.PurchaseOrder.Services;

public interface IPurchaseOrderService
{
    Task<List<PurchaseOrderDto>> GetAllPurchaseOrdersAsync(int? locationId = null);
    Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(int purchaseOrderId);
    Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request);
    Task<bool> UpdatePurchaseOrderAsync(int purchaseOrderId, CreatePurchaseOrderRequest request);
    Task<bool> UpdatePurchaseOrderStatusAsync(int purchaseOrderId, string status);
    Task<bool> DeletePurchaseOrderAsync(int purchaseOrderId);
    Task<List<SupplierDto>> GetAllSuppliersAsync();
    Task<SupplierDto> CreateSupplierAsync(SupplierDto supplier);
}
