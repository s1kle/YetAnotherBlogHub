using MediatR;

namespace BlogHub.Data.Queries.Get;

public record GetBlogQuery : IRequest<BlogVm>
{
    public required Guid Id { get; init; }
}