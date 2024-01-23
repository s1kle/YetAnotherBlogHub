namespace BlogHub.Data.Tags.List.Article;

internal sealed class GetArticleTagsQueryValidator : AbstractValidator<GetArticleTagsQuery>
{
    public GetArticleTagsQueryValidator()
    {
        RuleFor(query => query.ArticleId)
            .NotEmpty();
    }
}