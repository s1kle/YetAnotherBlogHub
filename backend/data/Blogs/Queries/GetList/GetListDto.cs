namespace BlogHub.Data.Queries.GetList;

public record GetListDto
{
    public required int Page { get; init; }
    public required int Size { get; init; }
}