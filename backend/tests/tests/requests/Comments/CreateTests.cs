namespace BlogHub.Tests.Requests.Comments;

public class CreateTests
{
    [Fact]
    public async Task CreateComment_ShouldSuccess()
    {
        var blog = BlogFactory.CreateBlog("title", Guid.NewGuid());

        var expected = CommentFactory.CreateComment(blog, blog.UserId);

        var command = new CreateCommentCommand()
        {
            UserId = expected.UserId,
            BlogId = expected.BlogId,
            Content = expected.Content
        };

        var repository = A.Fake<ICommentRepository>();
        var blogRepository = A.Fake<IBlogRepository>();

        A.CallTo(() => repository.CreateAsync(A<Comment>._, A<CancellationToken>._))
            .Returns(expected.Id);
        A.CallTo(() => blogRepository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);

        var handler = new CreateCommentCommandHandler(repository, blogRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.CreateAsync(A<Comment>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Comment actual, CancellationToken token) =>
            {
                actual.UserId.Should().Be(expected.UserId);
                actual.BlogId.Should().Be(expected.BlogId);
                actual.Content.Should().BeEquivalentTo(expected.Content);
                actual.CreationDate.Should().BeOnOrAfter(expected.CreationDate);

                token.Should().Be(CancellationToken.None);

                return true;
            })
            .MustHaveHappenedOnceExactly();
    }
}