namespace BlogHub.Tests;

public class ArticleFactory
{
    public static Article CreateArticle(string title = "title", Guid? userId = null) => new Article()
    {
        Id = Guid.NewGuid(),
        UserId = userId ?? Guid.NewGuid(),
        Title = title,
        CreationDate = DateTime.UtcNow
    };

    public static List<Article> CreateArticles(int size, Guid? userId = null, string title = "title") => Enumerable
        .Range(0, size)
        .Select(index => CreateArticle($"{title} {index}", userId))
        .ToList();
}