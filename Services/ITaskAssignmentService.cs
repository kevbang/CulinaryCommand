using CulinaryCommand.Data.Entities;

namespace CulinaryCommand.Services
{
    public interface ITaskAssignmentService
    {
        Task<List<WorkTask>> GetByLocationAsync(int locationId);
        Task<WorkTask> CreateAsync(WorkTask task);
        Task UpdateStatusAsync(int id, string status);
        Task BumpDueDateAsync(int id, int days);
        Task DeleteAsync(int id);
        Task<List<WorkTask>> GetForUserAsync(int userId, int? locationId = null);
    }
}
