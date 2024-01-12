namespace BlogHub.Data.Comments.Queries.GetList.Blog;

internal sealed class GetBlogCommentListQueryValidator : AbstractValidator<GetBlogCommentListQuery>
{
    public GetBlogCommentListQueryValidator()
    {
        RuleFor(query => query.BlogId)
            .NotEmpty();
    }
}