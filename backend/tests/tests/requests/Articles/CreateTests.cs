namespace BlogHub.Tests.Requests.Articles;

public class CreateTests
{
    [Fact]
    public async Task CreateArticle_ShouldSuccess()
    {
        var expected = ArticleFactory.CreateArticle("title");

        var command = new CreateArticleCommand()
        {
            UserId = expected.UserId,
            Title = expected.Title
        };

        var repository = A.Fake<IArticleRepository>();

        A.CallTo(() => repository.CreateAsync(A<Article>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new CreateArticleCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.CreateAsync(A<Article>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Article actual, CancellationToken token) =>
            {
                actual.UserId.Should().Be(expected.UserId);
                actual.Title.Should().BeEquivalentTo(expected.Title);
                actual.CreationDate.Should().BeOnOrAfter(expected.CreationDate);
                actual.Details.Should().BeNull();
                actual.EditDate.Should().BeNull();

                token.Should().Be(CancellationToken.None);

                return true;
            })
            .MustHaveHappenedOnceExactly();
    }
}