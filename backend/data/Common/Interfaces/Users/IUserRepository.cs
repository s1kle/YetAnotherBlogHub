namespace BlogHub.Data.Common.Interfaces.Users;

public interface IUserRepository
{
    Task<User?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(User user, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(User user, CancellationToken cancellationToken);
}