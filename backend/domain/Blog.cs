﻿namespace BlogHub.Domain;

public record Blog
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Title { get; init; }
    public required DateTime CreationDate { get; init; }
    public required string? Details { get; init; }
    public required DateTime? EditDate { get; init; }
}
