namespace BlogHub.Data.Articles.List.User;

internal sealed class GetUserArticlesQueryHandler : IRequestHandler<GetUserArticlesQuery, ArticleListVm>
{
    private readonly IArticleRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;

    public GetUserArticlesQueryHandler(IArticleRepository repository,
        IUserRepository userRepository,
        ITagRepository tagRepository,
        IMapper mapper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<ArticleListVm> Handle(GetUserArticlesQuery request, CancellationToken cancellationToken)
    {
        var temp = await _repository
            .GetAllByUserIdAsync(request.UserId, request.Page, request.Size, cancellationToken);

        if (temp is null) return new() { Articles = Array.Empty<ArticleListItemVm>() };

        var articles = new List<ArticleListItemVm>();

        foreach (var article in temp)
        {
            var user = await _userRepository.GetAsync(article.UserId, cancellationToken);
            var tags = await _tagRepository.GetAllByArticleIdAsync(article.Id, cancellationToken);

            var vm = _mapper.Map<ArticleListItemVm>(article);

            articles.Add(vm with
            {
                Author = user?.Name ?? "null",
                Tags = tags?
                    .Select(_mapper.Map<TagVm>)
                    .ToList(),
            });
        }

        return new() { Articles = articles };
    }
}