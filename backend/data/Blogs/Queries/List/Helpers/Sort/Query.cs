namespace BlogHub.Data.Blogs.List.Helpers.Sort;

public sealed record Query : IRequest<ListVm>
{
    public required ListVm Blogs { get; init; }
    public required string Property { get; init; }
    public bool Descending { get; init; }
}