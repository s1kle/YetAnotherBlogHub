using BlogHub.Domain;

namespace BlogHub.Data.BlogTags.Queries.GetList;

public record BlogTagListVm
{
    public required IReadOnlyList<BlogTag> BlogTags { get; init;}
}