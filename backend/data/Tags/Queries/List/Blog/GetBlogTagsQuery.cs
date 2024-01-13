namespace BlogHub.Data.Tags.List.Blog;

public sealed record GetBlogTagsQuery : IRequest<IReadOnlyList<TagVm>>
{
    public required Guid BlogId { get; init; }
}