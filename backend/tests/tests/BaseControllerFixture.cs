using System.Linq.Expressions;
using System.Security.Claims;
using BlogHub.Api.Extensions;
using BlogHub.Api.Services;
using BlogHub.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlogHub.Tests;

public abstract class BaseControllerFixture
{
    public BlogDbContext BlogDbContext { get; init; }
    public TagDbContext TagDbContext { get; init; }
    public BlogTagDbContext BlogTagDbContext { get; init; }
    public Guid UserId => _userState
        ? _userId
        : _wrongUserId;
    private Guid _userId;
    private Guid _wrongUserId;
    private bool _userState = false;

    public BaseControllerFixture(ServiceProvider serviceProvider)
    {
        BlogDbContext = serviceProvider.GetRequiredService<BlogDbContext>();
        TagDbContext = serviceProvider.GetRequiredService<TagDbContext>();
        BlogTagDbContext = serviceProvider.GetRequiredService<BlogTagDbContext>();

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
            _ when typeof(T).Equals(typeof(Blog)) => BlogDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ when typeof(T).Equals(typeof(Tag)) => TagDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ when typeof(T).Equals(typeof(BlogTagLink)) => BlogTagDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ => throw new InvalidOperationException()
        };

        await task;

        await SaveChangesAsync(); 
    }

    private async Task SaveChangesAsync()
    {
        await BlogDbContext.SaveChangesAsync();
        await TagDbContext.SaveChangesAsync();
        await BlogTagDbContext.SaveChangesAsync();
    }

    public abstract void ChangeUser();

    public void EnsureCreated()
    {
        BlogDbContext.Database.EnsureCreated();
        TagDbContext.Database.EnsureCreated();
        BlogTagDbContext.Database.EnsureCreated();
    }

    public void EnsureDeleted()
    {
        BlogDbContext.Database.EnsureDeleted();
        TagDbContext.Database.EnsureDeleted();
        BlogTagDbContext.Database.EnsureDeleted();
    }

    private ClaimsPrincipal GetUser() =>
        new(new ClaimsIdentity(new Claim[] {
            new (ClaimTypes.NameIdentifier, UserId.ToString())
        }));

}