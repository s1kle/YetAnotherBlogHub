namespace BlogHub.Tests;

public class CommentFactory
{
    public static Comment CreateComment(Blog blog, Guid userId, string content = "content") => new Comment()
    {
        Id = Guid.NewGuid(),
        BlogId = blog.Id,
        UserId = userId,
        Content = content,
        CreationDate = DateTime.UtcNow
    };

    public static List<Comment> CreateComments(int size, Blog blog, Guid userId, string content = "content") => Enumerable
        .Range(0, size)
        .Select(index => CreateComment(blog, userId, $"{content} {index}"))
        .ToList();
}