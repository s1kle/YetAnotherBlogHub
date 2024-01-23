namespace BlogHub.Tests.Requests.Articles;

public class DeleteTests
{
    [Fact]
    public async Task DeleteArticle_WithCorrectData_ShouldSuccess()
    {
        var expected = ArticleFactory.CreateArticle();

        var command = new DeleteArticleCommand()
        {
            UserId = expected.UserId,
            Id = expected.Id
        };

        var repository = A.Fake<IArticleRepository>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(expected);
        A.CallTo(() => repository.RemoveAsync(A<Article>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new DeleteArticleCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.RemoveAsync(A<Article>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Article actual, CancellationToken token) =>
            {
                actual.Should().BeEquivalentTo(expected);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteArticle_WithIncorrectUserId_ShouldFail()
    {
        var Article = ArticleFactory.CreateArticle("title");

        var command = new DeleteArticleCommand()
        {
            UserId = Guid.NewGuid(),
            Id = Article.Id,
        };

        var repository = A.Fake<IArticleRepository>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);

        var handler = new DeleteArticleCommandHandler(repository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.RemoveAsync(A<Article>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task DeleteArticle_WithIncorrectArticleId_ShouldFail()
    {
        Article? Article = null;

        var command = new DeleteArticleCommand()
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };

        var repository = A.Fake<IArticleRepository>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);

        var handler = new DeleteArticleCommandHandler(repository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.RemoveAsync(A<Article>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
}