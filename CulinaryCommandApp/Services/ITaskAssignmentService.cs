using CulinaryCommand.Data.Entities;

namespace CulinaryCommand.Services
{
    public interface ITaskAssignmentService
    {
        Task<List<Tasks>> GetByLocationAsync(int locationId);
        Task<Tasks> CreateAsync(Tasks task);
        Task UpdateStatusAsync(int id, string status);
        Task BumpDueDateAsync(int id, int days);
        Task DeleteAsync(int id);
        Task<List<Tasks>> GetForUserAsync(int userId, int? locationId = null);
    }
}