using BlogHub.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.AuthorizeController.Articles;

public class DeleteTests
{
    [Fact]
    public async void Delete_WithValidData_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var Article = ArticleFactory.CreateArticle("1234567890", fixture.UserId);

        await fixture.AddRangeAsync(new[] { Article });

        var expected = new OkObjectResult(Article.Id);

        var temp = await fixture.Controller.Delete(Article.Id);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.BlogHubDbContext.Articles.Should().BeEmpty();

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Delete_WithEmptyArticleId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.Delete(Guid.Empty);
        });

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Delete_WithInvalidArticleId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();


        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await fixture.Controller.Delete(Guid.NewGuid());
        });

        fixture.EnsureDeleted();
    }
}