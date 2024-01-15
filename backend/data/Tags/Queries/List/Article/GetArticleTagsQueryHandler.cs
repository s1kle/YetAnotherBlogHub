namespace BlogHub.Data.Tags.List.Article;

internal sealed class GetArticleTagsQueryHandler : IRequestHandler<GetArticleTagsQuery, IReadOnlyList<TagVm>>
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public GetArticleTagsQueryHandler(ITagRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TagVm>> Handle(GetArticleTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repository.GetAllByArticleIdAsync(request.ArticleId, cancellationToken);

        if (tags is null) return Array.Empty<TagVm>();

        return tags.Select(_mapper.Map<TagVm>).ToList();
    }
}