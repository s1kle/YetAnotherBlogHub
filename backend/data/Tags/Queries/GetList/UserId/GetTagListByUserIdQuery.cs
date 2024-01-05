using MediatR;

namespace BlogHub.Data.Tags.Queries.GetList.ByUserId;

public record GetTagListByUserIdQuery : IRequest<TagListVm>
{
    public Guid? UserId { get; init; }
}