using BlogHub.Api.Controllers;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Tests.Api.AuthorizeController.Articles;

public class UnlinkTagTests
{
    [Fact]
    public async Task UnlinkTag_WithLinkedTag_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var Article = ArticleFactory.CreateArticle("123", fixture.UserId);
        var tag = TagFactory.CreateTag("Test tag");
        var link = LinkFactory.CreateArticleTagLink(Article, tag);

        await fixture.AddRangeAsync(new[] { Article });
        await fixture.AddRangeAsync(new[] { tag });
        await fixture.AddRangeAsync(new[] { link });

        await fixture.Controller.UnlinkTag(Article.Id, tag.Id);

        var ArticleTag = await fixture.BlogHubDbContext.ArticleTags.FirstOrDefaultAsync();

        ArticleTag.Should().BeNull();

        fixture.EnsureDeleted();
    }

    [Fact]
    public async Task UnlinkTag_WithNonExistentTag_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
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
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.UnlinkTag(Guid.NewGuid(), Guid.Empty);
        });
    }
}