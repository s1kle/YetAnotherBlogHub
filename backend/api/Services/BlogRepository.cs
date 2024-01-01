using BlogHub.Api.Extensions;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Services;

public class BlogRepository : IBlogRepository
{
    private readonly IDistributedCache _cache;
    private readonly IBlogDbContext _dbContext;

    public BlogRepository(IDistributedCache cache, IBlogDbContext dbContext)
    {
        _cache = cache;
        _dbContext = dbContext;
    }

    public async Task<int> GetListCountAsync(Guid userId, CancellationToken cancellationToken)
    {
        var key = $"Count-{userId}";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext.Blogs
            .CountAsync(blog => blog.UserId.Equals(userId), cancellationToken));
    }

    public async Task<List<Blog>?> GetAllBlogsAsync(Guid userId, int page, int size, CancellationToken cancellationToken)
    {
        var key = $"Blogs-{userId}";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext.Blogs
            .Where(blog => blog.UserId.Equals(userId))
            .OrderBy(blog => blog.Id)
            .Skip(page * size)
            .Take(size)
            .ToListAsync(), 
        cancellationToken);
    }

    public async Task<Blog?> GetBlogAsync(Guid blogId, CancellationToken cancellationToken)
    {
        var key = $"Blog-{blogId}";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext.Blogs
            .FirstOrDefaultAsync(blog => blog.Id.Equals(blogId), cancellationToken), 
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(Blog blog, CancellationToken cancellationToken)
    {
        var key = $"Blog-{blog.Id}";
        var blogsKey = $"Blogs-{blog.UserId}";

        await _dbContext.Blogs.AddAsync(blog, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.SetItemAsync(key, blog, cancellationToken);
        await _cache.RemoveAsync(blogsKey, cancellationToken);

        return blog.Id;
    }

    public async Task<Guid> UpdateAsync(Blog original, Blog updated, CancellationToken cancellationToken)
    {
        var key = $"Blog-{original.Id}";
        var blogsKey = $"Blogs-{original.UserId}";

        _dbContext.Blogs.Remove(original);
        await _dbContext.Blogs.AddAsync(updated, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(cancellationToken, key, blogsKey);
        await _cache.SetItemAsync(key, updated, cancellationToken);

        return updated.Id;
    }

    public async Task<Guid> RemoveAsync(Blog blog, CancellationToken cancellationToken)
    {
        var key = $"Blog-{blog.Id}";
        var blogsKey = $"Blogs-{blog.UserId}";
        
        _dbContext.Blogs.Remove(blog);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(cancellationToken, key, blogsKey);

        return blog.Id;
    }
}