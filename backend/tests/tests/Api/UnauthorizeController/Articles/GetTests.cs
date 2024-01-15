using BlogHub.Api.Controllers;
using BlogHub.Data.Articles.Get.Helpers;
using BlogHub.Data.Articles.List.Helpers;
using BlogHub.Data.Comments.List.Helpers;
using BlogHub.Data.Tags.Get.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.UnauthorizeController.Articles;

public class GetTests
{
    [Fact]
    public async void Get_NotEmpty_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<UnauthorizeArticleController>();
        fixture.EnsureCreated();

        var Article = ArticleFactory.CreateArticle(userId: fixture.UserId);
        await fixture.AddRangeAsync(new[] { Article });

        var expected = new OkObjectResult(new ArticleVm()
        {
            UserId = Article.UserId,
            Title = Article.Title,
            Details = Article.Details,
            Id = Article.Id,
            CreationDate = Article.CreationDate,
            Tags = Array.Empty<TagVm>(),
            Comments = Array.Empty<CommentVm>()
        });

        var temp = await fixture.Controller.Get(Article.Id);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Get_WithInvalidArticleId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<UnauthorizeArticleController>();

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.Get(Guid.Empty);
        });
    }
    [Fact]
    public async void Get_WithEmptyContext_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<UnauthorizeArticleController>();
        fixture.EnsureCreated();

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await fixture.Controller.Get(Guid.NewGuid());
        });

        fixture.EnsureDeleted();
    }
}