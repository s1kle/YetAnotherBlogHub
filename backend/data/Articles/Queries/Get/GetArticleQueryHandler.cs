namespace BlogHub.Data.Articles.Get;

internal sealed class GetArticleQueryHandler : IRequestHandler<GetArticleQuery, ArticleVm>
{
    private readonly IArticleRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;

    public GetArticleQueryHandler(IArticleRepository repository,
        IUserRepository userRepository,
        ICommentRepository commentRepository,
        ITagRepository tagRepository,
        IMapper mapper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<ArticleVm> Handle(GetArticleQuery request, CancellationToken cancellationToken)
    {
        var article = await _repository.GetAsync(request.Id, cancellationToken);

        if (article is null) throw new NotFoundException(nameof(article));

        var user = await _userRepository.GetAsync(article.UserId, cancellationToken);
        var tags = await _tagRepository.GetAllByArticleIdAsync(article.Id, cancellationToken);
        var comments = await _commentRepository.GetAllByArticleIdAsync(article.Id, cancellationToken);

        var vm = _mapper.Map<ArticleVm>(article);

        return vm with
        {
            Author = user?.Name ?? "null",
            Tags = tags?
                .Select(_mapper.Map<TagVm>)
                .ToList(),
            Comments = comments?
                .Select(_mapper.Map<CommentVm>)
                .ToList()
        };
    }
}