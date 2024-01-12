namespace BlogHub.Data.Comments.Queries.GetList.AddUser;

internal sealed class CommentAddUserQueryHandler : IRequestHandler<CommentAddUserQueryQuery, IReadOnlyList<CommentVm>>
{
    private readonly IUserRepository _repository;

    public CommentAddUserQueryHandler(IUserRepository repository) =>
        _repository = repository;

    public async Task<IReadOnlyList<CommentVm>> Handle(CommentAddUserQueryQuery request, CancellationToken cancellationToken)
    {
        var comments = request.Comments.ToList();

        for (var i = 0; i < comments.Count; i++)
        {
            var comment = comments[i];

            var user = await _repository.GetAsync(comment.UserId, cancellationToken);

            comments[i] = comment with { Author = user?.Name };
        }

        return comments;
    }
}