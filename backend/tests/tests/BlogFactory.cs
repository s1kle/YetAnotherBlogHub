using BlogHub.Domain;

namespace BlogHub.Tests;

public class BlogFactory
{
    public static Blog CreateBlog(string title = "title", Guid? userId = null) => new Blog()
    {
        Id = Guid.NewGuid(),
        UserId = userId ?? Guid.NewGuid(),
        Title = title,
        CreationDate = DateTime.UtcNow
    };

    public static List<Blog> CreateBlogs(int size, Guid? userId = null) => Enumerable
        .Range(0, size)
        .Select(index => CreateBlog($"Title {index}", userId))
        .ToList();
}