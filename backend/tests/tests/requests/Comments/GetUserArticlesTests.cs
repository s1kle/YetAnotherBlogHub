using BlogHub.Data.Comments.List.Helpers;

namespace BlogHub.Tests.Requests.Comments;

public class GetArticleCommentsTests
{
    [Fact]
    public async Task GetCommentList_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var Article = ArticleFactory.CreateArticle(userId: Guid.NewGuid());

        var comments = CommentFactory.CreateComments(size, Article, Article.UserId);

        var expected = comments.Select(comment => new CommentVm()
        {
            UserId = comment.UserId,
            Id = comment.Id,
            Content = comment.Content,
            CreationDate = Article.CreationDate
        }).ToList();

        var query = new GetArticleCommentsQuery()
        {
            ArticleId = Article.Id
        };

        var repository = A.Fake<ICommentRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAllByArticleIdAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(comments);

        A.CallTo(() => mapper.Map<CommentVm>(A<Comment>._))
            .ReturnsNextFromSequence(expected.ToArray());

        var handler = new GetArticleCommentsQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAllByArticleIdAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid ArticleId, CancellationToken token) =>
            {
                ArticleId.Should().Be(Article.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => mapper.Map<CommentVm>(A<Comment>._))
            .WhenArgumentsMatch((object arg) =>
            {
                var actual = (Comment?)arg;
                comments.IndexOf(actual!).Should().NotBe(-1);
                return true;
            })
            .MustHaveHappened(size, Times.Exactly);

        var actual = result;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}