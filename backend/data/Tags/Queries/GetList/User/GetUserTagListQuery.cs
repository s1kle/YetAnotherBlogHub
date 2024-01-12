using BlogHub.Data.Tags.Queries.Get;

namespace BlogHub.Data.Tags.Queries.GetList;

public sealed record GetUserTagListQuery : IRequest<IReadOnlyList<TagVm>>
{
    public required Guid UserId { get; init; }
}