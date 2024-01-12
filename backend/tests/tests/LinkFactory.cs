namespace BlogHub.Tests;

public class LinkFactory
{
    public static BlogTagLink CreateBlogTagLink(Blog blog, Tag tag) => new BlogTagLink()
    {
        Id = Guid.NewGuid(),
        TagId = tag.Id,
        BlogId = blog.Id
    };
}