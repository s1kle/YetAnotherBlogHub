namespace BlogHub.Data.Blogs.Get;

internal sealed class Handler : IRequestHandler<Query, BlogVm>
{
    private readonly BlogsContext.Repository _repository;
    private readonly IMapper _mapper;

    public Handler(BlogsContext.Repository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<BlogVm> Handle(Query request, CancellationToken cancellationToken)
    {
        var blog = await _repository.GetAsync(request.Id, cancellationToken);

        if (blog is null) throw new NotFoundException(nameof(blog));

        return _mapper.Map<BlogVm>(blog);
    }
}