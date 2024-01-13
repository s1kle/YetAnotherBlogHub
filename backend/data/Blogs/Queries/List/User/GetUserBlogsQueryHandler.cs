namespace BlogHub.Data.Blogs.List.User;

internal sealed class GetUserBlogsQueryHandler : IRequestHandler<GetUserBlogsQuery, BlogListVm>
{
    private readonly BlogRepository _repository;
    private readonly IMapper _mapper;

    public GetUserBlogsQueryHandler(BlogRepository repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);

    public async Task<BlogListVm> Handle(GetUserBlogsQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _repository
            .GetAllByUserIdAsync(request.UserId, request.Page, request.Size, cancellationToken);

        if (blogs is null) return new() { Blogs = Array.Empty<BlogListItemVm>() };

        return new() { Blogs = blogs.Select(_mapper.Map<BlogListItemVm>).ToList() };
    }
}