using BlogHub.Api.Extensions;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Services.Comments;

public class CommentRepository : Interfaces.Comments.Repository
{
    private readonly IDistributedCache _cache;
    private readonly Interfaces.Comments.DbContext _dbContext;

    public CommentRepository(IDistributedCache cache, Interfaces.Comments.DbContext dbContext) =>
        (_cache, _dbContext) = (cache, dbContext);

    public async Task<List<Comment>?> GetAllByBlogIdAsync(Guid blogId, CancellationToken cancellationToken)
    {
        var key = $"Name:Comments;Blog:{blogId}";
        
        var query = _dbContext.Comments
            .OrderBy(comment => comment.CreationDate)
            .Where(comment => comment.BlogId.Equals(blogId));

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .ToListAsync(), 
        cancellationToken);
    }

    public async Task<Comment?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var key = $"Name:Comment;Entity:{id}";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext.Comments
            .FirstOrDefaultAsync(comment => comment.Id.Equals(id), cancellationToken), 
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(Comment comment, CancellationToken cancellationToken)
    {
        var key = $"Name:Comment;Entity:{comment.Id}";

        await _dbContext.Comments.AddAsync(comment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);
        await _cache.SetItemAsync(key, comment, cancellationToken);

        return comment.Id;
    }

    public async Task<Guid> RemoveAsync(Comment comment, CancellationToken cancellationToken)
    {
        _dbContext.Comments.Remove(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);

        return comment.Id;
    }
}