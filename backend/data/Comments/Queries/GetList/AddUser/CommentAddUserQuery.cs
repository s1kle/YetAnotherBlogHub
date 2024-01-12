namespace BlogHub.Data.Comments.Queries.GetList.AddUser;

public sealed record CommentAddUserQueryQuery : IRequest<IReadOnlyList<CommentVm>>
{
    public required IReadOnlyList<CommentVm> Comments { get; init; }
}