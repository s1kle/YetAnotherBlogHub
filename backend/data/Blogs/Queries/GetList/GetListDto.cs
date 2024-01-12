namespace BlogHub.Data.Blogs.Queries.GetList;

public sealed record GetListDto
{
    public required int Page { get; init; }
    public required int Size { get; init; }
}