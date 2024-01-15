namespace BlogHub.Tests.Requests.Articles;

public class UpdateTests
{
    [Fact]
    public async Task UpdateArticle_WithCorrectData_ShouldSuccess()
    {
        var Article = ArticleFactory.CreateArticle("title");

        var expected = new Article()
        {
            UserId = Article.UserId,
            Id = Article.Id,
            Title = "New Title",
            CreationDate = Article.CreationDate,
            Details = "Details",
            EditDate = DateTime.UtcNow
        };

        var command = new UpdateArticleCommand()
        {
            UserId = Article.UserId,
            Id = Article.Id,
            Title = "New Title",
            Details = "Details"
        };

        var repository = A.Fake<IArticleRepository>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);
        A.CallTo(() => repository.UpdateAsync(A<Article>._, A<Article>._, A<CancellationToken>._))
            .Returns(Article.Id);

        var handler = new UpdateArticleCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.UpdateAsync(A<Article>._, A<Article>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Article original, Article actual, CancellationToken token) =>
            {
                original.Should().BeEquivalentTo(Article);

                actual.UserId.Should().Be(expected.UserId);
                actual.Title.Should().BeEquivalentTo(expected.Title);
                actual.CreationDate.Should().BeOneOf(expected.CreationDate);
                actual.Details.Should().BeEquivalentTo(expected.Details);
                actual.EditDate.Should().BeOnOrAfter(expected.EditDate.Value);

                return true;
            })
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateArticle_WithIncorrectArticleId_ShouldFail()
    {
        Article? Article = null;

        var command = new UpdateArticleCommand()
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            Title = "New Title",
            Details = "Details"
        };

        var repository = A.Fake<IArticleRepository>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);

        var handler = new UpdateArticleCommandHandler(repository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.UpdateAsync(A<Article>._, A<Article>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UpdateArticle_WithIncorrectUserId_ShouldFail()
    {
        var Article = ArticleFactory.CreateArticle("title");

        var command = new UpdateArticleCommand()
        {
            UserId = Guid.NewGuid(),
            Id = Article.Id,
            Title = "New Title",
            Details = "Details"
        };

        var repository = A.Fake<IArticleRepository>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);

        var handler = new UpdateArticleCommandHandler(repository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.UpdateAsync(A<Article>._, A<Article>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
}