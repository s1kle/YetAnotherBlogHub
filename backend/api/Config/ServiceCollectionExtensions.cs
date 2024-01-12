using BlogHub.Api.Middlewares;
using BlogHub.Api.Services;
using BlogHub.Data;
using BlogHub.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
    private const string RabbitMQHost = "RabbitMQ:Host";
    private const string RabbitMQUser = "RabbitMQ:User";
    private const string RabbitMQPassword = "RabbitMQ:Password";
    private const string RabbitMQExchange = "RabbitMQ:Exchange";
 
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {   
        services.AddSerilog(config => config
            .ReadFrom
            .Configuration(configuration));
        services
            .AddDataDependencies()
        
            .AddHostedService(provider => new EventConsumerService(
                configuration[RabbitMQHost]!, 
                configuration[RabbitMQUser]!, 
                configuration[RabbitMQPassword]!,
                configuration[RabbitMQExchange]!,
                provider))

            .AddTransient<ExceptionHandlingMiddleware>()

            .AddDbContext<IBlogDbContext, BlogDbContext>(options => options
                .UseNpgsql(configuration.GetConnectionString(BlogsString)))
            .AddDbContext<ITagDbContext, TagDbContext>(options => options
                .UseNpgsql(configuration.GetConnectionString(BlogsString)))
            .AddDbContext<IBlogTagDbContext, BlogTagDbContext>(options => options
                .UseNpgsql(configuration.GetConnectionString(BlogsString)))
            .AddDbContext<IUserDbContext, UserDbContext>(options => options
                .UseNpgsql(configuration.GetConnectionString(BlogsString)))

            .AddScoped<IBlogRepository, BlogRepository>()
            .AddScoped<ITagRepository, TagRepository>()
            .AddScoped<IBlogTagRepository, BlogTagRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString(RedisString);
                options.InstanceName = configuration[RedisInstanceName];
            })
            .AddScoped<IDistributedCache>(provider => new LoggingDistributedCache(
                new RedisCache(
                    provider.GetRequiredService<IOptions<RedisCacheOptions>>())))

            .AddCors(options => options
                .AddPolicy(ClientString, policy =>
                {
                    policy.WithOrigins(configuration.GetConnectionString(ClientString) 
                        ?? throw new ArgumentNullException("No connection string for client provided"));
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                }))

            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new ()
                    {
                        AuthorizationCode = new ()
                        {
                            AuthorizationUrl = new Uri("https://localhost:7010/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:7010/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                {configuration[Audience]!, "Api"}
                            }
                        }
                    }
                });

                var requirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        new[] { configuration[Audience] }
                    }
                };

                options.AddSecurityRequirement(requirement);
            });

        services
            .AddControllers();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration[Authority];
                options.Audience = configuration[Audience];
            });
            
        return services;
    }
}