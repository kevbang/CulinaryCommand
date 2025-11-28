using CulinaryCommand.Data;
using CulinaryCommand.Inventory.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryCommand.Inventory.Services
{
    public class UnitService
    {
        private readonly AppDbContext _db;

        public UnitService(AppDbContext db)
        {
            _db = db;
        }

        /// Return all inventory units used by the inventory subsystem.
        public async Task<List<Unit>> GetAllUnitsAsync()
        {
            return await _db.Units
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        // For compatibility: return units that could be used for the given ingredient.
        // Currently returns all units (no per-ingredient restriction).
        public async Task<List<Unit>> GetUnitsForIngredient(int ingredientId)
        {
            return await GetAllUnitsAsync();
        }
    }
}
