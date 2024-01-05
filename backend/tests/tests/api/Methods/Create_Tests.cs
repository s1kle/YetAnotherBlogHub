using BlogHub.Data.Commands.Create;
using BlogHub.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.Methods;

public class Create_Tests
{
    private readonly FixtureFactory _fixtureFactory;
    private readonly string _dbContextName;

    public Create_Tests()
    {
        _fixtureFactory = new ();
        _dbContextName = $"{this.GetType()}";
    }

    [Fact]
    public async Task Create_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.AuthorizeBlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var count = 5;
        var expected = new List<Blog>();
        var dtoList = new List<CreateBlogDto>();

        for (var i = 0; i < count; i++)
        {
            var title = $"Title{i}";
            var details = $"Details{i}";

            var blog = new Blog()
            {
                Id = Guid.NewGuid(),
                UserId = blogControllerFixture.UserId,
                Title = title,
                Details = details,
                CreationDate = DateTime.UtcNow,
                EditDate = null
            };

            dtoList.Add(new ()
            {
                Title = title,
                Details = details
            });

            expected.Add(blog);
        }

        foreach (var command in dtoList)
        {
            var result = (await blogController.Create(command)).Result as OkObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        var actual =  blogControllerFixture.BlogDbContext.Blogs.ToList();

        actual.Count().Should().Be(5);

        for (var i = 0; i < count; i++)
        {
            actual[i].UserId.Should().Be(expected[i].UserId);
            actual[i].Title.Should().BeEquivalentTo(expected[i].Title);
            actual[i].Details.Should().BeEquivalentTo(expected[i].Details);
            actual[i].CreationDate.Should().BeOnOrAfter(expected[i].CreationDate);
            actual[i].EditDate.Should().BeNull();
        }

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Create_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture(_dbContextName);
        var blogController = blogControllerFixture.AuthorizeBlogController;

        blogControllerFixture.BlogDbContext.Database.EnsureCreated();

        var createBlogDto = new CreateBlogDto()
        {
            Title = new ('a', 101),
            Details = new ('a', 101)
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await blogController.Create(createBlogDto);
        });

        blogControllerFixture.BlogDbContext.Database.EnsureDeleted();
    }
}