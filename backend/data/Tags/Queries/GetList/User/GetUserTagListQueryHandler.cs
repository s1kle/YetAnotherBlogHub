using BlogHub.Data.Tags.Queries.Get;

namespace BlogHub.Data.Tags.Queries.GetList.User;

internal sealed class GetUserTagListQueryHandler : IRequestHandler<GetUserTagListQuery, IReadOnlyList<TagVm>>
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public GetUserTagListQueryHandler(ITagRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<IReadOnlyList<TagVm>> Handle(GetUserTagListQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repository.GetAllByUserIdAsync(request.UserId, cancellationToken);

        if (tags is null) return Array.Empty<TagVm>();

        return tags.Select(_mapper.Map<TagVm>).ToList();
    }
}