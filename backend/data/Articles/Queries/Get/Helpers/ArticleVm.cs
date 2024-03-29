namespace BlogHub.Data.Articles.Get.Helpers;

public sealed record ArticleVm
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required DateTime CreationDate { get; init; }
    public string? Details { get; init; }
    public DateTime? EditDate { get; init; }
    public string? Author { get; init; }
    public IReadOnlyList<TagVm>? Tags { get; init; }
    public IReadOnlyList<CommentVm>? Comments { get; init; }
}