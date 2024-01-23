
namespace BlogHub.Data.Comments.List.Article;

internal sealed class GetArticleCommentsQueryHandler : IRequestHandler<GetArticleCommentsQuery, IReadOnlyList<CommentVm>>
{
    private readonly ICommentRepository _repository;
    private readonly IMapper _mapper;

    public GetArticleCommentsQueryHandler(ICommentRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<IReadOnlyList<CommentVm>> Handle(GetArticleCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _repository.GetAllByArticleIdAsync(request.ArticleId, cancellationToken);

        if (comments is null) return Array.Empty<CommentVm>();

        return comments.Select(_mapper.Map<CommentVm>).ToList();
    }
}