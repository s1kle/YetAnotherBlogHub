namespace BlogHub.Data.Queries.GetBlogList;

public record BlogListVm
{
    public required IReadOnlyList<BlogVmForList> Blogs { get; init;}
}