namespace BlogHub.Data.Blogs.Get;

internal sealed class Validator : AbstractValidator<Query>
{
    public Validator()
    {
        RuleFor(query => query.Id)
            .NotEqual(Guid.Empty);
    }
}