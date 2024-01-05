using MediatR;

namespace BlogHub.Data.BlogTags.Commands.Create;

public record CreateBlogTagCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid BlogId { get; init; }
    public required Guid TagId { get; init; }
}