namespace BlogHub.Tests.Requests.Comments;

public class DeleteTests
{
    [Fact]
    public async Task DeleteComment_WithCorrectData_ShouldSuccess()
    {
        var blog = BlogFactory.CreateBlog("title", Guid.NewGuid());

        var expected = CommentFactory.CreateComment(blog, blog.UserId);

        var command = new DeleteCommentCommand()
        {
            UserId = expected.UserId,
            Id = expected.Id
        };

        var repository = A.Fake<ICommentRepository>();
        
        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(expected);
        A.CallTo(() => repository.RemoveAsync(A<Comment>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new DeleteCommentCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.RemoveAsync(A<Comment>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Comment actual, CancellationToken token) =>
            {
                actual.Should().BeEquivalentTo(expected);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteComment_WithIncorrectUserId_ShouldFail()
    {
        var blog = BlogFactory.CreateBlog("title", Guid.NewGuid());

        var comment = CommentFactory.CreateComment(blog, blog.UserId);

        var command = new DeleteCommentCommand()
        {
            UserId = Guid.NewGuid(),
            Id = comment.Id
        };

        var repository = A.Fake<ICommentRepository>();
        
        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(comment);

        var handler = new DeleteCommentCommandHandler(repository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.RemoveAsync(A<Comment>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task DeleteComment_WithIncorrectCommentId_ShouldFail()
    {
        Comment? comment = null;

        var command = new DeleteCommentCommand()
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };

        var repository = A.Fake<ICommentRepository>();
        
        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(comment);

        var handler = new DeleteCommentCommandHandler(repository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.RemoveAsync(A<Comment>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
}