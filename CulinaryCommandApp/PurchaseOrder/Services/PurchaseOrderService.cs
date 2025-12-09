using Microsoft.EntityFrameworkCore;
using CulinaryCommand.Data;
using CulinaryCommand.PurchaseOrder.DTOs;
using CulinaryCommand.PurchaseOrder.Entities;
using CulinaryCommand.Data.Entities;

namespace CulinaryCommand.PurchaseOrder.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly AppDbContext _context;

    public PurchaseOrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PurchaseOrderDto>> GetAllPurchaseOrdersAsync(int? locationId = null)
    {
        var query = _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Location)
            .Include(po => po.ResponsibleUser)
            .Include(po => po.Items)
            .AsQueryable();

        if (locationId.HasValue)
        {
            query = query.Where(po => po.LocationId == locationId.Value);
        }

        var orders = await query.OrderByDescending(po => po.CreatedAt).ToListAsync();

        return orders.Select(po => new PurchaseOrderDto
        {
            PurchaseOrderId = po.PurchaseOrderId,
            Reference = po.Reference,
            Description = po.Description,
            SupplierId = po.SupplierId,
            SupplierName = po.Supplier?.Name,
            SupplierReference = po.SupplierReference,
            ProjectCode = po.ProjectCode,
            Currency = po.Currency,
            StartDate = po.StartDate,
            TargetDate = po.TargetDate,
            CompletionDate = po.CompletionDate,
            Status = po.Status,
            TotalPrice = po.TotalPrice,
            LocationId = po.LocationId,
            LocationName = po.Location?.LocationName,
            Link = po.Link,
            ResponsibleUserId = po.ResponsibleUserId,
            ResponsibleUserName = po.ResponsibleUser != null 
                ? $"{po.ResponsibleUser.FirstName} {po.ResponsibleUser.LastName}" 
                : null,
            CreatedAt = po.CreatedAt,
            UpdatedAt = po.UpdatedAt,
            TotalItems = po.Items.Count,
            ReceivedItems = po.Items.Count(i => i.Status == "Received")
        }).ToList();
    }

    public async Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(int purchaseOrderId)
    {
        var po = await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Location)
            .Include(po => po.ResponsibleUser)
            .Include(po => po.Items)
            .FirstOrDefaultAsync(po => po.PurchaseOrderId == purchaseOrderId);

        if (po == null) return null;

        return new PurchaseOrderDto
        {
            PurchaseOrderId = po.PurchaseOrderId,
            Reference = po.Reference,
            Description = po.Description,
            SupplierId = po.SupplierId,
            SupplierName = po.Supplier?.Name,
            SupplierReference = po.SupplierReference,
            ProjectCode = po.ProjectCode,
            Currency = po.Currency,
            StartDate = po.StartDate,
            TargetDate = po.TargetDate,
            CompletionDate = po.CompletionDate,
            Status = po.Status,
            TotalPrice = po.TotalPrice,
            LocationId = po.LocationId,
            LocationName = po.Location?.LocationName,
            Link = po.Link,
            ResponsibleUserId = po.ResponsibleUserId,
            ResponsibleUserName = po.ResponsibleUser != null 
                ? $"{po.ResponsibleUser.FirstName} {po.ResponsibleUser.LastName}" 
                : null,
            CreatedAt = po.CreatedAt,
            UpdatedAt = po.UpdatedAt,
            TotalItems = po.Items.Count,
            ReceivedItems = po.Items.Count(i => i.Status == "Received"),
            Items = po.Items.Select(item => new PurchaseOrderItemDto
            {
                PurchaseOrderItemId = item.PurchaseOrderItemId,
                PurchaseOrderId = item.PurchaseOrderId,
                IngredientId = item.IngredientId,
                ItemName = item.ItemName,
                Description = item.Description,
                Quantity = item.Quantity,
                Unit = item.Unit,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.TotalPrice,
                QuantityReceived = item.QuantityReceived,
                Status = item.Status,
                ReceivedDate = item.ReceivedDate
            }).ToList()
        };
    }

    public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request)
    {
        var purchaseOrder = new PurchaseOrder
        {
            Reference = request.Reference,
            Description = request.Description,
            SupplierId = request.SupplierId,
            SupplierReference = request.SupplierReference,
            ProjectCode = request.ProjectCode,
            Currency = request.Currency,
            StartDate = request.StartDate,
            TargetDate = request.TargetDate,
            LocationId = request.LocationId,
            Link = request.Link,
            ResponsibleUserId = request.ResponsibleUserId,
            Status = "Pending",
            TotalPrice = 0,
            CreatedAt = DateTime.UtcNow
        };

        _context.PurchaseOrders.Add(purchaseOrder);
        await _context.SaveChangesAsync();

        return (await GetPurchaseOrderByIdAsync(purchaseOrder.PurchaseOrderId))!;
    }

    public async Task<bool> UpdatePurchaseOrderAsync(int purchaseOrderId, CreatePurchaseOrderRequest request)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(purchaseOrderId);
        if (purchaseOrder == null) return false;

        purchaseOrder.Reference = request.Reference;
        purchaseOrder.Description = request.Description;
        purchaseOrder.SupplierId = request.SupplierId;
        purchaseOrder.SupplierReference = request.SupplierReference;
        purchaseOrder.ProjectCode = request.ProjectCode;
        purchaseOrder.Currency = request.Currency;
        purchaseOrder.StartDate = request.StartDate;
        purchaseOrder.TargetDate = request.TargetDate;
        purchaseOrder.LocationId = request.LocationId;
        purchaseOrder.Link = request.Link;
        purchaseOrder.ResponsibleUserId = request.ResponsibleUserId;
        purchaseOrder.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdatePurchaseOrderStatusAsync(int purchaseOrderId, string status)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(purchaseOrderId);
        if (purchaseOrder == null) return false;

        purchaseOrder.Status = status;
        if (status == "Complete")
        {
            purchaseOrder.CompletionDate = DateTime.UtcNow;
        }
        purchaseOrder.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePurchaseOrderAsync(int purchaseOrderId)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .Include(po => po.Items)
            .FirstOrDefaultAsync(po => po.PurchaseOrderId == purchaseOrderId);
            
        if (purchaseOrder == null) return false;

        _context.PurchaseOrders.Remove(purchaseOrder);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<SupplierDto>> GetAllSuppliersAsync()
    {
        var suppliers = await _context.Suppliers
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync();

        return suppliers.Select(s => new SupplierDto
        {
            SupplierId = s.SupplierId,
            Name = s.Name,
            ContactPerson = s.ContactPerson,
            Email = s.Email,
            Phone = s.Phone,
            Address = s.Address,
            IsActive = s.IsActive
        }).ToList();
    }

    public async Task<SupplierDto> CreateSupplierAsync(SupplierDto supplierDto)
    {
        var supplier = new Supplier
        {
            Name = supplierDto.Name,
            ContactPerson = supplierDto.ContactPerson,
            Email = supplierDto.Email,
            Phone = supplierDto.Phone,
            Address = supplierDto.Address,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        supplierDto.SupplierId = supplier.SupplierId;
        return supplierDto;
    }
}
