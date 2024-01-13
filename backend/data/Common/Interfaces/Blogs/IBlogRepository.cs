namespace BlogHub.Data.Common.Interfaces.Blogs;

public interface IBlogRepository
{
    Task<List<Blog>?> GetAllByUserIdAsync(Guid userId, int page, int size, CancellationToken cancellationToken);
    Task<List<Blog>?> GetAllAsync(int page, int size, CancellationToken cancellationToken);
    Task<Blog?> GetAsync(Guid blogId, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Blog blog, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(Blog blog, CancellationToken cancellationToken);
    Task<Guid> UpdateAsync(Blog original, Blog updated, CancellationToken cancellationToken);
}