using BlogHub.Data.Commands.Update;
using BlogHub.Data.Exceptions;
using BlogHub.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.Methods;

public class Update_Tests
{
    private readonly FixtureFactory _fixtureFactory;
    private readonly string _dbContextName;

    public Update_Tests()
    {
        _fixtureFactory = new ();
        _dbContextName = $"{this.GetType()}";
    }

    [Fact]
    public async Task Update_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.AuthorizeBlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var id = Guid.NewGuid();

        var date = DateTime.UtcNow;

        var blog = new Blog()
        {
            Id = id,
            UserId = blogControllerFixture.UserId,
            Title = "Title",
            Details = "Details",
            CreationDate = date,
            EditDate = null
        };

        var newTitle = "new Title";
        var newDetails = "new Details";

        var expected = blog with
        {
            Title = newTitle,
            Details = newDetails,
            EditDate = DateTime.UtcNow
        };

        var dto = new UpdateBlogDto()
        {
            Title = newTitle,
            Details = newDetails
        };

        await blogControllerFixture.BlogDbContext.Blogs.AddAsync(blog);
        await blogControllerFixture.BlogDbContext.SaveChangesAsync();

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        var result = (await blogController.Update(id, dto)).Result as OkObjectResult;
        await blogControllerFixture.BlogDbContext.Entry(blog).ReloadAsync();

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);

        blogControllerFixture.BlogDbContext.Blogs.Count().Should().Be(1);

        var actual = blogControllerFixture.BlogDbContext.Blogs.FirstOrDefault();
        
        actual!.UserId.Should().Be(expected.UserId);
        actual.Id.Should().Be(expected.Id);
        actual.Title.Should().BeEquivalentTo(expected.Title);
        actual.Details.Should().BeEquivalentTo(expected.Details);
        actual.CreationDate.Should().BeOnOrAfter(expected.CreationDate);
        actual.EditDate.Should().BeOnOrAfter((DateTime)expected.EditDate);

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Update_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.AuthorizeBlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var wrongId = Guid.NewGuid();
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

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController
                .Update(Guid.Empty, new () { Title = "T", Details = "D"});
        });

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController
                .Update(id, new () { Title = new ('a', 101), Details = "D"});
        });

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await blogController
                .Update(wrongId, new () { Title = "T", Details = "D"});
        });

        blogControllerFixture.ChangeUser();

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await blogController
                .Update(id, new () { Title = "T", Details = "D"});
        });

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }
}