namespace BlogHub.Data.Blogs.Queries.GetList;

public sealed record BlogListVm
{
    public required IReadOnlyList<BlogVmForList> Blogs { get; init;}
}