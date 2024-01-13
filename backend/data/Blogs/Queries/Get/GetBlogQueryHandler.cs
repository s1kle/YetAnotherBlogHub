namespace BlogHub.Data.Blogs.Get;

internal sealed class GetBlogQueryHandler : IRequestHandler<GetBlogQuery, BlogVm>
{
    private readonly IBlogRepository _repository;
    private readonly IMapper _mapper;

    public GetBlogQueryHandler(IBlogRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<BlogVm> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        var blog = await _repository.GetAsync(request.Id, cancellationToken);

        if (blog is null) throw new NotFoundException(nameof(blog));

        return _mapper.Map<BlogVm>(blog);
    }
}