using BlogHub.Data.Interfaces;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Data;

public class BlogRepository : IBlogRepository
{
    private readonly IDistributedCache _cache;
    private readonly IBlogDbContext _dbContext;

    public BlogRepository(IDistributedCache cache, IBlogDbContext dbContext)
    {
        _cache = cache;
        _dbContext = dbContext;
    }


    public IQueryable<Blog> GetAllBlogsAsync(Guid userId, int page, int size)
    {
        var blogs = _dbContext.Blogs
            .Where(blog => blog.UserId.Equals(userId))
            .Skip(page * size)
            .Take(size);
        return blogs;
    }

    public async Task<Blog?> GetBlogAsync(Guid blogId, CancellationToken cancellationToken)
    {
        var blog = await _dbContext
            .Blogs
            .FirstOrDefaultAsync(entity => entity.Id.Equals(blogId), cancellationToken);
        return blog;
    }

    public async Task<Guid> CreateAsync(Blog blog, CancellationToken cancellationToken)
    {
        await _dbContext.Blogs.AddAsync(blog, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return blog.Id;
    }

    public async Task<Guid> UpdateAsync(Blog original, Blog updated, CancellationToken cancellationToken)
    {
        _dbContext.Blogs.Remove(original);
        await _dbContext.Blogs.AddAsync(updated, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return updated.Id;
    }

    public async Task<Guid> RemoveAsync(Blog blog, CancellationToken cancellationToken)
    {
        _dbContext.Blogs.Remove(blog);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return blog.Id;
    }
}