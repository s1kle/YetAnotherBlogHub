using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;

namespace BlogHub.Tests.Requests;

public class GetAllTests
{
    [Fact]
    public async Task GetBlogList_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var blogs = BlogFactory.CreateBlogs(size);

        var expected = blogs.Select(blog => new BlogVmForList()
        {
            Id = blog.Id,
            Title = blog.Title,
            CreationDate = blog.CreationDate
        }).ToList();

        var query = new GetBlogListQuery()
        {
            Page = 0,
            Size = 10
        };

        var repository = A.Fake<IBlogRepository>();
        var mapper = A.Fake<IMapper>();

        A.CallTo(() => repository.GetAllAsync(A<int>._, A<int>._, A<CancellationToken>._))
            .Returns(blogs);
            
        A.CallTo(() => mapper.Map<BlogVmForList>(A<Blog>._))
            .ReturnsNextFromSequence(expected.ToArray());

        var handler = new GetBlogListQueryHandler(repository, mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        A.CallTo(() => repository.GetAllAsync(A<int>._, A<int>._, A<CancellationToken>._))
            .WhenArgumentsMatch((int page, int size, CancellationToken token) =>
            {
                page.Should().Be(query.Page);
                size.Should().Be(query.Size);
                token.Should().Be(CancellationToken.None);
                return true;
            })
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => mapper.Map<BlogVmForList>(A<Blog>._))
            .WhenArgumentsMatch((object arg) =>
            {
                var actual = (Blog?)arg;
                blogs.IndexOf(actual!).Should().NotBe(-1);
                return true;
            })
            .MustHaveHappened(size, Times.Exactly);

        var actual = result.Blogs;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}