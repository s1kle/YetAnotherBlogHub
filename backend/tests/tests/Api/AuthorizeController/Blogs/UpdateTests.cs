using BlogHub.Api.Controllers;
using BlogHub.Data.Blogs.Update.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Tests.Api.AuthorizeController.Blogs;

public class UpdateTests
{
    [Fact]
    public async void Update_WithValidData_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var blog = BlogFactory.CreateBlog("1234567890", fixture.UserId);

        await fixture.AddRangeAsync(new[] { blog });

        var expected = blog with
        {
            Title = "New title for blog",
            EditDate = DateTime.UtcNow
        };

        var dto = new UpdateBlogDto()
        {
            Title = expected.Title
        };

        var temp = await fixture.Controller.Update(blog.Id, dto);

        var result = temp.Result as OkObjectResult;

        result.Should().NotBeNull();

        var actual = await fixture.BlogDbContext.Blogs.FirstOrDefaultAsync();

        actual.Should().NotBeNull();
        actual!.Id.Should().Be(expected.Id);
        actual!.Title.Should().BeEquivalentTo(expected.Title);
        actual!.Details.Should().BeEquivalentTo(expected.Details);
        actual!.CreationDate.Should().BeOneOf(expected.CreationDate);
        actual!.EditDate.Should().BeOnOrAfter(expected.EditDate.Value);
        actual!.UserId.Should().Be(expected.UserId);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Update_WithInvalidTitle_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var blog = BlogFactory.CreateBlog("1234567890", fixture.UserId);

        await fixture.AddRangeAsync(new[] { blog });

        var dto = new UpdateBlogDto()
        {
            Title = "123"
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.Update(blog.Id, dto);
        });

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Update_WithInvalidBlogId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var dto = new UpdateBlogDto()
        {
            Title = "1234567890"
        };

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await fixture.Controller.Update(Guid.NewGuid(), dto);
        });

        fixture.EnsureDeleted();
    }
}