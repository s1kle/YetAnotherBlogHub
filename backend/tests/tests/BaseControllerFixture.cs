using System.Security.Claims;
using BlogHub.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlogHub.Tests;

public abstract class BaseControllerFixture
{
    public BlogHubDbContext BlogHubDbContext { get; init; }
    public Guid UserId => _userState
        ? _userId
        : _wrongUserId;
    private Guid _userId;
    private Guid _wrongUserId;
    private bool _userState = false;

    public BaseControllerFixture(ServiceProvider serviceProvider)
    {
        BlogHubDbContext = serviceProvider.GetRequiredService<BlogHubDbContext>();

        _userId = Guid.Parse("681e7e3e-0618-5a0c-a9d2-9cae1397baa4");
        _wrongUserId = Guid.Parse("af59d62a-ed51-5845-b953-d922356e24c2");
    }

    public HttpContext ChangeContext()
    {
        _userState = !_userState;

        var context = new DefaultHttpContext()
        {
            User = GetUser()
        };

        return context;
    }

    public async Task AddRangeAsync<T>(IEnumerable<T> entities)
    {
        var task = entities.GetType() switch
        {
            _ when typeof(T).Equals(typeof(Article)) => BlogHubDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ when typeof(T).Equals(typeof(Tag)) => BlogHubDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ when typeof(T).Equals(typeof(ArticleTag)) => BlogHubDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ when typeof(T).Equals(typeof(Comment)) => BlogHubDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ when typeof(T).Equals(typeof(User)) => BlogHubDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ => throw new InvalidOperationException()
        };

        await task;

        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await BlogHubDbContext.SaveChangesAsync();
    }

    public abstract void ChangeUser();

    public void EnsureCreated()
    {
        BlogHubDbContext.Database.EnsureCreated();
    }

    public void EnsureDeleted()
    {
        BlogHubDbContext.Database.EnsureDeleted();
    }

    private ClaimsPrincipal GetUser() =>
        new(new ClaimsIdentity(new Claim[] {
            new (ClaimTypes.NameIdentifier, UserId.ToString())
        }));

}