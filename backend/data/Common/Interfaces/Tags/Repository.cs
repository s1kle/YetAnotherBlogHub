namespace BlogHub.Data.Common.Interfaces.Tags;

public interface Repository
{
    Task<List<Tag>?> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<Tag>?> GetAllAsync(CancellationToken cancellationToken);
    Task<Tag?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Tag tag, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(Tag tag, CancellationToken cancellationToken);
}