using BlogHub.Api.Extensions;
using BlogHub.Data.Common.Interfaces;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Services.Articles;

public class ArticleRepository : IArticleRepository
{
    private readonly IDistributedCache _cache;
    private readonly IBlogHubDbContext _dbContext;
    private readonly string _prefix = "article";

    public ArticleRepository(IDistributedCache cache, IBlogHubDbContext dbContext) =>
        (_cache, _dbContext) = (cache, dbContext);

    public async Task<List<Article>?> GetAllAsync(int page, int size, CancellationToken cancellationToken)
    {
        var key = $"{_prefix},page{page},size{size}";

        var query =
            from article in _dbContext.Articles
            orderby article.CreationDate
            select article;

        await CacheArticlesPageAsync(page + 1, size, null, query, cancellationToken);
        await CacheArticlesPageAsync(page - 1, size, null, query, cancellationToken);

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .Skip(page * size)
            .Take(size)
            .ToListAsync(),
        cancellationToken);
    }

    public async Task<List<Article>?> GetAllByUserIdAsync(Guid userId, int page, int size, CancellationToken cancellationToken)
    {
        var key = $"{_prefix},page{page},size{size},user{userId}";

        var query =
            from article in _dbContext.Articles
            orderby article.CreationDate
            where article.UserId.Equals(userId)
            select article;

        await CacheArticlesPageAsync(page + 1, size, userId, query, cancellationToken);
        await CacheArticlesPageAsync(page - 1, size, userId, query, cancellationToken);

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .Skip(page * size)
            .Take(size)
            .ToListAsync(),
        cancellationToken);
    }

    public async Task<Article?> GetAsync(Guid articleId, CancellationToken cancellationToken)
    {
        var key = $"{_prefix},{articleId}";

        var query =
            from article in _dbContext.Articles
            where article.Id.Equals(articleId)
            select article;

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .FirstOrDefaultAsync(cancellationToken),
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(Article article, CancellationToken cancellationToken)
    {
        var key = $"{_prefix},{article.Id}";

        await _dbContext.Articles.AddAsync(article, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(_prefix, cancellationToken);
        await _cache.SetItemAsync(key, article, cancellationToken);

        return article.Id;
    }

    public async Task<Guid> UpdateAsync(Article original, Article updated, CancellationToken cancellationToken)
    {
        var key = $"{_prefix},{original.Id}";

        _dbContext.Articles.Remove(original);
        await _dbContext.Articles.AddAsync(updated, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(_prefix, cancellationToken);
        await _cache.SetItemAsync(key, updated, cancellationToken);

        return updated.Id;
    }

    public async Task<Guid> RemoveAsync(Article article, CancellationToken cancellationToken)
    {
        _dbContext.Articles.Remove(article);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(_prefix, cancellationToken);

        return article.Id;
    }

    private async Task CacheArticlesPageAsync(int page, int size, Guid? userId, IQueryable<Article> query, CancellationToken cancellationToken)
    {
        if (page < 0) return;

        var key = userId is null
            ? $"{_prefix},page{page},size{size}"
            : $"{_prefix},page{page},size{size},user{userId}";

        if (await _cache.ContainsAsync(key, cancellationToken)) return;

        await _cache.SetItemAsync(key, await query
            .Skip(page * size)
            .Take(size)
            .ToListAsync(),
        cancellationToken);
    }
}