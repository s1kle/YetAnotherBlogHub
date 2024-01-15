using BlogHub.Data.Articles.List.Helpers;
using BlogHub.Data.Articles.List.Helpers.Sort;

namespace BlogHub.Tests.Requests.Articles;

public class ListSortTests
{
    [Fact]
    public async Task ListSort_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var Articles = ArticleFactory.CreateArticles(size).Select(Article => new ArticleListItemVm()
        {
            UserId = Guid.Empty,
            Id = Article.Id,
            Title = Article.Title,
            CreationDate = Article.CreationDate
        });

        var expected = Articles.OrderBy(Article => Article.CreationDate).ToList();

        var query = new ArticleListSortQuery()
        {
            Articles = new() { Articles = Articles.ToList() },
            Property = "CreationDate",
            Descending = false
        };

        var handler = new ArticleListSortQueryHandler();

        var result = await handler.Handle(query, CancellationToken.None);

        var actual = result.Articles;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);

        expected = Articles.OrderByDescending(Article => Article.Title).ToList();

        query = new ArticleListSortQuery()
        {
            Articles = new() { Articles = Articles.ToList() },
            Property = "title",
            Descending = true
        };

        result = await handler.Handle(query, CancellationToken.None);

        actual = result.Articles;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ListSort_WithIncorrectSortProperty_ShouldSuccess()
    {
        var size = 10;

        var Articles = ArticleFactory.CreateArticles(size).Select(Article => new ArticleListItemVm()
        {
            UserId = Guid.Empty,
            Id = Article.Id,
            Title = Article.Title,
            CreationDate = Article.CreationDate
        });

        var expected = Articles.ToList();

        var query = new ArticleListSortQuery()
        {
            Articles = new() { Articles = Articles.ToList() },
            Property = "details",
            Descending = false
        };

        var handler = new ArticleListSortQueryHandler();

        var result = await handler.Handle(query, CancellationToken.None);

        var actual = result.Articles;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}