using System.Linq.Expressions;

namespace BlogHub.Tests;

public class ValidatorFactory
{
    public static List<IValidator> GetValidators<T>()
    {
        var validators = new List<IValidator>();

        typeof(T).Assembly.DefinedTypes
            .Where(x =>
                x.BaseType is not null &&
                x.BaseType.Equals(typeof(AbstractValidator<>).MakeGenericType(typeof(T))))
            .ToList()
            .ForEach(type => validators.Add((Activator.CreateInstance(type) as IValidator)!));

        return validators;
    }

    public static ValidationContext<T> GetValidationContext<T>(T context) =>
        new ValidationContext<T>(context);

    public static T CreateValidRequest<T>() =>
        CreateFixture().Create<T>();

    public static T[] CreateInvalidRequest<T>(params (Expression<Func<T, object>> Expression, object InvalidValue)[] expressions)
    {
        var fixture = CreateFixture();
        return expressions
            .Select(expression => fixture.Build<T>().With(expression.Expression, expression.InvalidValue).Create())
            .ToArray();
    }

    private static Fixture CreateFixture()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(g => g.FromFactory(() => Guid.NewGuid()));
        fixture.Customize<string>(s => s.FromFactory(() => new string('*', 10)));
        fixture.Customize<int>(n => n.FromFactory(() => 2));

        return fixture;
    }
}
