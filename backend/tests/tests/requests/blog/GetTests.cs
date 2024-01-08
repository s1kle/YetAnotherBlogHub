using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;

namespace BlogHub.Tests.Requests.Blogs;

public class GetTests
{
    [Fact]
    public async Task GetBlog_WithCorrectData_ShouldSuccess()
    {
        var blog = BlogFactory.CreateBlog("title");

        var expected = new BlogVm()
        {
            Id = blog.Id,
            Title = blog.Title,
            CreationDate = blog.CreationDate
        };

        var query = new GetBlogQuery()
        {
            Id = blog.Id
        };

        var repository = A.Fake<IBlogRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);

        A.CallTo(() => mapper.Map<BlogVm>(A<Blog>._))
            .Returns(expected);

        var handler = new GetBlogQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .WhenArgumentsMatch((Guid id, CancellationToken token) =>
            {
                id.Should().Be(query.Id);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => mapper.Map<BlogVm>(A<Blog>._))
            .WhenArgumentsMatch((object arg) =>
            {
                var actual = (Blog)arg;
                actual.Should().BeEquivalentTo(expected);
                return true;
            })
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBlog_WithIncorrectBlogId_ShouldFail()
    {
        Blog? blog = null;

        var query = new GetBlogQuery()
        {
            Id = Guid.NewGuid()
        };

        var repository = A.Fake<IBlogRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .Returns(blog);
            
        var handler = new GetBlogQueryHandler(repository, mapper);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await handler.Handle(query, CancellationToken.None);
        });

        A.CallTo(() => repository.GetAsync(A<Guid>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => mapper.Map<BlogVm>(A<Blog>._))
            .MustNotHaveHappened();
    }
}