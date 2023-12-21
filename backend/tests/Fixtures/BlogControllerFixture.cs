using System.Reflection;
using System.Security.Claims;
using BlogHub.Api.Controllers;
using BlogHub.Api.Data;
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
    private BlogDbContext _blogDbContext;

    public BlogControllerFixture(Guid userId, Guid wrongUserId)
    {
        var dataAssembly = Assembly.GetAssembly(typeof(ValidationBehavior<,>));
        var services = new ServiceCollection();

        #region setup
        services.AddMediatR(config => config.RegisterServicesFromAssembly(dataAssembly!));
        services.AddAutoMapper(dataAssembly);
        services.AddValidatorsFromAssembly(dataAssembly);
        services.AddDbContext<BlogDbContext>(options => options.UseInMemoryDatabase("BlogHub.Tests"));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped<IBlogDbContext, BlogDbContext>();
        services.AddScoped<IDistributedCache, MemoryDistributedCache>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        #endregion

        var provider = services.BuildServiceProvider();

        _blogDbContext = provider.GetService<BlogDbContext>()!;
        var mediatr = provider.GetService<IMediator>();
        var mapper = provider.GetService<IMapper>();
        
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
    public void EnsureCreated()
    {
        _blogDbContext.Database.EnsureCreated();
    }
    public void EnsureDeleted()
    {
        _blogDbContext.Database.EnsureDeleted();
    }
    public void ChangeUser(Guid userId)
    {
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
}