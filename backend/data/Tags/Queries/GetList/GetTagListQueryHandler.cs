using BlogHub.Data.Interfaces;
using MediatR;

namespace BlogHub.Data.Tags.Queries.GetList;

public class GetTagListQueryHandler : IRequestHandler<GetTagListQuery, TagListVm>
{
    private readonly ITagRepository _repository;

    public GetTagListQueryHandler(ITagRepository repository)
    {
        _repository = repository;
    }

    public async Task<TagListVm> Handle(GetTagListQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repository
            .GetAllAsync(request.BlogId, cancellationToken)
            ?? new ();

        var mappedTags = tags
            .Select(tag => tag.Name)
            .ToList();

        return new TagListVm { Tags = mappedTags };
    }
}