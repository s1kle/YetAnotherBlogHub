namespace BlogHub.Data.Queries.GetList;

public record BlogListVm
{
    public required IReadOnlyList<BlogVmForList> Blogs { get; init;}
    public required int Count { get; init; }
}