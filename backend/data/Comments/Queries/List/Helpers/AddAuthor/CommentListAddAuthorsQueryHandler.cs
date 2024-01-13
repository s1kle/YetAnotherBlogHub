namespace BlogHub.Data.Comments.List.Helpers.AddAuthor;

internal sealed class CommentListAddAuthorsQueryHandler : IRequestHandler<CommentListAddAuthorsQuery, IReadOnlyList<CommentVm>>
{
    private readonly IUserRepository _repository;

    public CommentListAddAuthorsQueryHandler(IUserRepository repository) =>
        _repository = repository;

    public async Task<IReadOnlyList<CommentVm>> Handle(CommentListAddAuthorsQuery request, CancellationToken cancellationToken)
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