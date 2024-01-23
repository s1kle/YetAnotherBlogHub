using BlogHub.Api.Controllers;
using BlogHub.Data.Articles.List.Helpers;
using BlogHub.Data.Tags.Get.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Tests.Api.AuthorizeController.Articles;

public class GetAllTests
{
    [Fact]
    public async void GetAll_Empty_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var expected = new OkObjectResult(new ArticleListVm()
        {
            Articles = Array.Empty<ArticleListItemVm>()
        });

        var dto = new ArticleListDto()
        {
            List = new() { Page = 0, Size = 10 }
        };

        var temp = await fixture.Controller.GetAll(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_NotEmpty_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var size = 5;

        var Articles = ArticleFactory.CreateArticles(size, fixture.UserId);

        var expected = new OkObjectResult(new ArticleListVm()
        {
            Articles = Articles.Select(Article => new ArticleListItemVm()
            {
                Title = Article.Title,
                Id = Article.Id,
                CreationDate = Article.CreationDate,
                Author = "null",
                Tags = Array.Empty<TagVm>()
            }).ToList()
        });

        await fixture.AddRangeAsync(Articles);

        var dto = new ArticleListDto()
        {
            List = new() { Page = 0, Size = 10 }
        };

        var temp = await fixture.Controller.GetAll(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_WithSearchFilter_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var searchQuery = "searched";
        var searchProperties = "title";

        var size = 5;

        var searchedArticles = ArticleFactory.CreateArticles(size, fixture.UserId, searchQuery);
        var Articles = ArticleFactory.CreateArticles(size, fixture.UserId);

        var expected = new OkObjectResult(new ArticleListVm()
        {
            Articles = searchedArticles.Select(Article => new ArticleListItemVm()
            {
                Title = Article.Title,
                Author = "null",
                Id = Article.Id,
                CreationDate = Article.CreationDate,
                Tags = Array.Empty<TagVm>()
            }).ToList()
        });

        await fixture.AddRangeAsync(Articles);
        await fixture.AddRangeAsync(searchedArticles);

        var dto = new ArticleListDto()
        {
            List = new() { Page = 0, Size = 10 },
            Search = new()
            {
                Query = searchQuery,
                Properties = searchProperties
            }
        };

        var temp = await fixture.Controller.GetAll(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_WithSortFilter_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var sortProperty = "title";
        var sortDirection = "desc";

        var size = 5;

        var Articles = ArticleFactory.CreateArticles(size, fixture.UserId);

        var expected = new OkObjectResult(new ArticleListVm()
        {
            Articles = Articles.Select(Article => new ArticleListItemVm()
            {
                Title = Article.Title,
                Author = "null",
                Id = Article.Id,
                CreationDate = Article.CreationDate,
                Tags = Array.Empty<TagVm>()
            }).OrderByDescending(Article => Article.Title).ToList()
        });

        await fixture.AddRangeAsync(Articles);

        var dto = new ArticleListDto()
        {
            List = new() { Page = 0, Size = 10 },
            Sort = new()
            {
                Property = sortProperty,
                Direction = sortDirection
            }
        };

        var temp = await fixture.Controller.GetAll(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_NotEmptyWithExtraArticles_ShouldSuccess()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var size = 5;

        var Articles = ArticleFactory.CreateArticles(size, fixture.UserId);
        var extraArticles = ArticleFactory.CreateArticles(size, Guid.NewGuid());

        var expected = new OkObjectResult(new ArticleListVm()
        {
            Articles = Articles.Select(Article => new ArticleListItemVm()
            {
                Title = Article.Title,
                Author = "null",
                Id = Article.Id,
                CreationDate = Article.CreationDate,
                Tags = Array.Empty<TagVm>()
            }).ToList()
        });

        await fixture.AddRangeAsync(Articles);
        await fixture.AddRangeAsync(extraArticles);

        var dto = new ArticleListDto()
        {
            List = new() { Page = 0, Size = 10 }
        };

        var temp = await fixture.Controller.GetAll(dto);

        var result = temp.Result as OkObjectResult;

        result.Should().BeEquivalentTo(expected);

        fixture.EnsureDeleted();
    }

    [Fact]
    public async void GetAll_WithInvalidQuery_ShouldFail()
    {
        var fixture = ControllerFactory.CreateFixture<AuthorizeArticleController>();
        fixture.EnsureCreated();

        var dto = new ArticleListDto()
        {
            List = new() { Page = 0, Size = 0 }
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.GetAll(dto);
        });

        dto = new ArticleListDto()
        {
            List = new() { Page = -1, Size = 10 }
        };

        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await fixture.Controller.GetAll(dto);
        });

        fixture.EnsureDeleted();
    }
}