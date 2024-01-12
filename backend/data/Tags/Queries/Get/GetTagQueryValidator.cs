namespace BlogHub.Data.Tags.Queries.Get;

internal sealed class GetTagQueryValidator : AbstractValidator<GetTagQuery>
{
    public GetTagQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty();
    }
}