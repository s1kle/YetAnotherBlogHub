
namespace BlogHub.Data.Comments.Queries.GetList.Blog;

internal sealed class GetBlogCommentListQueryHandler : IRequestHandler<GetBlogCommentListQuery, IReadOnlyList<CommentVm>>
{
    private readonly ICommentRepository _repository;
    private readonly IMapper _mapper;

    public GetBlogCommentListQueryHandler(ICommentRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<IReadOnlyList<CommentVm>> Handle(GetBlogCommentListQuery request, CancellationToken cancellationToken)
    {
        var comments = await _repository.GetAllByBlogIdAsync(request.BlogId, cancellationToken);

        if (comments is null) return Array.Empty<CommentVm>();

        return comments.Select(_mapper.Map<CommentVm>).ToList();
    }
}