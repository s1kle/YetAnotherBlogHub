using BlogHub.Domain;

namespace BlogHub.Data.Interfaces;

public interface ITagRepository
{
    Task<List<Tag>?> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<Tag?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Tag tag, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(Tag tag, CancellationToken cancellationToken);
}