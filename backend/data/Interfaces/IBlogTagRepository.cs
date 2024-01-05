using BlogHub.Domain;

namespace BlogHub.Data.Interfaces;

public interface IBlogTagRepository
{
    Task<List<BlogTag>?> GetAllAsync(Guid blodId, CancellationToken cancellationToken);
    Task<BlogTag?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(BlogTag blogTag, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(BlogTag blogTag, CancellationToken cancellationToken);
}