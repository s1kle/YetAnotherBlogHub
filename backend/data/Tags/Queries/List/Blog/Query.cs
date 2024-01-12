namespace BlogHub.Data.Tags.List.Blog;

public sealed record Query : IRequest<IReadOnlyList<TagVm>>
{
    public required Guid BlogId { get; init; }
}