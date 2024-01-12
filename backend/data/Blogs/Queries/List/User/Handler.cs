namespace BlogHub.Data.Blogs.List.User;

internal sealed class Handler : IRequestHandler<Query, ListVm>
{
    private readonly BlogsContext.Repository _repository;
    private readonly IMapper _mapper;

    public Handler(BlogsContext.Repository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<ListVm> Handle(Query request, CancellationToken cancellationToken)
    {
        var blogs = await _repository
            .GetAllByUserIdAsync(request.UserId,request.Page, request.Size, cancellationToken);

        if (blogs is null) return new () { Blogs = Array.Empty<ItemVm>() };

        return new () { Blogs = blogs.Select(_mapper.Map<ItemVm>).ToList() };
    }
}