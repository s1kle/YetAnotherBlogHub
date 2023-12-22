using BlogHub.Api.Middlewares;
using BlogHub.Api.Services;
using BlogHub.Data;
using BlogHub.Data.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Serilog;

namespace BlogHub.Api.Configuration;

public static class ServiceCollectionExtensions
{
    private const string BlogsConnectionString = "Blogs";
    private const string RedisConnectionString = "Redis";
    private const string ClientConnectionString = "Client";
 
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {   
        services.AddSerilog(config => config.ReadFrom.Configuration(configuration));
        services.AddDataDependencies();
        services.AddTransient<ExceptionHandlingMiddleware>();
        services.AddDbContext<BlogDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(BlogsConnectionString)));
        services.AddScoped<IBlogDbContext, BlogDbContext>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString(RedisConnectionString);
            options.InstanceName = configuration["Redis:InstanceName"];
        });
        services.AddScoped<IDistributedCache>(provider =>
        {
            var redisCacheOptions = provider.GetRequiredService<IOptions<RedisCacheOptions>>();
            var cache = new RedisCache(redisCacheOptions);
            return new LoggingDistributedCache(cache);
        });
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddControllers();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = configuration["JwtOptions:Authority"];
            options.Audience = configuration["JwtOptions:Audience"];
        });
        services.AddCors(options =>
        {
            options.AddPolicy("Client", policy =>
            {
                policy.WithOrigins(configuration.GetConnectionString(ClientConnectionString)!);
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}