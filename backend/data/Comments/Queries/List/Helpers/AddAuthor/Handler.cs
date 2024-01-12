namespace BlogHub.Data.Comments.List.Helpers.AddAuthor;

internal sealed class Handler : IRequestHandler<Query, IReadOnlyList<CommentVm>>
{
    private readonly UsersContext.Repository _repository;

    public Handler(UsersContext.Repository repository) =>
        _repository = repository;

    public async Task<IReadOnlyList<CommentVm>> Handle(Query request, CancellationToken cancellationToken)
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