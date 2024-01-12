
namespace BlogHub.Data.Comments.List.Blog;

internal sealed class Handler : IRequestHandler<Query, IReadOnlyList<CommentVm>>
{
    private readonly CommentsContext.Repository _repository;
    private readonly IMapper _mapper;

    public Handler(CommentsContext.Repository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<IReadOnlyList<CommentVm>> Handle(Query request, CancellationToken cancellationToken)
    {
        var comments = await _repository.GetAllByBlogIdAsync(request.BlogId, cancellationToken);

        if (comments is null) return Array.Empty<CommentVm>();

        return comments.Select(_mapper.Map<CommentVm>).ToList();
    }
}