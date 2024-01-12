using BlogHub.Api.Extensions;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Services;

public class UserRepository : IUserRepository
{
    private readonly IDistributedCache _cache;
    private readonly IUserDbContext _dbContext;

    public UserRepository(IDistributedCache cache, IUserDbContext dbContext) =>
        (_cache, _dbContext) = (cache, dbContext);

    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var key = $"Name:User;Entity:{id}";

        return await _cache.GetOrCreateItemAsync(key, async () => await _dbContext
            .Users
            .FirstOrDefaultAsync(user => user.Id.Equals(id), cancellationToken), 
        cancellationToken);
    }

    public async Task<Guid> CreateAsync(User user, CancellationToken cancellationToken)
    {
        var key = $"Name:User;Entity:{user.Id}";

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);
        await _cache.SetItemAsync(key, user, cancellationToken);

        return user.Id;
    }

    public async Task<Guid> RemoveAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.ClearAsync(cancellationToken);

        return user.Id;
    }
}