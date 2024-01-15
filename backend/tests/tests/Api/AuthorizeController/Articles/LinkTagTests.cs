using BlogHub.Api.Controllers;
using BlogHub.Data.Tags.Link.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Tests.Api.AuthorizeController.Articles;

public class LinkTagTests
{
    [Fact]
    public async void LinkTag_WithValidData_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var Article = ArticleFactory.CreateArticle("123", fixture.UserId);
        var tag = TagFactory.CreateTag("Test tag");

        await fixture.AddRangeAsync(new[] { Article });
        await fixture.AddRangeAsync(new[] { tag });

        var dto = new LinkTagDto { TagId = tag.Id };

        var temp = await fixture.Controller.LinkTag(Article.Id, dto);

        var result = temp.Result as OkObjectResult;

        temp.Should().NotBeNull();

        var linkedTag = await fixture.ArticleTagDbContext.Links.FirstOrDefaultAsync();

        linkedTag.Should().NotBeNull();

        fixture.EnsureDeleted();
    }

    [Fact]
    public async Task LinkTag_WithInvalidTagId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();

        var dto = new LinkTagDto { TagId = Guid.Empty };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.LinkTag(Guid.NewGuid(), dto);
        });
    }

    [Fact]
    public async Task LinkTag_WithNonExistentTag_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var dto = new LinkTagDto { TagId = Guid.NewGuid() };

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await fixture.Controller.LinkTag(Guid.NewGuid(), dto);
        });

        fixture.EnsureDeleted();
    }
}