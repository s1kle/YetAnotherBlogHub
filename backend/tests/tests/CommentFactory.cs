namespace BlogHub.Tests;

public class CommentFactory
{
    public static Comment CreateComment(Article Article, Guid userId, string content = "content") => new Comment()
    {
        Id = Guid.NewGuid(),
        ArticleId = Article.Id,
        UserId = userId,
        Content = content,
        CreationDate = DateTime.UtcNow
    };

    public static List<Comment> CreateComments(int size, Article Article, Guid userId, string content = "content") => Enumerable
        .Range(0, size)
        .Select(index => CreateComment(Article, userId, $"{content} {index}"))
        .ToList();
}