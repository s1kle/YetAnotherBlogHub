namespace BlogHub.Data.Comments.List.Helpers.AddAuthor;

public sealed record CommentListAddAuthorsQuery : IRequest<IReadOnlyList<CommentVm>>
{
    public required IReadOnlyList<CommentVm> Comments { get; init; }
}