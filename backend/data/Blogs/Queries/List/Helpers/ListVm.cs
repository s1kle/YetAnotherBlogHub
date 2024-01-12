namespace BlogHub.Data.Blogs.List.Helpers;

public sealed record ListVm
{
    public required IReadOnlyList<ItemVm> Blogs { get; init;}
}