using System.Security.Claims;
using BlogHub.Api.Services.Articles;
using BlogHub.Api.Services.ArticleTags;
using BlogHub.Api.Services.Comments;
using BlogHub.Api.Services.Tags;
using BlogHub.Api.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BlogHub.Tests;

public abstract class BaseControllerFixture
{
    public ArticleDbContext ArticleDbContext { get; init; }
    public TagDbContext TagDbContext { get; init; }
    public ArticleTagDbContext ArticleTagDbContext { get; init; }
    public UserDbContext UserDbContext { get; init; }
    public CommentDbContext CommentDbContext { get; init; }
    public Guid UserId => _userState
        ? _userId
        : _wrongUserId;
    private Guid _userId;
    private Guid _wrongUserId;
    private bool _userState = false;

    public BaseControllerFixture(ServiceProvider serviceProvider)
    {
        ArticleDbContext = serviceProvider.GetRequiredService<ArticleDbContext>();
        TagDbContext = serviceProvider.GetRequiredService<TagDbContext>();
        ArticleTagDbContext = serviceProvider.GetRequiredService<ArticleTagDbContext>();
        UserDbContext = serviceProvider.GetRequiredService<UserDbContext>();
        CommentDbContext = serviceProvider.GetRequiredService<CommentDbContext>();

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
            _ when typeof(T).Equals(typeof(Article)) => ArticleDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ when typeof(T).Equals(typeof(Tag)) => TagDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ when typeof(T).Equals(typeof(ArticleTagLink)) => ArticleTagDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ when typeof(T).Equals(typeof(Comment)) => CommentDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ when typeof(T).Equals(typeof(User)) => UserDbContext
                .AddRangeAsync(entities.Select(entity => (object)entity!)),
            _ => throw new InvalidOperationException()
        };

        await task;

        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await ArticleDbContext.SaveChangesAsync();
        await TagDbContext.SaveChangesAsync();
        await ArticleTagDbContext.SaveChangesAsync();
        await CommentDbContext.SaveChangesAsync();
        await UserDbContext.SaveChangesAsync();
    }

    public abstract void ChangeUser();

    public void EnsureCreated()
    {
        ArticleDbContext.Database.EnsureCreated();
        TagDbContext.Database.EnsureCreated();
        ArticleTagDbContext.Database.EnsureCreated();
    }

    public void EnsureDeleted()
    {
        ArticleDbContext.Database.EnsureDeleted();
        TagDbContext.Database.EnsureDeleted();
        ArticleTagDbContext.Database.EnsureDeleted();
    }

    private ClaimsPrincipal GetUser() =>
        new(new ClaimsIdentity(new Claim[] {
            new (ClaimTypes.NameIdentifier, UserId.ToString())
        }));

}