using FluentValidation;

namespace BlogHub.Data.BlogTags.Queries.Get;

public class GetBlogTagQueryValidator : AbstractValidator<GetBlogTagQuery>
{
    public GetBlogTagQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty();
    }
}