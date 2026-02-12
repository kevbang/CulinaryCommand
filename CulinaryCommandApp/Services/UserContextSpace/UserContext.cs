using CulinaryCommand.Data.Entities;

namespace CulinaryCommand.Services.UserContextSpace;

public sealed class UserContext
{
    public bool IsAuthenticated { get; init; }
    public string? Email { get; init; }
    public string? CognitoSub { get; init; }

    // Null => invite-only user not found in DB
    public User? User { get; init; }

    // Normalized list for LocationState/UI
    public List<Location> AccessibleLocations { get; init; } = new();
}
