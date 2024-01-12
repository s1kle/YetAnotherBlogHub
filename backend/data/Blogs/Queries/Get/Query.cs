namespace BlogHub.Data.Blogs.Get;

public sealed record Query : IRequest<BlogVm>
{
    public required Guid Id { get; init; }
}