using BlogHub.Data.Blogs.List.Helpers;
using BlogHub.Data.Blogs.List.Helpers.Sort;

namespace BlogHub.Tests.Requests.Blogs;

public class ListSortTests
{
    [Fact]
    public async Task ListSort_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var blogs = BlogFactory.CreateBlogs(size).Select(blog => new BlogListItemVm()
        {
            UserId = Guid.Empty,
            Id = blog.Id,
            Title = blog.Title,
            CreationDate = blog.CreationDate
        });

        var expected = blogs.OrderBy(blog => blog.CreationDate).ToList();

        var query = new BlogListSortQuery()
        {
            Blogs = new() { Blogs = blogs.ToList() },
            Property = "CreationDate",
            Descending = false
        };

        var handler = new BlogListSortQueryHandler();

        var result = await handler.Handle(query, CancellationToken.None);

        var actual = result.Blogs;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);

        expected = blogs.OrderByDescending(blog => blog.Title).ToList();

        query = new BlogListSortQuery()
        {
            Blogs = new() { Blogs = blogs.ToList() },
            Property = "title",
            Descending = true
        };

        result = await handler.Handle(query, CancellationToken.None);

        actual = result.Blogs;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ListSort_WithIncorrectSortProperty_ShouldSuccess()
    {
        var size = 10;

        var blogs = BlogFactory.CreateBlogs(size).Select(blog => new BlogListItemVm()
        {
            UserId = Guid.Empty,
            Id = blog.Id,
            Title = blog.Title,
            CreationDate = blog.CreationDate
        });

        var expected = blogs.ToList();

        var query = new BlogListSortQuery()
        {
            Blogs = new() { Blogs = blogs.ToList() },
            Property = "details",
            Descending = false
        };

        var handler = new BlogListSortQueryHandler();

        var result = await handler.Handle(query, CancellationToken.None);

        var actual = result.Blogs;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}