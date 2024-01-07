using MediatR;

namespace BlogHub.Data.Tags.Queries.GetList;

public record GetUserTagListQuery : IRequest<TagListVm>
{
    public required Guid UserId { get; init; }
}