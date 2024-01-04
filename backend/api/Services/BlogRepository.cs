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

    public async Task<List<Blog>?> GetAllAsync(int page, int size, CancellationToken cancellationToken)
    {
        var key = $"Name:Blogs;Page:{page}";
        
        var blogsQuery = _dbContext.Blogs.OrderBy(blog => blog.CreationDate);

        await CacheBlogsPageAsync(page + 1, size, null, blogsQuery, cancellationToken);
        await CacheBlogsPageAsync(page - 1, size, null, blogsQuery, cancellationToken);

        return await _cache.GetOrCreateItemAsync(key, async () => await blogsQuery
            .Skip(page * size)
            .Take(size)
            .ToListAsync(), 
        cancellationToken);
    }

    public async Task<List<Blog>?> GetAllByUserIdAsync(Guid userId, int page, int size, CancellationToken cancellationToken)
    {
        var key = $"Name:Blogs;Page:{page};User:{userId}";
        
        var blogsQuery = _dbContext.Blogs
            .Where(blog => blog.UserId.Equals(userId));

        _ = CacheBlogsPageAsync(page + 1, size, userId, blogsQuery, cancellationToken);
        _ = CacheBlogsPageAsync(page - 1, size, userId, blogsQuery, cancellationToken);

        return await _cache.GetOrCreateItemAsync(key, async () => await blogsQuery
            .Skip(page * size)
            .Take(size)
            .ToListAsync(), 
        cancellationToken);
    }

    public async Task<Blog?> GetAsync(Guid blogId, CancellationToken cancellationToken)
    {
        var key = $"Name:Blog;Entity:{blogId}";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext.Blogs
            .FirstOrDefaultAsync(blog => blog.Id.Equals(blogId), cancellationToken), 
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(Blog blog, CancellationToken cancellationToken)
    {
        var key = $"Name:Blog;Entity:{blog.Id}";

        await _dbContext.Blogs.AddAsync(blog, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);
        await _cache.SetItemAsync(key, blog, cancellationToken);

        return blog.Id;
    }

    public async Task<Guid> UpdateAsync(Blog original, Blog updated, CancellationToken cancellationToken)
    {
        var key = $"Name:Blog;Entity:{original.Id}";

        _dbContext.Blogs.Remove(original);
        await _dbContext.Blogs.AddAsync(updated, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);
        await _cache.SetItemAsync(key, updated, cancellationToken);

        return updated.Id;
    }

    public async Task<Guid> RemoveAsync(Blog blog, CancellationToken cancellationToken)
    {
        _dbContext.Blogs.Remove(blog);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);

        return blog.Id;
    }

    private async Task CacheBlogsPageAsync(int page, int size, Guid? userId, IQueryable<Blog> query, CancellationToken cancellationToken)
    {
        if (page < 0) return;

        var key = userId is null
            ? $"Name:Blogs;Page:{page}"
            : $"Name:Blogs;Page:{page};User:{userId}";

        if (_cache.Contains(key)) return;

        await _cache.SetItemAsync(key, await query
            .Skip(page * size)
            .Take(size)
            .ToListAsync(),
        cancellationToken);
    }
}