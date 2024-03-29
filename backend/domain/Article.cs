﻿namespace BlogHub.Domain;

public sealed record Article
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Title { get; init; }
    public required DateTime CreationDate { get; init; }
    public string? Details { get; init; }
    public DateTime? EditDate { get; init; }
}
