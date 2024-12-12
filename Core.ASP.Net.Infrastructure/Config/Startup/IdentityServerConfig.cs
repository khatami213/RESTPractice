using IdentityServer4.Models;

namespace Core.ASP.Net.Infrastructure.Config.Startup;

public static class IdentityServerConfig
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            //define project name for assign grant to users
            new ApiScope("customer_api_service", "Customer API")
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource
            {

            }
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            //Client for web APIs
            new Client
            {
                ClientId = "customer_api_swagger",
                ClientName = "Customer Web App",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                RequirePkce = true,
                RequireClientSecret = true,
                ClientSecrets =
                {
                    new Secret("6yhn(OL>".Sha256())
                },
                AllowedScopes={"customer_api_service"},
                AccessTokenLifetime=(int)TimeSpan.FromDays(1).TotalSeconds
            }
        };

}
