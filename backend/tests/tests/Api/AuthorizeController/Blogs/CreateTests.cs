using BlogHub.Api.Controllers;
using BlogHub.Data.Blogs.Create.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Tests.Api.AuthorizeController.Blogs;

public class CreateTests
{
    [Fact]
    public async void Create_WithValidData_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var expected = BlogFactory.CreateBlog("1234567890", fixture.UserId);

        var dto = new CreateBlogDto()
        {
            Title = expected.Title,
            Details = expected.Details
        };

        var temp = await fixture.Controller.Create(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().NotBeNull();

        var actual = await fixture.BlogDbContext.Blogs.FirstOrDefaultAsync();

        actual.Should().NotBeNull();
        actual!.Title.Should().BeEquivalentTo(expected.Title);
        actual!.Details.Should().BeEquivalentTo(expected.Details);
        actual!.CreationDate.Should().BeOnOrAfter(expected.CreationDate);
        actual!.EditDate.Should().BeNull();
        actual!.UserId.Should().Be(expected.UserId);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Create_WithInvalidTitle_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var blog = BlogFactory.CreateBlog(userId: fixture.UserId);

        var dto = new CreateBlogDto()
        {
            Title = blog.Title,
            Details = blog.Details
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.Create(dto);
        });

        fixture.EnsureDeleted();
    }
}