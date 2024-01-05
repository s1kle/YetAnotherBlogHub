using MediatR;

namespace BlogHub.Data.BlogTags.Commands.Delete;

public record DeleteBlogTagCommand : IRequest<Guid>
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
}