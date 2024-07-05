using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<Client> Clients =>
        new List<Client> {
            new Client()
            {
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientId = "client",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "secretApi" },
            },
            new Client()
            {
                ClientId = "client2",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = {"secretApi"}
            },
            new Client()
            {
                ClientId = "postman",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                RedirectUris = { "https://oauth.pstmn.io/v1/callback" },
                AllowedScopes =
                {
                    "secretApi",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true,
                RequireConsent = false
            }
        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> {
                new ApiScope(){Name = "secretApi"},
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource> { };
    }
}