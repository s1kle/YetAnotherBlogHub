using BlogHub.Data.Commands.Create;
using BlogHub.Data.Commands.Delete;
using BlogHub.Data.Commands.Update;
using BlogHub.Data.Queries.Get;
using BlogHub.Data.Queries.GetList;

namespace BlogHub.Tests.Data.Fixtures.Validation;

public class ValidationFixture
{
    public IEnumerable<IValidator> GetValidators(Type requestType)
    {
        var validators = new List<IValidator>();

        var validatorTypes = requestType.Assembly.DefinedTypes
            .Where(x => 
                x.BaseType is not null &&
                x.BaseType.Equals(typeof(AbstractValidator<>).MakeGenericType(requestType)));

        foreach(var type in validatorTypes)
            validators.Add(Activator.CreateInstance(type) as IValidator);

        return validators;
    }

    #region DeleteBlogCommand
    public DeleteBlogCommand GetValidDeleteBlogCommand()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(guid => guid.FromFactory(() => Guid.NewGuid()));
    
        return fixture.Create<DeleteBlogCommand>();
    }

    public DeleteBlogCommand[] GetInvalidDeleteBlogCommand()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(guid => guid.FromFactory(() => Guid.NewGuid()));
    
        return new[]
        {
            fixture.Build<DeleteBlogCommand>()
                .With(request => request.UserId, Guid.Empty)
                .Create(),
            fixture.Build<DeleteBlogCommand>()
                .With(request => request.Id, Guid.Empty)
                .Create(),
            fixture.Build<DeleteBlogCommand>()
                .With(request => request.Id, Guid.Empty)
                .With(request => request.UserId, Guid.Empty)
                .Create()
        };
    }
    #endregion
    #region CreateBlogCommand
    public CreateBlogCommand GetValidCreateBlogCommand()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(guid => guid.FromFactory(() => Guid.NewGuid()));
        fixture.Customize<string>(title => title.FromFactory(() => Guid.NewGuid().ToString()));
    
        return fixture.Create<CreateBlogCommand>();
    }

    public CreateBlogCommand[] GetInvalidCreateBlogCommand()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(guid => guid.FromFactory(() => Guid.NewGuid()));
        fixture.Customize<string>(title => title.FromFactory(() => Guid.NewGuid().ToString()));
    
        return new[]
        {
            fixture.Build<CreateBlogCommand>()
                .With(request => request.UserId, Guid.Empty)
                .Create(),
            fixture.Build<CreateBlogCommand>()
                .With(request => request.Title, new string('a', 101))
                .Create(),
            fixture.Build<CreateBlogCommand>()
                .With(request => request.Title, new string('a', 101))
                .With(request => request.UserId, Guid.Empty)
                .Create()
        };
    }
    #endregion
    #region UpdateBlogCommand
    public UpdateBlogCommand GetValidUpdateBlogCommand()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(guid => guid.FromFactory(() => Guid.NewGuid()));
        fixture.Customize<string>(title => title.FromFactory(() => Guid.NewGuid().ToString()));
    
        return fixture.Create<UpdateBlogCommand>();
    }

    public UpdateBlogCommand[] GetInvalidUpdateBlogCommand()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(guid => guid.FromFactory(() => Guid.NewGuid()));
        fixture.Customize<string>(title => title.FromFactory(() => Guid.NewGuid().ToString()));
    
        return new[]
        {
            fixture.Build<UpdateBlogCommand>()
                .With(request => request.UserId, Guid.Empty)
                .Create(),
            fixture.Build<UpdateBlogCommand>()
                .With(request => request.Id, Guid.Empty)
                .Create(),
            fixture.Build<UpdateBlogCommand>()
                .With(request => request.Title, new string('a', 101))
                .Create(),
            fixture.Build<UpdateBlogCommand>()
                .With(request => request.Title, new string('a', 101))
                .With(request => request.Id, Guid.Empty)
                .With(request => request.UserId, Guid.Empty)
                .Create()
        };
    }
    #endregion
    #region GetBlogQuery
    public GetBlogQuery GetValidGetBlogQuery()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(guid => guid.FromFactory(() => Guid.NewGuid()));
    
        return fixture.Create<GetBlogQuery>();
    }

    public GetBlogQuery[] GetInvalidGetBlogQuery()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(guid => guid.FromFactory(() => Guid.NewGuid()));
    
        return new[]
        {
            fixture.Build<GetBlogQuery>()
                .With(request => request.UserId, Guid.Empty)
                .Create(),
            fixture.Build<GetBlogQuery>()
                .With(request => request.Id, Guid.Empty)
                .Create(),
            fixture.Build<GetBlogQuery>()
                .With(request => request.Id, Guid.Empty)
                .With(request => request.UserId, Guid.Empty)
                .Create()
        };
    }
    #endregion
    #region GetBlogListQuery
    public GetBlogListQuery GetValidGetBlogListQuery()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(guid => guid.FromFactory(() => Guid.NewGuid()));
        fixture.Customize<int>(n => n.FromFactory(() => 2));
    
        return fixture.Create<GetBlogListQuery>();
    }

    public GetBlogListQuery[] GetInvalidGetBlogListQuery()
    {
        var fixture = new Fixture();

        fixture.Customize<Guid>(guid => guid.FromFactory(() => Guid.NewGuid()));
        fixture.Customize<int>(n => n.FromFactory(() => 2));
    
        return new[]
        {
            fixture.Build<GetBlogListQuery>()
                .With(request => request.UserId, Guid.Empty)
                .Create(),
            fixture.Build<GetBlogListQuery>()
                .With(request => request.Page, -1)
                .Create(),
            fixture.Build<GetBlogListQuery>()
                .With(request => request.Size, 0)
                .Create(),
            fixture.Build<GetBlogListQuery>()
                .With(request => request.Size, 0)
                .With(request => request.Page, -1)
                .With(request => request.UserId, Guid.Empty)
                .Create()
        };
    }
    #endregion
}