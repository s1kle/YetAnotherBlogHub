namespace BlogHub.Data.Blogs.Queries.GetList.User;

internal sealed class GetUserBlogListQueryHandler : IRequestHandler<GetUserBlogListQuery, BlogListVm>
{
    private readonly IBlogRepository _repository;
    private readonly IMapper _mapper;

    public GetUserBlogListQueryHandler(IBlogRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<BlogListVm> Handle(GetUserBlogListQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _repository
            .GetAllByUserIdAsync(request.UserId,request.Page, request.Size, cancellationToken);

        if (blogs is null) return new () { Blogs = Array.Empty<BlogVmForList>() };

        return new () { Blogs = blogs.Select(_mapper.Map<BlogVmForList>).ToList() };
    }
}