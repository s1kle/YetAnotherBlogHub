namespace BlogHub.Tests.Requests.Comments;

public class CreateTests
{
    [Fact]
    public async Task CreateComment_ShouldSuccess()
    {
        var Article = ArticleFactory.CreateArticle("title", Guid.NewGuid());

        var expected = CommentFactory.CreateComment(Article, Article.UserId);

        var command = new CreateCommentCommand()
        {
            UserId = expected.UserId,
            ArticleId = expected.ArticleId,
            Content = expected.Content
        };

        var repository = A.Fake<ICommentRepository>();
        var ArticleRepository = A.Fake<IArticleRepository>();

        A.CallTo(() => repository.CreateAsync(A<Comment>._, A<CancellationToken>._))
            .Returns(expected.Id);
        A.CallTo(() => ArticleRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(Article);

        var handler = new CreateCommentCommandHandler(repository, ArticleRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.CreateAsync(A<Comment>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Comment actual, CancellationToken token) =>
            {
                actual.UserId.Should().Be(expected.UserId);
                actual.ArticleId.Should().Be(expected.ArticleId);
                actual.Content.Should().BeEquivalentTo(expected.Content);
                actual.CreationDate.Should().BeOnOrAfter(expected.CreationDate);

                token.Should().Be(CancellationToken.None);

                return true;
            })
            .MustHaveHappenedOnceExactly();
    }
}