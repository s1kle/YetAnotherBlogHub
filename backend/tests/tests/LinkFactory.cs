namespace BlogHub.Tests;

public class LinkFactory
{
    public static ArticleTag CreateArticleTagLink(Article Article, Tag tag) => new ArticleTag()
    {
        Id = Guid.NewGuid(),
        TagId = tag.Id,
        ArticleId = Article.Id
    };
}