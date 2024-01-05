using MediatR;

namespace BlogHub.Data.Tags.Queries.GetList.ByBlogId;

public record GetTagListByBlogIdQuery : IRequest<TagListVm>
{
    public Guid BlogId { get; init; }
}