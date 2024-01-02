using FluentValidation;

namespace BlogHub.Data.Tags.Queries.Get;

public class GetTagQueryValidator : AbstractValidator<GetTagQuery>
{
    public GetTagQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty();

        RuleFor(query => query.BlogId)
            .NotEmpty();
    }
}