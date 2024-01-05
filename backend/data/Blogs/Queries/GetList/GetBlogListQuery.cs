using MediatR;

namespace BlogHub.Data.Blogs.Queries.GetList;

public record GetBlogListQuery : IRequest<BlogListVm>
{
    public required Guid? UserId { get; init; }
    public required int Page { get; init; }
    public required int Size { get; init; }
    public SortFilter? SortFilter { get; init; }
    public SearchFilter? SearchFilter { get; init; }
    public TagsFilter? TagsFilter { get; init; }
}