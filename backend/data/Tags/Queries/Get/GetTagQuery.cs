namespace BlogHub.Data.Tags.Queries.Get;

public sealed record GetTagQuery : IRequest<TagVm>
{
    public required Guid Id { get; init; }
}