using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CulinaryCommand.Services
{
    public class WorkTaskService
    {
        private readonly AppDbContext _db;

        public WorkTaskService(AppDbContext db)
        {
            _db = db;
        }

        // -----------------------------
        // Basic CRUD
        // -----------------------------

        public async Task<List<WorkTask>> GetAllWorkTasksAsync()
        {
            return await _db.WorkTasks
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<WorkTask?> GetWorkTaskByIdAsync(int id)
        {
            return await _db.WorkTasks
                .Include(t => t.User)
                .Include(t => t.Assignments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<WorkTask> CreateWorkTaskAsync(WorkTask task)
        {
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            _db.WorkTasks.Add(task);
            await _db.SaveChangesAsync();

            return task;
        }

        public async Task UpdateWorkTaskAsync(WorkTask task)
        {
            task.UpdatedAt = DateTime.UtcNow;
            _db.WorkTasks.Update(task);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteWorkTaskAsync(int id)
        {
            var task = await _db.WorkTasks.FindAsync(id);
            if (task == null) return false;

            _db.WorkTasks.Remove(task);
            await _db.SaveChangesAsync();
            return true;
        }

        // -----------------------------
        // Assignment Methods
        // -----------------------------

        public async Task AssignToUserAsync(int taskId, int userId)
        {
            var assignment = new TaskAssignment
            {
                WorkTaskId = taskId,
                UserId = userId
            };

            _db.TaskAssignments.Add(assignment);
            await _db.SaveChangesAsync();
        }

        public async Task AssignToPositionAsync(int taskId, int positionId)
        {
            var assignment = new TaskAssignment
            {
                WorkTaskId = taskId,
                PositionId = positionId
            };

            _db.TaskAssignments.Add(assignment);
            await _db.SaveChangesAsync();
        }

        public async Task AssignToStationAsync(int taskId, int stationId)
        {
            var assignment = new TaskAssignment
            {
                WorkTaskId = taskId,
                StationId = stationId
            };

            _db.TaskAssignments.Add(assignment);
            await _db.SaveChangesAsync();
        }

        public async Task<List<TaskAssignment>> GetAssignmentsForTaskAsync(int taskId)
        {
            return await _db.TaskAssignments
                .Where(a => a.WorkTaskId == taskId)
                .Include(a => a.User)
                .Include(a => a.WorkTask)
                .ToListAsync();
        }

        // -----------------------------
        // Filtering (User-facing)
        // -----------------------------

        public async Task<List<WorkTask>> GetTasksForUserAsync(int userId)
        {
            // Step 1: get user’s positions
            var positionIds = await _db.UserPositions
                .Where(up => up.UserId == userId)
                .Select(up => up.PositionId)
                .ToListAsync();

            // Step 2: fetch tasks via assignments
            var tasks = await _db.TaskAssignments
                .Where(a =>
                    a.UserId == userId ||
                    (a.PositionId != null && positionIds.Contains(a.PositionId.Value)) ||
                    (a.StationId != null && a.StationId == _db.Users.Where(u => u.Id == userId)
                                                                   .Select(u => u.StationId)
                                                                   .FirstOrDefault())
                )
                .Select(a => a.WorkTask)
                .Distinct()
                .Include(t => t.User)
                .ToListAsync();

            return tasks;
        }

        public async Task<List<WorkTask>> GetTasksForPositionAsync(int positionId)
        {
            return await _db.TaskAssignments
                .Where(a => a.PositionId == positionId)
                .Select(a => a.WorkTask)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<WorkTask>> GetTasksForStationAsync(int stationId)
        {
            return await _db.TaskAssignments
                .Where(a => a.StationId == stationId)
                .Select(a => a.WorkTask)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<WorkTask>> GetTasksForLocationAsync(int locationId)
        {
            // If you add LocationId to TaskAssignment
            return await _db.TaskAssignments
                .Where(a => a.WorkTask.LocationId == locationId)
                .Select(a => a.WorkTask)
                .Distinct()
                .ToListAsync();
        }
    }
}
