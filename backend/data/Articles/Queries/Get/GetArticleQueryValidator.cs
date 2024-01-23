namespace BlogHub.Data.Articles.Get;

internal sealed class GetArticleQueryValidator : AbstractValidator<GetArticleQuery>
{
    public GetArticleQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEqual(Guid.Empty);
    }
}