namespace CulinaryCommand.Services.UserContextSpace;

public interface IUserContextService
{
    Task<UserContext> GetAsync(CancellationToken ct = default);
    Task<UserContext> RefreshAsync(CancellationToken ct = default);
}
