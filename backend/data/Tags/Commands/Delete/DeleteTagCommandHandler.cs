namespace BlogHub.Data.Tags.Delete;

internal sealed class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Guid>
{
    private readonly ITagRepository _tagRepository;
    private readonly IBlogTagRepository _blogTagRepository;

    public DeleteTagCommandHandler(ITagRepository repository, IBlogTagRepository blogTagRepository) =>
        (_tagRepository, _blogTagRepository) = (repository, blogTagRepository);

    public async Task<Guid> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetAsync(request.Id, cancellationToken);

        if (tag is null || tag.UserId != request.UserId)
            throw new NotFoundException(nameof(tag));

        var blogTags = await _blogTagRepository.GetAllAsync(cancellationToken) ?? new();

        foreach (var blogTag in blogTags
            .Where(blogTag => blogTag.TagId.Equals(tag.Id)))
        {
            await _blogTagRepository.RemoveAsync(blogTag, cancellationToken);
        }

        var id = await _tagRepository.RemoveAsync(tag, cancellationToken);

        return id;
    }
}