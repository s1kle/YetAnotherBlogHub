namespace BlogHub.Data.Blogs.List.Helpers.Search;

public sealed record Query : IRequest<ListVm>
{
    public required ListVm Blogs { get; init; }
    public required string Filter { get; init; }
    public required string[] Properties { get; init; }
}