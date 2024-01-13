using BlogHub.Api.Controllers;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Tests.Api.AuthorizeController.Blogs;

public class UnlinkTagTests
{
    [Fact]
    public async Task UnlinkTag_WithLinkedTag_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        var blog = BlogFactory.CreateBlog("123", fixture.UserId);
        var tag = TagFactory.CreateTag("Test tag");
        var link = LinkFactory.CreateBlogTagLink(blog, tag);

        await fixture.AddRangeAsync(new[] { blog });
        await fixture.AddRangeAsync(new[] { tag });
        await fixture.AddRangeAsync(new[] { link });

        await fixture.Controller.UnlinkTag(blog.Id, tag.Id);

        var blogTag = await fixture.BlogTagDbContext.Links.FirstOrDefaultAsync();

        blogTag.Should().BeNull();

        fixture.EnsureDeleted();
    }

    [Fact]
    public async Task UnlinkTag_WithNonExistentTag_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();
        fixture.EnsureCreated();

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await fixture.Controller.UnlinkTag(Guid.NewGuid(), Guid.NewGuid());
        });

        fixture.EnsureDeleted();
    }

    [Fact]
    public async Task UnlinkTag_WithInvalidTagId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeBlogController>();

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.UnlinkTag(Guid.NewGuid(), Guid.Empty);
        });
    }
}