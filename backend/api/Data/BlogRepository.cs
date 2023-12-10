using System.Text.Json;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;

namespace BlogHub.Api.Data;

public class BlogRepository : IBlogRepository
{
    private readonly IDistributedCache _cache;
    private readonly IBlogDbContext _dbContext;
    private readonly ILogger _logger;

    public BlogRepository(IDistributedCache cache, IBlogDbContext dbContext, ILogger logger)
    {
        _cache = cache;
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<List<Blog>> GetAllBlogsAsync(Guid userId, int page, int size, CancellationToken cancellationToken)
    {
        var cacheKey = $"Blogs-{userId}";

        var bytes = await _cache.GetAsync(cacheKey, cancellationToken);

        if (bytes is not null)
        {
            _logger.Information($"Reading cache for {cacheKey}");
            var cachedBlogs = JsonSerializer.Deserialize<List<Blog>>(bytes);
            return cachedBlogs ?? new ();
        }

        _logger.Information($"Reading data for {userId}");
        var blogs = _dbContext.Blogs
            .Where(blog => blog.UserId.Equals(userId))
            .OrderBy(blog => blog.Id)
            .Skip(page * size)
            .Take(size)
            .ToList();

        bytes = JsonSerializer.SerializeToUtf8Bytes(blogs);
        await _cache.SetAsync(cacheKey, bytes, cancellationToken);

        return blogs;
    }

    public async Task<Blog?> GetBlogAsync(Guid blogId, CancellationToken cancellationToken)
    {
        var cacheKey = $"Blog-{blogId}";

        var bytes = await _cache.GetAsync(cacheKey, cancellationToken);

        if (bytes is not null)
        {
            _logger.Information($"Reading cache for {cacheKey}");
            var cachedBlog = JsonSerializer.Deserialize<Blog>(bytes);
            return cachedBlog;
        }

        _logger.Information($"Reading data for {blogId}");
        var blog = await _dbContext
            .Blogs
            .FirstOrDefaultAsync(entity => entity.Id.Equals(blogId), cancellationToken);

        bytes = JsonSerializer.SerializeToUtf8Bytes(blog);
        await _cache.SetAsync(cacheKey, bytes, cancellationToken);

        return blog;
    }

    public async Task<Guid> CreateAsync(Blog blog, CancellationToken cancellationToken)
    {
        await _dbContext.Blogs.AddAsync(blog, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var bytes = JsonSerializer.SerializeToUtf8Bytes(blog);
        await _cache.SetAsync($"Blog-{blog.Id}", bytes, cancellationToken);
        await _cache.RemoveAsync($"Blogs-{blog.UserId}", cancellationToken);

        return blog.Id;
    }

    public async Task<Guid> UpdateAsync(Blog original, Blog updated, CancellationToken cancellationToken)
    {
        _dbContext.Blogs.Remove(original);
        await _dbContext.Blogs.AddAsync(updated, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync($"Blog-{original.Id}", cancellationToken);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(updated);
        await _cache.SetAsync($"Blog-{updated.Id}", bytes, cancellationToken);
        await _cache.RemoveAsync($"Blogs-{updated.UserId}", cancellationToken);

        return updated.Id;
    }

    public async Task<Guid> RemoveAsync(Blog blog, CancellationToken cancellationToken)
    {
        _dbContext.Blogs.Remove(blog);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync($"Blog-{blog.Id}", cancellationToken);
        await _cache.RemoveAsync($"Blogs-{blog.UserId}", cancellationToken);

        return blog.Id;
    }
}