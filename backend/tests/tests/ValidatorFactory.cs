using System.Linq.Expressions;
using System.Reflection;

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

    // public static Expression<Func<T, object>>[] GetExpressions<T>(Expression<Func<T, object>>[] expressions) =>
    //     ex

    public static ValidationContext<T> GetValidationContext<T>(T context) =>
        new ValidationContext<T>(context);

    public static T CreateValidRequest<T>() =>
        CreateFixture().Create<T>();

    public static T[] CreateInvalidRequest<T>(params Expression<Func<T, object>>[] expressions)
    {
        var fixture = CreateFixture();
        return expressions
            .Select(e => 
            {
                if (e.Body is MemberExpression m)
                {
                    var propertyType = m.Member.DeclaringType?.GetProperty(m.Member.Name)?.PropertyType;
                    var invalidValue = GetInvalidValue(propertyType);

                    return fixture.Build<T>().With(e, invalidValue).Create();  
                }
                else if (e.Body is UnaryExpression u && u.Operand is MemberExpression mOperand)
                {
                    var propertyType = mOperand.Member.DeclaringType?.GetProperty(mOperand.Member.Name)?.PropertyType;
                    var invalidValue = GetInvalidValue(propertyType);

                    return fixture.Build<T>().With(e, invalidValue).Create();
                }
                return fixture.Build<T>().With(e, Guid.Empty).Create();
            })
            .ToArray();
    }

    public static T[] CreateInvalidRequest<T>(params (Expression<Func<T, object>>, object)[] expressions)
    {
        var fixture = CreateFixture();
        return expressions
            .Select(expression => fixture.Build<T>().With(expression.Item1, expression.Item2).Create())
            .ToArray();
    }

    private static object GetInvalidValue(Type? type) => type switch
    {
        null => "No type provided",
        _ when type.Equals(typeof(Guid)) => Guid.Empty,
        _ when type.Equals(typeof(string)) => new string('*', 101),
        _ when type.Equals(typeof(int)) => -1,
        _ => "Not supported type"
    };

    private static Fixture CreateFixture()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(g => g.FromFactory(() => Guid.NewGuid()));
        fixture.Customize<string>(s => s.FromFactory(() => Guid.NewGuid().ToString()));
        fixture.Customize<int>(n => n.FromFactory(() => 2));

        return fixture;
    }
}
