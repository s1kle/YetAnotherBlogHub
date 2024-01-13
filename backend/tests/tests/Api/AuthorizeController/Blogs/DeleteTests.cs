using BlogHub.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.AuthorizeController.Blogs;

public class DeleteTests
{
    [Fact]
    public async void Delete_WithValidData_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var blog = BlogFactory.CreateBlog("1234567890", fixture.UserId);

        await fixture.AddRangeAsync(new[] { blog });

        var expected = new OkObjectResult(blog.Id);

        var temp = await fixture.Controller.Delete(blog.Id);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.BlogDbContext.Blogs.Should().BeEmpty();

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Delete_WithEmptyBlogId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.Delete(Guid.Empty);
        });

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Delete_WithInvalidBlogId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();


        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await fixture.Controller.Delete(Guid.NewGuid());
        });

        fixture.EnsureDeleted();
    }
}