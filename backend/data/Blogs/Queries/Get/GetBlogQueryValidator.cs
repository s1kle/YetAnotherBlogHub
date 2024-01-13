namespace BlogHub.Data.Blogs.Get;

internal sealed class GetBlogQueryValidator : AbstractValidator<GetBlogQuery>
{
    public GetBlogQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEqual(Guid.Empty);
    }
}