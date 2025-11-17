using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CulinaryCommand.Services
{
    public class IngredientService
    {
        private readonly AppDbContext _db;

        public IngredientService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _db.Ingredients
                .Select(i => i.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<List<Ingredient>> GetByCategoryAsync(string category)
        {
            return await _db.Ingredients
                .Where(i => i.Category == category)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<List<Ingredient>> GetAllAsync() =>
            await _db.Ingredients.OrderBy(i => i.Name).ToListAsync();

        public async Task<Ingredient?> GetByIdAsync(int id) =>
            await _db.Ingredients.FirstOrDefaultAsync(i => i.IngredientId == id);

        public async Task CreateAsync(Ingredient ing)
        {
            _db.Ingredients.Add(ing);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ingredient ing)
        {
            _db.Ingredients.Update(ing);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Ingredients.FindAsync(id);
            if (entity != null)
            {
                _db.Ingredients.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}