using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CulinaryCommand.Services
{
    public class TaskAssignmentService : ITaskAssignmentService
    {
        private readonly AppDbContext _db;

        public TaskAssignmentService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<WorkTask>> GetByLocationAsync(int locationId)
        {
            return await _db.Tasks  
                .Include(t => t.User)
                .Include(t => t.Recipe)
                .Include(t => t.Ingredient)             
                .Where(t => t.LocationId == locationId)
                .OrderByDescending(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<WorkTask> CreateAsync(WorkTask task)
        {
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            _db.Tasks.Add(task);                
            await _db.SaveChangesAsync();
            return task;
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var task = await _db.Tasks.FindAsync(id);   
            if (task == null) return;

            task.Status = status;
            task.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task BumpDueDateAsync(int id, int days)
        {
            var task = await _db.Tasks.FindAsync(id);   
            if (task == null) return;

            task.DueDate = task.DueDate.AddDays(days);
            task.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task == null) return;

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
        }

       public async Task<List<WorkTask>> GetForUserAsync(int userId, int? locationId = null)
        {
            var query = _db.Tasks.AsQueryable()
                                 .Where(t => t.UserId == userId);

            if (locationId.HasValue)
            {
                query = query.Where(t => t.LocationId == locationId.Value);
            }

            return await query
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

    }
}
