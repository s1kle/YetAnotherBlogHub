using BlogHub.Data.Blogs.Commands.Delete;
using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;

namespace BlogHub.Tests.Requests.Blogs;

public class DeleteTests
{
    [Fact]
    public async Task DeleteBlog_WithCorrectData_ShouldSuccess()
    {
        var expected = BlogFactory.CreateBlog();

        var command = new DeleteBlogCommand()
        {
            UserId = expected.UserId,
            Id = expected.Id
        };

        var repository = A.Fake<IBlogRepository>();
        
        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(expected);
        A.CallTo(() => repository.RemoveAsync(A<Blog>._, A<CancellationToken>._))
            .Returns(expected.Id);

        var handler = new DeleteBlogCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(command.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => repository.RemoveAsync(A<Blog>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Blog actual, CancellationToken token) =>
            {
                actual.Should().BeEquivalentTo(expected);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteBlog_WithIncorrectUserId_ShouldFail()
    {
        var blog = BlogFactory.CreateBlog("title");

        var command = new DeleteBlogCommand()
        {
            UserId = Guid.NewGuid(),
            Id = blog.Id,
        };

        var repository = A.Fake<IBlogRepository>();
        
        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);

        var handler = new DeleteBlogCommandHandler(repository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.RemoveAsync(A<Blog>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task DeleteBlog_WithIncorrectBlogId_ShouldFail()
    {
        Blog? blog = null;

        var command = new DeleteBlogCommand()
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };

        var repository = A.Fake<IBlogRepository>();
        
        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);

        var handler = new DeleteBlogCommandHandler(repository);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(command, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => repository.RemoveAsync(A<Blog>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }
}