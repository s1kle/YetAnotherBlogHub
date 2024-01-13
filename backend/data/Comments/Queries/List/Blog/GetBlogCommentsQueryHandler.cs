
namespace BlogHub.Data.Comments.List.Blog;

internal sealed class GetBlogCommentsQueryHandler : IRequestHandler<GetBlogCommentsQuery, IReadOnlyList<CommentVm>>
{
    private readonly ICommentRepository _repository;
    private readonly IMapper _mapper;

    public GetBlogCommentsQueryHandler(ICommentRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<IReadOnlyList<CommentVm>> Handle(GetBlogCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _repository.GetAllByBlogIdAsync(request.BlogId, cancellationToken);

        if (comments is null) return Array.Empty<CommentVm>();

        return comments.Select(_mapper.Map<CommentVm>).ToList();
    }
}