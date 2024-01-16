using BlogHub.Data.Articles.List.Helpers;
using BlogHub.Data.Articles.List.Helpers.Search;

namespace BlogHub.Tests.Requests.Articles;

public class ListSearchTests
{
    [Fact]
    public async Task ListSearch_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var Articles = ArticleFactory.CreateArticles(size).Select(Article => new ArticleListItemVm()
        {
            Id = Article.Id,
            Title = Article.Title,
            CreationDate = Article.CreationDate
        });

        var expected = Articles.Where(Article => Article.Title.Contains("title", StringComparison.OrdinalIgnoreCase)).ToList();

        var query = new ArticleListSearchQuery()
        {
            Articles = new() { Articles = Articles.ToList() },
            Query = "title",
            Properties = new[] { "title" }
        };

        var handler = new ArticleListSearchQueryHandler();

        var result = await handler.Handle(query, CancellationToken.None);

        var actual = result.Articles;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ListSearch_WithIncorrectSearchQuery_ShouldSuccess()
    {
        var size = 10;

        var Articles = ArticleFactory.CreateArticles(size).Select(Article => new ArticleListItemVm()
        {
            Id = Article.Id,
            Title = Article.Title,
            CreationDate = Article.CreationDate
        });

        var expected = Articles.ToList();

        var query = new ArticleListSearchQuery()
        {
            Articles = new() { Articles = Articles.ToList() },
            Query = "    ",
            Properties = new[] { "title" }
        };

        var handler = new ArticleListSearchQueryHandler();

        var result = await handler.Handle(query, CancellationToken.None);

        var actual = result.Articles;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ListSearch_WithIncorrectSearchProperties_ShouldSuccess()
    {
        var size = 10;

        var Articles = ArticleFactory.CreateArticles(size).Select(Article => new ArticleListItemVm()
        {
            Id = Article.Id,
            Title = Article.Title,
            CreationDate = Article.CreationDate
        });

        var expected = Articles.ToList();

        var query = new ArticleListSearchQuery()
        {
            Articles = new() { Articles = Articles.ToList() },
            Query = "title",
            Properties = Array.Empty<string>()
        };

        var handler = new ArticleListSearchQueryHandler();

        var result = await handler.Handle(query, CancellationToken.None);

        var actual = result.Articles;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}