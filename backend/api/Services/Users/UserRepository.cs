using BlogHub.Api.Extensions;
using BlogHub.Data.Common.Interfaces;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Services.Users;

public class UserRepository : IUserRepository
{
    private readonly IDistributedCache _cache;
    private readonly IBlogHubDbContext _dbContext;
    private readonly string _prefix = "user";

    public UserRepository(IDistributedCache cache, IBlogHubDbContext dbContext) =>
        (_cache, _dbContext) = (cache, dbContext);

    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:user{id}";

        var query =
            from user in _dbContext.Users
            where user.Id.Equals(id)
            select user;

        return await _cache.GetOrCreateItemAsync(key, async () => await query
            .FirstOrDefaultAsync(cancellationToken),
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(User user, CancellationToken cancellationToken)
    {
        var key = $"{_prefix}:user{user.Id}";

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(_prefix, cancellationToken);
        await _cache.SetItemAsync(key, user, cancellationToken);

        return user.Id;
    }

    public async Task<Guid> RemoveAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(_prefix, cancellationToken);

        return user.Id;
    }
}