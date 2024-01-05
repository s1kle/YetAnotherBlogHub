/*
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace BlogHub.Identity.Configuration;

public class IdentityServerConfiguration
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ("Your secured api scopes")
        };
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ("Your api resorce", new [] { JwtClaimTypes.Name })
            {
                Scopes = { "Your secured api scopes" }
            }
        };
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new ()
            {
                //Your client configuration
            },
        };
}
*/