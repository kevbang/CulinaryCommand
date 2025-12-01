using System.Linq;
using CulinaryCommand.Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CulinaryCommand.Inventory.DTOs;
using CulinaryCommand.Data;
using CulinaryCommand.Inventory.Entities;

namespace CulinaryCommand.Inventory.Services
{
    public class InventoryManagementService : IInventoryManagementService
    {
        private readonly AppDbContext _db;

        public InventoryManagementService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<InventoryItemDTO>> GetAllItemsAsync()
        {
            return await _db.Ingredients
                .Select(ingredient => new InventoryItemDTO
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,
                    Category = ingredient.Category,
                    CurrentQuantity = ingredient.StockQuantity,
                    Unit = ingredient.Unit != null ? ingredient.Unit.Name : "count",
                    // fields that aren't on ingredient yet
                    SKU = ingredient.Sku ?? "",
                    Price = ingredient.Price ?? 0m,
                    ReorderLevel = ingredient.ReorderLevel,
                    IsLowStock = ingredient.StockQuantity <= ingredient.ReorderLevel,
                    OutOfStockDate = null,
                    LastOrderDate = null,
                    Notes = ingredient.Notes ?? ""
                })
                .ToListAsync();
        }

        public async Task<InventoryItemDTO> AddItemAsync(CreateIngredientDTO dto) {
            var entity = new CulinaryCommand.Inventory.Entities.Ingredient {
                Name = dto.Name,
                Sku = dto.SKU,
                Price = dto.Price,
                StockQuantity = dto.CurrentQuantity,
                ReorderLevel = dto.ReorderLevel,
                UnitId = dto.UnitId,
                CreatedAt = DateTime.UtcNow
            };
            _db.Ingredients.Add(entity);
            await _db.SaveChangesAsync();
            return new InventoryItemDTO {
                Id = entity.Id,
                Name = entity.Name,
                SKU = entity.Sku ?? string.Empty,
                Category = entity.Category ?? string.Empty,
                CurrentQuantity = entity.StockQuantity,
                Unit = entity.Unit != null ? entity.Unit.Name : "count",
                Price = entity.Price ?? 0m,
                ReorderLevel = entity.ReorderLevel,
                IsLowStock = entity.StockQuantity <= entity.ReorderLevel,
                OutOfStockDate = null,
                LastOrderDate = null,
                Notes = entity.Notes ?? string.Empty
            };
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var entity = await _db.Ingredients.FindAsync(id);
            if (entity == null)
                return false;

            _db.Ingredients.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<InventoryItemDTO?> UpdateItemAsync(InventoryItemDTO dto)
        {
            var entity = await _db.Ingredients.FindAsync(dto.Id);
            if (entity == null)
                return null;

            // Map mutable fields from DTO back to the entity
            entity.Name = dto.Name;
            entity.Sku = string.IsNullOrWhiteSpace(dto.SKU) ? null : dto.SKU;
            entity.StockQuantity = dto.CurrentQuantity;
            entity.Price = dto.Price;
            entity.ReorderLevel = dto.ReorderLevel;
            // Category and Unit mapping: if you have Unit lookup by name to Id, handle here.
            // For now assume Category stored on entity and UnitId unchanged.
            entity.Category = dto.Category;

            await _db.SaveChangesAsync();

            return new InventoryItemDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                SKU = entity.Sku ?? string.Empty,
                Category = entity.Category ?? string.Empty,
                CurrentQuantity = entity.StockQuantity,
                Unit = entity.Unit != null ? entity.Unit.Name : "count",
                Price = entity.Price ?? 0m,
                ReorderLevel = entity.ReorderLevel,
                IsLowStock = entity.StockQuantity <= entity.ReorderLevel,
                OutOfStockDate = null,
                LastOrderDate = null,
                Notes = entity.Notes ?? string.Empty
            };
        }
     }
 }