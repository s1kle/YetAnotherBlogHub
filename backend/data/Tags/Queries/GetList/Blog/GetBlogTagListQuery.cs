using MediatR;

namespace BlogHub.Data.Tags.Queries.GetList;

public record GetBlogTagListQuery : IRequest<TagListVm>
{
    public required Guid BlogId { get; init; }
}