namespace BlogHub.Tests;

public class LinkFactory
{
    public static ArticleTagLink CreateArticleTagLink(Article Article, Tag tag) => new ArticleTagLink()
    {
        Id = Guid.NewGuid(),
        TagId = tag.Id,
        ArticleId = Article.Id
    };
}