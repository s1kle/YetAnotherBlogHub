using MediatR;

namespace BlogHub.Data.Queries.GetList;

public record GetBlogListQuery : IRequest<BlogListVm>
{
    public required Guid? UserId { get; init; }
    public required int Page { get; init; }
    public required int Size { get; init; }
    public required string SearchQuery { get; init; }
    public required string[] SearchProperties { get; init; }
    public required string SortProperty { get; init; }
    public required bool SortDirection { get; init; }
}