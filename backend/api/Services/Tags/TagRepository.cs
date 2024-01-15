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

    public TagRepository(IDistributedCache cache, IBlogHubDbContext dbContext) =>
        (_cache, _dbContext) = (cache, dbContext);

    public async Task<List<Tag>?> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var key = $"Name:Tags;User:{userId}";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext
            .Tags
            .Where(tag => tag.UserId.Equals(userId))
            .ToListAsync(),
        cancellationToken);
    }

    public Task<List<Tag>?> GetAllByArticleIdAsync(Guid articleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Tag>?> GetAllAsync(CancellationToken cancellationToken)
    {
        var key = "Name:Tags";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext
            .Tags
            .ToListAsync(),
        cancellationToken);
    }

    public async Task<Tag?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var key = $"Name:Tag;Entity:{id}";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext
            .Tags
            .FirstOrDefaultAsync(tag => tag.Id.Equals(id), cancellationToken),
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(Tag tag, CancellationToken cancellationToken)
    {
        var key = $"Name:Tag;Entity:{tag.Id}";

        await _dbContext.Tags.AddAsync(tag, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);
        await _cache.SetItemAsync(key, tag, cancellationToken);

        return tag.Id;
    }

    public async Task<Guid> RemoveAsync(Tag tag, CancellationToken cancellationToken)
    {
        _dbContext.Tags.Remove(tag);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);

        return tag.Id;
    }
}