using System.Reflection;
using System.Security.Claims;
using BlogHub.Api.Controllers;
using BlogHub.Api.Services;
using BlogHub.Data.Interfaces;
using BlogHub.Data.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace BlogHub.Tests.Fixtures;

public class BlogControllerFixture
{
    public Guid UserId { get; }
    public Guid WrongUserId { get; }
    public BlogController BlogController { get; }
    public BlogDbContext BlogDbContext;
    private bool _userState;

    public BlogControllerFixture(Guid userId, Guid wrongUserId, string dbContextName)
    {
        _userState = false;
        UserId = userId;
        WrongUserId = wrongUserId;
        var dataAssembly = Assembly.GetAssembly(typeof(ValidationBehavior<,>));
        var services = new ServiceCollection();

        #region setup
        services.AddMediatR(config => config.RegisterServicesFromAssembly(dataAssembly!));
        services.AddAutoMapper(dataAssembly);
        services.AddValidatorsFromAssembly(dataAssembly);
        services.AddDbContext<BlogDbContext>(options => options
            .UseInMemoryDatabase(dbContextName));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddSingleton<IDistributedCache, MemoryDistributedCache>();
        services.AddSingleton<IBlogRepository, BlogRepository>();
        services.AddSingleton<IBlogDbContext, BlogDbContext>();
        #endregion

        var serviceProvider = services.BuildServiceProvider();

        var mediatr = serviceProvider.GetService<IMediator>();
        var mapper = serviceProvider.GetService<IMapper>();
        BlogDbContext = serviceProvider.GetService<BlogDbContext>()!;

        BlogController = new BlogController(mapper!, mediatr!);

        var httpContext = A.Fake<HttpContext>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new (ClaimTypes.NameIdentifier, userId.ToString())
        }));
        A.CallTo(() => httpContext.User).Returns(user);

        BlogController.ControllerContext = new ()
        {
            HttpContext = httpContext
        };
    }
    public void ChangeUser()
    {
        var id = _userState ? UserId : WrongUserId;
        _userState = !_userState;

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new (ClaimTypes.NameIdentifier, id.ToString())
        }));

        var httpContext = A.Fake<HttpContext>();
        A.CallTo(() => httpContext.User).Returns(user);

        BlogController.ControllerContext = new ()
        {
            HttpContext = httpContext
        };
    }
}