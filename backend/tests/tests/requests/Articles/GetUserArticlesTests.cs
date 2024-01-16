using BlogHub.Data.Articles.List.Helpers;

namespace BlogHub.Tests.Requests.Articles;

public class GetUserArticlesTests
{
    [Fact]
    public async Task GetArticleList_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var articles = ArticleFactory.CreateArticles(size, Guid.NewGuid());

        var expected = articles.Select(Article => new ArticleListItemVm()
        {
            Id = Article.Id,
            Title = Article.Title,
            CreationDate = Article.CreationDate,
            Author = "null"
        }).ToList();

        var query = new GetUserArticlesQuery()
        {
            UserId = articles[0].UserId,
            Page = 0,
            Size = 10
        };

        User? user = null;
        List<Tag>? tags = null;

        var repository = A.Fake<IArticleRepository>();
        var tagRepository = A.Fake<ITagRepository>();
        var userRepository = A.Fake<IUserRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAllByUserIdAsync(A<Guid>._, A<int>._, A<int>._, A<CancellationToken>._))
            .Returns(articles);
        A.CallTo(() => userRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(user);
        A.CallTo(() => tagRepository.GetAllByArticleIdAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(tags);

        A.CallTo(() => mapper.Map<ArticleListItemVm>(A<Article>._))
            .ReturnsNextFromSequence(expected.ToArray());

        var handler = new GetUserArticlesQueryHandler(repository, userRepository, tagRepository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAllByUserIdAsync(A<Guid>._, A<int>._, A<int>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid userId, int page, int size, CancellationToken token) =>
            {
                userId.Should().Be(articles[0].UserId);
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
                articles.IndexOf(actual!).Should().NotBe(-1);
                return true;
            })
            .MustHaveHappened(size, Times.Exactly);

        var actual = result.Articles;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}