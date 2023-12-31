using BlogHub.Identity.Data;
using BlogHub.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Identity.Configuration;

public static class ServiceCollectionExtensions
{
    private const string IdentityConnectionString = "Identity";

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthorizationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(IdentityConnectionString)));

        services.AddIdentity<ApplicationUser, IdentityRole>(config =>
        {
            config.Password.RequiredLength = 5;
            config.Password.RequireDigit = false;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireUppercase = false;

        })
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(config =>
        {
            config.Cookie.Name = "BlogHub.Identity.Cookie";
            config.LoginPath = "/Authorization/Login";
            config.LogoutPath = "/Authorization/Logout";
        });

        services.AddIdentityServer()
            .AddAspNetIdentity<ApplicationUser>()
            .AddInMemoryApiScopes(IdentityServerConfiguration.ApiScopes)
            .AddInMemoryApiResources(IdentityServerConfiguration.ApiResources)
            .AddInMemoryIdentityResources(IdentityServerConfiguration.IdentityResources)
            .AddInMemoryClients(IdentityServerConfiguration.Clients)
            .AddDeveloperSigningCredential();

        return services;
    }
}