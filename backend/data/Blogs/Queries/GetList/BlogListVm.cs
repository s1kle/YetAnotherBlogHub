namespace BlogHub.Data.Queries.GetList;

public record BlogListVm
{
    public required IReadOnlyList<BlogVmForList> Blogs { get; init;}
}