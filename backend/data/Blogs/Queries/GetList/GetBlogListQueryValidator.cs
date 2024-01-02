using FluentValidation;

namespace BlogHub.Data.Queries.GetList;

public class GetBlogListQueryValidator : AbstractValidator<GetBlogListQuery>
{
    public GetBlogListQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEqual(Guid.Empty);    

        RuleFor(query => query.Size)
            .GreaterThan(0);

        RuleFor(query => query.Page)
            .GreaterThan(-1);
    }
}