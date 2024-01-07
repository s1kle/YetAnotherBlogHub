using FluentValidation;

namespace BlogHub.Data.Tags.Queries.GetList;

public class GetBlogTagListQueryValidator : AbstractValidator<GetBlogTagListQuery>
{
    public GetBlogTagListQueryValidator()
    {
        RuleFor(query => query.BlogId)
            .NotEmpty();
    }
}