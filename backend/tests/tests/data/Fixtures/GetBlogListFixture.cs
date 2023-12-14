using BlogHub.Data.Queries.GetList;
using BlogHub.Domain;

namespace BlogHub.Tests.Data.Fixtures;

public class GetBlogListFixture
{
    public GetBlogListQuery Query { get; }
    public List<Blog> BlogList { get; }
    public List<BlogVmForList> BlogVmList { get; }
    public BlogListVm BlogListVm { get; }
    private Random _random;

    public GetBlogListFixture(Guid userId, int size)
    {
        _random = new Random();
        BlogList = new ();
        BlogVmList = new ();
        for (var i = 0; i < size; i++)
        {
            (var blog, var blogVm) = GenerateData(userId, $"Title{i}", $"Details{i}");
            BlogList.Add(blog);
            BlogVmList.Add(blogVm);
        }
        BlogListVm = new () { Blogs = BlogVmList };
        Query = new ()
        {
            UserId = userId,
            Page = 0,
            Size = size
        };
    }

    private (Blog, BlogVmForList) GenerateData(Guid userId, string title, string? details)
    {
        var id = Guid.NewGuid();
        var creationDate = DateTime.UtcNow.AddMinutes(_random.Next(-10, 1));
        DateTime? editDate = _random.Next(0, 2).Equals(1)
            ? DateTime.UtcNow.AddMinutes(_random.Next(0, 11))
            : null;
        var blog = new Blog()
        {
            Id = id,
            UserId = userId,
            Title = title,
            Details = details,
            CreationDate = creationDate,
            EditDate = editDate,
        };
        var blogVm = new BlogVmForList()
        {
            Id = id,
            Title = title,
            Details = details,
        };

        return (blog, blogVm);
    }
}