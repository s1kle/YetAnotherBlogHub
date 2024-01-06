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
    private const string BlogsString = "Blogs";
    private const string RedisString = "Redis";
    private const string ClientString = "Client";
    private const string RedisInstanceName = "Redis:InstanceName";
    private const string Authority = "JwtOptions:Authority";
    private const string Audience = "JwtOptions:Audience";
 
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {   
        services.AddSerilog(config => config
            .ReadFrom
            .Configuration(configuration));
        services.AddDataDependencies();
        services.AddTransient<ExceptionHandlingMiddleware>();
        services.AddDbContext<IBlogDbContext, BlogDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(BlogsString)));
        services.AddDbContext<ITagDbContext, TagDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(BlogsString)));
        services.AddDbContext<IBlogTagDbContext, BlogTagDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(BlogsString)));
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IBlogTagRepository, BlogTagRepository>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString(RedisString);
            options.InstanceName = configuration[RedisInstanceName];
        });
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration[Authority];
                options.Audience = configuration[Audience];
            });
        services.AddScoped<IDistributedCache>(provider => 
            new LoggingDistributedCache(
                new RedisCache(
                    provider.GetRequiredService<IOptions<RedisCacheOptions>>())));
        services.AddCors(options =>
            options.AddPolicy(ClientString, policy =>
            {
                policy.WithOrigins(configuration.GetConnectionString(ClientString) 
                    ?? throw new ArgumentNullException("No connection string for client provided"));
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            }));
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}