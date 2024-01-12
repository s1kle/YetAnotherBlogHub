namespace BlogHub.Data.Blogs.Queries.Get;

public sealed record BlogAddUserQuery : IRequest<BlogVm>
{
    public required BlogVm Blog { get; init; }
}