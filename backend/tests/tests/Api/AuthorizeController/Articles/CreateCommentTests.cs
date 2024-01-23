using BlogHub.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Tests.Api.AuthorizeController.Articles;

public class CreateCommentTests
{
    [Fact]
    public async Task CreateComment_WithValidData_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var Article = ArticleFactory.CreateArticle("123", fixture.UserId);
        await fixture.AddRangeAsync(new[] { Article });

        var dto = new CreateCommentDto { Content = "Test comment" };

        var temp = await fixture.Controller.CreateComment(Article.Id, dto);

        var result = temp.Result as OkObjectResult;

        result.Should().NotBeNull();

        var created = await fixture.BlogHubDbContext.Comments.FirstOrDefaultAsync();

        created.Should().NotBeNull();

        fixture.EnsureDeleted();
    }

    [Fact]
    public async Task CreateComment_WithNonExistentArticle_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var dto = new CreateCommentDto { Content = "Test comment" };

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await fixture.Controller.CreateComment(Guid.NewGuid(), dto);
        });

        fixture.EnsureDeleted();
    }

    [Fact]
    public async Task CreateComment_WithInvalidArticleId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();

        var dto = new CreateCommentDto { Content = "Test comment" };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.CreateComment(Guid.Empty, dto);
        });
    }
}