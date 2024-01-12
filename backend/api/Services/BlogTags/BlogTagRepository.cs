using BlogHub.Api.Extensions;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Services.BlogTags;

public class BlogTagRepository : Interfaces.BlogTags.Repository
{
    private readonly IDistributedCache _cache;
    private readonly Interfaces.BlogTags.DbContext _dbContext;

    public BlogTagRepository(IDistributedCache cache, Interfaces.BlogTags.DbContext dbContext) =>
        (_cache, _dbContext) = (cache, dbContext);

    public async Task<List<BlogTagLink>?> GetAllAsync(CancellationToken cancellationToken)
    {
        var key = "Name:BlogTags";
    
        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext
            .Links
            .ToListAsync(), 
        cancellationToken);
    }

    public async Task<List<BlogTagLink>?> GetAllByBlogIdAsync(Guid blogId, CancellationToken cancellationToken)
    {
        var key = $"Name:BlogTags;Blog:{blogId}";
    
        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext
            .Links
            .Where(link => link.BlogId.Equals(blogId))
            .ToListAsync(), 
        cancellationToken);
    }

    public async Task<BlogTagLink?> GetAsync(Guid blogId, Guid tagId, CancellationToken cancellationToken)
    {
        var key = $"Name:BlogTag;Blog:{blogId};Tag:{tagId}";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext
            .Links
            .FirstOrDefaultAsync(link => link.BlogId.Equals(blogId) &&
                link.TagId.Equals(tagId), cancellationToken), 
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(BlogTagLink link, CancellationToken cancellationToken)
    {
        var key = $"Name:BlogTag;Entity:{link.Id}";

        await _dbContext.Links.AddAsync(link, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);
        await _cache.SetItemAsync(key, link, cancellationToken);

        return link.Id;
    }

    public async Task<Guid> RemoveAsync(BlogTagLink link, CancellationToken cancellationToken)
    {
        _dbContext.Links.Remove(link);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);

        return link.Id;
    }
}