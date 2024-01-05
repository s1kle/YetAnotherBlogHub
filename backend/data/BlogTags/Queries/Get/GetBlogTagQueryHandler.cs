using AutoMapper;
using BlogHub.Data.Exceptions;
using BlogHub.Data.Interfaces;
using BlogHub.Domain;
using MediatR;

namespace BlogHub.Data.BlogTags.Queries.Get;

public class GetBlogTagQueryHandler : IRequestHandler<GetBlogTagQuery, BlogTag>
{
    private readonly IBlogTagRepository _repository;

    public GetBlogTagQueryHandler(IBlogTagRepository repository) =>
        _repository = repository;

    public async Task<BlogTag> Handle(GetBlogTagQuery request, CancellationToken cancellationToken)
    {
        var blogTag = await _repository.GetAsync(request.Id, cancellationToken);

        if (blogTag is null) throw new NotFoundException(nameof(blogTag));

        return blogTag;
    }
}