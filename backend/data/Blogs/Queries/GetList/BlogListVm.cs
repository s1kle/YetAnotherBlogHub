namespace BlogHub.Data.Blogs.Queries.GetList;

public record BlogListVm
{
    public required IReadOnlyList<BlogVmForList> Blogs { get; init;}
}