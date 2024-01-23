using BlogHub.Api.Controllers;
using BlogHub.Data.Articles.Update.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Tests.Api.AuthorizeController.Articles;

public class UpdateTests
{
    [Fact]
    public async void Update_WithValidData_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var Article = ArticleFactory.CreateArticle("1234567890", fixture.UserId);

        await fixture.AddRangeAsync(new[] { Article });

        var expected = Article with
        {
            Title = "New title for Article",
            EditDate = DateTime.UtcNow
        };

        var dto = new UpdateArticleDto()
        {
            Title = expected.Title
        };

        var temp = await fixture.Controller.Update(Article.Id, dto);

        var result = temp.Result as OkObjectResult;

        result.Should().NotBeNull();

        var actual = await fixture.BlogHubDbContext.Articles.FirstOrDefaultAsync();

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
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var Article = ArticleFactory.CreateArticle("1234567890", fixture.UserId);

        await fixture.AddRangeAsync(new[] { Article });

        var dto = new UpdateArticleDto()
        {
            Title = "123"
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.Update(Article.Id, dto);
        });

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Update_WithInvalidArticleId_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var dto = new UpdateArticleDto()
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