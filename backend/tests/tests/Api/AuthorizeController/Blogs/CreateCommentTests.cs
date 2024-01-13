using BlogHub.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Tests.Api.AuthorizeController.Blogs;

public class CreateCommentTests
{
    [Fact]
    public async Task CreateComment_WithValidData_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var blog = BlogFactory.CreateBlog("123", fixture.UserId);
        await fixture.AddRangeAsync(new[] { blog });

        var dto = new CreateCommentDto { Content = "Test comment" };

        var temp = await fixture.Controller.CreateComment(blog.Id, dto);

        var result = temp.Result as OkObjectResult;

        result.Should().NotBeNull();

        var created = await fixture.CommentDbContext.Comments.FirstOrDefaultAsync();

        created.Should().NotBeNull();

        fixture.EnsureDeleted();
    }

    [Fact]
    public async Task CreateComment_WithNonExistentBlog_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var dto = new CreateCommentDto { Content = "Test comment" };

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await fixture.Controller.CreateComment(Guid.NewGuid(), dto);
        });

        fixture.EnsureDeleted();
    }

    [Fact]
    public async Task CreateComment_WithInvalidBlogId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();

        var dto = new CreateCommentDto { Content = "Test comment" };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.CreateComment(Guid.Empty, dto);
        });
    }
}