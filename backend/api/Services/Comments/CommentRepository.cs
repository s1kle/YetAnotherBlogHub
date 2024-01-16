using BlogHub.Api.Extensions;
using BlogHub.Data.Common.Interfaces;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Services.Comments;

public class CommentRepository : ICommentRepository
{
    private readonly IDistributedCache _cache;
    private readonly IBlogHubDbContext _dbContext;
    private readonly string _prefix = "comment";

    public CommentRepository(IDistributedCache cache, IBlogHubDbContext dbContext) =>
        (_cache, _dbContext) = (cache, dbContext);

    public async Task<List<Comment>?> GetAllByArticleIdAsync(Guid articleId, CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:article{articleId}";

        var query =
            from comment in _dbContext.Comments
            orderby comment.CreationDate
            where comment.ArticleId.Equals(articleId)
            select comment;

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .ToListAsync(),
        cancellationToken);
    }

    public async Task<Comment?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:comment{id}";

        var query =
            from comment in _dbContext.Comments
            where comment.Id.Equals(id)
            select comment;

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .FirstOrDefaultAsync(cancellationToken),
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(Comment comment, CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:comment{comment.Id}";

        await _dbContext.Comments.AddAsync(comment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(_prefix, cancellationToken);
        await _cache.SetItemAsync(key, comment, cancellationToken);

        return comment.Id;
    }

    public async Task<Guid> RemoveAsync(Comment comment, CancellationToken cancellationToken)
    {
        _dbContext.Comments.Remove(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(_prefix, cancellationToken);

        return comment.Id;
    }
}