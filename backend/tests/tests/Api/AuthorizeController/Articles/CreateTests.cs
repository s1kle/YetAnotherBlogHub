using BlogHub.Api.Controllers;
using BlogHub.Data.Articles.Create.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Tests.Api.AuthorizeController.Articles;

public class CreateTests
{
    [Fact]
    public async void Create_WithValidData_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var expected = ArticleFactory.CreateArticle("1234567890", fixture.UserId);

        var dto = new CreateArticleDto()
        {
            Title = expected.Title,
            Details = expected.Details
        };

        var temp = await fixture.Controller.Create(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().NotBeNull();

        var actual = await fixture.ArticleDbContext.Articles.FirstOrDefaultAsync();

        actual.Should().NotBeNull();
        actual!.Title.Should().BeEquivalentTo(expected.Title);
        actual!.Details.Should().BeEquivalentTo(expected.Details);
        actual!.CreationDate.Should().BeOnOrAfter(expected.CreationDate);
        actual!.EditDate.Should().BeNull();
        actual!.UserId.Should().Be(expected.UserId);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void Create_WithInvalidTitle_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var Article = ArticleFactory.CreateArticle(userId: fixture.UserId);

        var dto = new CreateArticleDto()
        {
            Title = Article.Title,
            Details = Article.Details
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.Create(dto);
        });

        fixture.EnsureDeleted();
    }
}