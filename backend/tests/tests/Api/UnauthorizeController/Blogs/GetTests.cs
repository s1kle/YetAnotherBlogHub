using BlogHub.Api.Controllers;
using BlogHub.Data.Blogs.Get.Helpers;
using BlogHub.Data.Blogs.List.Helpers;
using BlogHub.Data.Comments.List.Helpers;
using BlogHub.Data.Tags.Get.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.UnauthorizeController.Blogs;

public class GetTests
{
    [Fact]
    public async void Get_NotEmpty_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<UnauthorizeBlogController>();
        fixture.EnsureCreated();

        var blog = BlogFactory.CreateBlog(userId: fixture.UserId);
        await fixture.AddRangeAsync(new[] { blog });

        var expected = new OkObjectResult(new BlogVm()
        {
            UserId = blog.UserId,
            Title = blog.Title,
            Details = blog.Details,
            Id = blog.Id,
            CreationDate = blog.CreationDate,
            Tags = Array.Empty<TagVm>(),
            Comments = Array.Empty<CommentVm>()
        });

        var temp = await fixture.Controller.Get(blog.Id);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Get_WithInvalidBlogId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<UnauthorizeBlogController>();

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.Get(Guid.Empty);
        });
    }
    [Fact]
    public async void Get_WithEmptyContext_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<UnauthorizeBlogController>();
        fixture.EnsureCreated();

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await fixture.Controller.Get(Guid.NewGuid());
        });

        fixture.EnsureDeleted();
    }
}