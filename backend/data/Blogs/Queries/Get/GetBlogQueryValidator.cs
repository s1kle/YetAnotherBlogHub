using FluentValidation;

namespace BlogHub.Data.Queries.Get;

public class GetBlogQueryValidator : AbstractValidator<GetBlogQuery>
{
    public GetBlogQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEqual(Guid.Empty);
    }
}