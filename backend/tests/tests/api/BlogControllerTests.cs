/*
    temp version
*/

using System.Reflection;
using BlogHub.Api.Controllers;
using BlogHub.Api.Data;
using BlogHub.Api.Services;
using BlogHub.Data.Interfaces;
using BlogHub.Data.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlogHub.Tests.Api;

public class BlogControllerTests
{
    private readonly BlogController _blogController;

    public BlogControllerTests()
    {
        var dataAssembly = Assembly.GetAssembly(typeof(ValidationBehavior<,>));
        var services = new ServiceCollection();
        services.AddMediatR(config => config.RegisterServicesFromAssembly(dataAssembly));
        services.AddAutoMapper(dataAssembly);
        services.AddValidatorsFromAssembly(dataAssembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddDbContext<BlogDbContext>(options =>
            options.UseInMemoryDatabase("BlogHub.Tests"));
        services.AddScoped<IBlogDbContext, BlogDbContext>();
        services.AddScoped<IDistributedCache, MemoryDistributedCache>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        var provider = services.BuildServiceProvider();

        provider.GetService<BlogDbContext>().Database.EnsureCreated();

        var mediatr = provider.GetService<IMediator>();
        var mapper = provider.GetService<IMapper>();
        
        _blogController = new (mapper, mediatr);
    }

    [Fact]
    public async Task Test()
    {
        var result = await _blogController.GetAll(new () { Page = 0, Size = 1});

        return;
    }
}