namespace BlogHub.Data.Comments.List.Article;

internal sealed class GetArticleCommentsQueryValidator : AbstractValidator<GetArticleCommentsQuery>
{
    public GetArticleCommentsQueryValidator()
    {
        RuleFor(query => query.ArticleId)
            .NotEmpty();
    }
}