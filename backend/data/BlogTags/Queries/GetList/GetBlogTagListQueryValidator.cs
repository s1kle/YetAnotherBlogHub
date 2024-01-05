using FluentValidation;

namespace BlogHub.Data.BlogTags.Queries.GetList;

public class GetBlogTagListQueryValidator : AbstractValidator<GetBlogTagListQuery>
{
    public GetBlogTagListQueryValidator()
    {
        RuleFor(query => query.BlogId)
            .NotEmpty();
    }
}