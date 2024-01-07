using MediatR;

namespace BlogHub.Data.Tags.Queries.Get;

public record GetTagQuery : IRequest<TagVm>
{
    public required Guid Id { get; init; }
}