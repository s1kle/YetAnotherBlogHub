using BlogHub.Api.Extensions;
using BlogHub.Data.Common.Interfaces;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Services.ArticleTags;

public class ArticleTagRepository : IArticleTagRepository
{
    private readonly IDistributedCache _cache;
    private readonly IBlogHubDbContext _dbContext;
    private readonly string _prefix = "articleTag";

    public ArticleTagRepository(IDistributedCache cache, IBlogHubDbContext dbContext) =>
        (_cache, _dbContext) = (cache, dbContext);

    public async Task<ArticleTag?> GetAsync(Guid articleId, Guid tagId, CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:aricle{articleId},tag{tagId}";

        var query =
            from link in _dbContext.ArticleTags
            where link.TagId.Equals(tagId) && link.ArticleId.Equals(articleId)
            select link;

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .FirstOrDefaultAsync(cancellationToken),
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(ArticleTag link, CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:link{link.Id}";

        await _dbContext.ArticleTags.AddAsync(link, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(_prefix, cancellationToken);
        await _cache.SetItemAsync(key, link, cancellationToken);

        return link.Id;
    }

    public async Task<Guid> RemoveAsync(ArticleTag link, CancellationToken cancellationToken)
    {
        _dbContext.ArticleTags.Remove(link);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(_prefix, cancellationToken);

        return link.Id;
    }
}