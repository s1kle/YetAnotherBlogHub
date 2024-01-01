using BlogHub.Domain;

namespace BlogHub.Data.Interfaces;

public interface IBlogRepository
{
    Task<List<Blog>?> GetAllBlogsAsync(Guid userId, int page, int size, CancellationToken cancellationToken);
    Task<Blog?> GetBlogAsync(Guid blogId, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Blog blog, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(Blog blog, CancellationToken cancellationToken);
    Task<Guid> UpdateAsync(Blog original, Blog updated, CancellationToken cancellationToken);
}