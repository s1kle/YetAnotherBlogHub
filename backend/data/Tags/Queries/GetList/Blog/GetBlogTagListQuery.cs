using BlogHub.Data.Tags.Queries.Get;

namespace BlogHub.Data.Tags.Queries.GetList.Blog;

public sealed record GetBlogTagListQuery : IRequest<IReadOnlyList<TagVm>>
{
    public required Guid BlogId { get; init; }
}