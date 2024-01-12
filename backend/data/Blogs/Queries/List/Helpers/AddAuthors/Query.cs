namespace BlogHub.Data.Blogs.List.Helpers.AddAuthors;

public sealed record Query : IRequest<ListVm>
{
    public required ListVm Blogs { get; init; }
}