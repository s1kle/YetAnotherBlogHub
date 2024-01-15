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

    public ArticleTagRepository(IDistributedCache cache, IBlogHubDbContext dbContext) =>
        (_cache, _dbContext) = (cache, dbContext);

    public async Task<List<ArticleTagLink>?> GetAllAsync(CancellationToken cancellationToken)
    {
        var key = "Name:ArticleTags";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext
            .Links
            .ToListAsync(),
        cancellationToken);
    }

    public async Task<List<ArticleTagLink>?> GetAllByArticleIdAsync(Guid articleId, CancellationToken cancellationToken)
    {
        var key = $"Name:ArticleTags;Article:{ArticleId}";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext
            .Links
            .Where(link => link.ArticleId.Equals(ArticleId))
            .ToListAsync(),
        cancellationToken);
    }

    public async Task<ArticleTagLink?> GetAsync(Guid articleId, Guid tagId, CancellationToken cancellationToken)
    {
        var key = $"Name:ArticleTag;Article:{ArticleId};Tag:{tagId}";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext
            .Links
            .FirstOrDefaultAsync(link => link.ArticleId.Equals(ArticleId) &&
                link.TagId.Equals(tagId), cancellationToken),
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(ArticleTagLink link, CancellationToken cancellationToken)
    {
        var key = $"Name:ArticleTag;Entity:{link.Id}";

        await _dbContext.Links.AddAsync(link, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);
        await _cache.SetItemAsync(key, link, cancellationToken);

        return link.Id;
    }

    public async Task<Guid> RemoveAsync(ArticleTagLink link, CancellationToken cancellationToken)
    {
        _dbContext.Links.Remove(link);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);

        return link.Id;
    }
}