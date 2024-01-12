namespace BlogHub.Data.Tags.List.Blog;

internal sealed class Validator : AbstractValidator<Query>
{
    public Validator()
    {
        RuleFor(query => query.BlogId)
            .NotEmpty();
    }
}