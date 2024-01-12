namespace BlogHub.Data.Blogs.List.All;

public sealed record Query : IRequest<ListVm>
{
    public required int Page { get; init; }
    public required int Size { get; init; }
}