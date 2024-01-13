namespace BlogHub.Tests.Requests.Blogs;

public class CreateTests
{
    [Fact]
    public async Task CreateBlog_ShouldSuccess()
    {
        var expected = BlogFactory.CreateBlog("title");

        var command = new CreateBlogCommand()
        {
            UserId = expected.UserId,
            Title = expected.Title
        };

        var repository = A.Fake<IBlogRepository>();

        A.CallTo(() => repository.CreateAsync(A<Blog>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new CreateBlogCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.CreateAsync(A<Blog>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Blog actual, CancellationToken token) =>
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