using BlogHub.Data.Articles.Get.Helpers;

namespace BlogHub.Tests.Requests.Articles;

public class GetTests
{
    [Fact]
    public async Task GetArticle_WithCorrectData_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var Article = ArticleFactory.CreateArticle("title", userId);

        var expected = new ArticleVm()
        {
            UserId = userId,
            Id = Article.Id,
            Title = Article.Title,
            CreationDate = Article.CreationDate
        };

        var query = new GetArticleQuery()
        {
            Id = Article.Id
        };

        var repository = A.Fake<IArticleRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);

        A.CallTo(() => mapper.Map<ArticleVm>(A<Article>._))
            .Returns(expected);

        var handler = new GetArticleQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(query.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetArticle_WithIncorrectArticleId_ShouldFail()
    {
        Article? Article = null;

        var query = new GetArticleQuery()
        {
            Id = Guid.NewGuid()
        };

        var repository = A.Fake<IArticleRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);

        var handler = new GetArticleQueryHandler(repository, mapper);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(query, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => mapper.Map<ArticleVm>(A<Article>._))
            .MustNotHaveHappened();
    }
}