using BlogHub.Data.Blogs.List.Helpers;
using BlogHub.Data.Blogs.List.Helpers.Search;

namespace BlogHub.Tests.Requests.Blogs;

public class ListSearchTests
{
    [Fact]
    public async Task ListSearch_WithCorrectData_ShouldSuccess()
    {
        var size = 10;

        var blogs = BlogFactory.CreateBlogs(size).Select(blog => new BlogListItemVm()
        {
            UserId = Guid.Empty,
            Id = blog.Id,
            Title = blog.Title,
            CreationDate = blog.CreationDate
        });

        var expected = blogs.Where(blog => blog.Title.Contains("title", StringComparison.OrdinalIgnoreCase)).ToList();

        var query = new BlogListSearchQuery()
        {
            Blogs = new() { Blogs = blogs.ToList() },
            Query = "title",
            Properties = new[] { "title" }
        };

        var handler = new BlogListSearchQueryHandler();

        var result = await handler.Handle(query, CancellationToken.None);

        var actual = result.Blogs;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ListSearch_WithIncorrectSearchQuery_ShouldSuccess()
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

        var query = new BlogListSearchQuery()
        {
            Blogs = new() { Blogs = blogs.ToList() },
            Query = "    ",
            Properties = new[] { "title" }
        };

        var handler = new BlogListSearchQueryHandler();

        var result = await handler.Handle(query, CancellationToken.None);

        var actual = result.Blogs;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ListSearch_WithIncorrectSearchProperties_ShouldSuccess()
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

        var query = new BlogListSearchQuery()
        {
            Blogs = new() { Blogs = blogs.ToList() },
            Query = "title",
            Properties = Array.Empty<string>()
        };

        var handler = new BlogListSearchQueryHandler();

        var result = await handler.Handle(query, CancellationToken.None);

        var actual = result.Blogs;

        actual.Should().HaveCount(10);
        actual.Should().BeEquivalentTo(expected);
    }
}