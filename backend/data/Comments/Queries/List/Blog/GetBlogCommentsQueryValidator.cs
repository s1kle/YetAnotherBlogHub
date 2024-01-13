namespace BlogHub.Data.Comments.List.Blog;

internal sealed class GetBlogCommentsQueryValidator : AbstractValidator<GetBlogCommentsQuery>
{
    public GetBlogCommentsQueryValidator()
    {
        RuleFor(query => query.BlogId)
            .NotEmpty();
    }
}