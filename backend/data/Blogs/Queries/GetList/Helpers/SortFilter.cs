namespace BlogHub.Data.Blogs.Queries.GetList;

public record SortFilter
{
    public required string SortProperty { get; init; }
    public bool SortDescending { get; init; }
}