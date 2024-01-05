using BlogHub.Data.Exceptions;
using BlogHub.Data.Queries.Get;
using BlogHub.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.Methods;

public class Get_Tests
{
    private readonly FixtureFactory _fixtureFactory;
    private readonly string _dbContextName;

    public Get_Tests()
    {
        _fixtureFactory = new ();
        _dbContextName = $"{this.GetType()}";
    }

    [Fact]
    public async Task Get_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.UnauthorizeBlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var id = Guid.NewGuid();

        var expected = new Blog()
        {
            Id = id,
            UserId = blogControllerFixture.UserId,
            Title = "Title",
            Details = "Details",
            CreationDate = DateTime.UtcNow,
            EditDate = null
        };

        await blogControllerFixture.BlogDbContext.Blogs.AddAsync(expected);
        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        var result = (await blogController.Get(id)).Result as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        var actual = result.Value as BlogVm;
        
        actual!.Id.Should().Be(expected.Id);
        actual.Title.Should().BeEquivalentTo(expected.Title);
        actual.Details.Should().BeEquivalentTo(expected.Details);
        actual.CreationDate.Should().BeSameDateAs(expected.CreationDate);
        actual.EditDate.Should().BeNull();

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Get_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.UnauthorizeBlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var id = Guid.NewGuid();
        var wrongId = Guid.NewGuid();

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

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController.Get(Guid.Empty);
        });

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await blogController.Get(wrongId);
        });

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }
}