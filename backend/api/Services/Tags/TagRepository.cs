using BlogHub.Api.Extensions;
using BlogHub.Data.Common.Interfaces;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Services;

public class TagRepository : ITagRepository
{
    private readonly IDistributedCache _cache;
    private readonly IBlogHubDbContext _dbContext;
    private readonly string _prefix = "tag";

    public TagRepository(IDistributedCache cache, IBlogHubDbContext dbContext) =>
        (_cache, _dbContext) = (cache, dbContext);

    public async Task<List<Tag>?> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:user{userId}";

        var query =
            from tag in _dbContext.Tags
            orderby tag.Name
            where tag.UserId.Equals(userId)
            select tag;

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .ToListAsync(),
        cancellationToken);
    }

    public async Task<List<Tag>?> GetAllByArticleIdAsync(Guid articleId, CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:article{articleId}";

        var query =
            from tag in _dbContext.Tags
            join link in _dbContext.ArticleTags on tag.Id equals link.TagId
            where link.ArticleId.Equals(articleId)
            orderby tag.Name
            select tag;

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .ToListAsync(),
        cancellationToken);
    }

    public async Task<List<Tag>?> GetAllAsync(CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:list";

        var query =
            from tag in _dbContext.Tags
            orderby tag.Name
            select tag;

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .ToListAsync(),
        cancellationToken);
    }

    public async Task<Tag?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:tag{id}";

        var query =
            from tag in _dbContext.Tags
            where tag.Id.Equals(id)
            select tag;

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .FirstOrDefaultAsync(cancellationToken),
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(Tag tag, CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:tag{tag.Id}";

        await _dbContext.Tags.AddAsync(tag, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(_prefix, cancellationToken);
        await _cache.SetItemAsync(key, tag, cancellationToken);

        return tag.Id;
    }

    public async Task<Guid> RemoveAsync(Tag tag, CancellationToken cancellationToken)
    {
        _dbContext.Tags.Remove(tag);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(_prefix, cancellationToken);

        return tag.Id;
    }
}