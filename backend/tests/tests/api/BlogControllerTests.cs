using BlogHub.Data.Queries.GetList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api;

public class BlogControllerTests
{
    private readonly FixtureFactory _fixtureFactory;

    public BlogControllerTests()
    {
        _fixtureFactory = new ();
    }

    [Fact]
    public async Task GetAllFromEmptyList_WithValidParams_ShouldSuccess()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.EnsureCreated();

        var dto = new GetListDto() 
        {
            Page = 0,
            Size = 10
        };

        var result = (await blogController.GetAll(dto)).Result as OkObjectResult;
        var value = result.Value as BlogListVm;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        value.Should().NotBeNull();
        value.Blogs.Should().NotBeNull();
        value.Blogs.Should().BeEmpty();

        blogControllerFixture.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllFromEmptyList_WithInvalidParams_ShouldFail()
    {
        var blogControllerFixture = _fixtureFactory.BlogControllerFixture();
        var blogController = blogControllerFixture.BlogController;

        blogControllerFixture.EnsureCreated();

        var dto = new GetListDto() 
        {
            Page = 0,
            Size = 0
        };

        var result = (await blogController.GetAll(dto)).Result as BadRequestObjectResult;
        var value = result.Value as BlogListVm;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        value.Should().BeNull();

        blogControllerFixture.EnsureDeleted();
    }
}