using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CulinaryCommand.Services
{
    public class UnitService
    {
        private readonly AppDbContext _db;

        public UnitService(AppDbContext db)
        {
            _db = db;
        }

        // Units are global, not ingredient-specific
        public async Task<List<MeasurementUnit>> GetAllUnitsAsync()
        {
            return await _db.MeasurementUnits
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        // For compatibility with your RecipeForm call
        public async Task<List<MeasurementUnit>> GetUnitsForIngredient(int ingredientId)
        {
            // Return all units — ingredients don't restrict units
            return await GetAllUnitsAsync();
        }
    }
}