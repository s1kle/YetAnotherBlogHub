using BlogHub.Data.Articles.List.Helpers;

namespace BlogHub.Tests.Requests.Articles;

public class GetUserArticlesTests
{
    [Fact]
    public async Task GetArticleList_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var Articles = ArticleFactory.CreateArticles(size, Guid.NewGuid());

        var expected = Articles.Select(Article => new ArticleListItemVm()
        {
            UserId = Guid.Empty,
            Id = Article.Id,
            Title = Article.Title,
            CreationDate = Article.CreationDate
        }).ToList();

        var query = new GetUserArticlesQuery()
        {
            UserId = Articles[0].UserId,
            Page = 0,
            Size = 10
        };

        var repository = A.Fake<IArticleRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAllByUserIdAsync(A<Guid>._, A<int>._, A<int>._, A<CancellationToken>._))
            .Returns(Articles);

        A.CallTo(() => mapper.Map<ArticleListItemVm>(A<Article>._))
            .ReturnsNextFromSequence(expected.ToArray());

        var handler = new GetUserArticlesQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAllByUserIdAsync(A<Guid>._, A<int>._, A<int>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid userId, int page, int size, CancellationToken token) =>
            {
                userId.Should().Be(Articles[0].UserId);
                page.Should().Be(query.Page);
                size.Should().Be(query.Size);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => mapper.Map<ArticleListItemVm>(A<Article>._))
            .WhenArgumentsMatch((object arg) =>
            {
                var actual = (Article?)arg;
                Articles.IndexOf(actual!).Should().NotBe(-1);
                return true;
            })
            .MustHaveHappened(size, Times.Exactly);

        var actual = result.Articles;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}