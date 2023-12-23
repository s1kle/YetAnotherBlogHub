using BlogHub.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.Methods;

public class Delete_Tests
{
    private readonly FixtureFactory _fixtureFactory;
    private readonly string _dbContextName;

    public Delete_Tests()
    {
        _fixtureFactory = new ();
        _dbContextName = $"{this.GetType()}";
    }

    [Fact]
    public async Task Delete_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var id = Guid.NewGuid();

        var blog = new Blog()
        {
            Id = id,
            UserId = blogControllerFixture.UserId,
            Title = "Title",
            Details = "Details",
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };

        await blogControllerFixture.BlogDbContext.Blogs.AddAsync(blog);
        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        var result = (await blogController.Delete(id)).Result as OkObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);

        blogControllerFixture.BlogDbContext.Blogs.Should().BeEmpty();

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }
    
    [Fact]
    public async Task Delete_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var id = Guid.NewGuid();
        var userId = blogControllerFixture.UserId;

        var blog = new Blog()
        {
            Id = id,
            UserId = userId,
            Title = "Title",
            Details = "Details",
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };

        await blogControllerFixture.BlogDbContext.Blogs.AddAsync(blog);
        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController.Delete(Guid.Empty);
        });

        blogControllerFixture.ChangeUser();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var result = await blogController.Delete(id);
        });

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }
}