namespace BlogHub.Data.Blogs.List.Helpers;

public sealed record BlogListVm
{
    public required IReadOnlyList<BlogListItemVm> Blogs { get; init; }
}