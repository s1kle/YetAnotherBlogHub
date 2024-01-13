namespace BlogHub.Data.Common.Interfaces.BlogTags;

public interface BlogTagRepository
{
    Task<List<BlogTagLink>?> GetAllByBlogIdAsync(Guid blogId, CancellationToken cancellationToken);
    Task<List<BlogTagLink>?> GetAllAsync(CancellationToken cancellationToken);
    Task<BlogTagLink?> GetAsync(Guid blogId, Guid tagId, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(BlogTagLink link, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(BlogTagLink link, CancellationToken cancellationToken);
}