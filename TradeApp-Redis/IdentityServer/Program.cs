using IdentityServer;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddIdentityServer()
    //.AddAspNetIdentity<IdentityUser>()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddDeveloperSigningCredential()
    .AddTestUsers(TestUsers.Users);

var app = builder.Build();

app.UseRouting();
app.UseIdentityServer();

app.MapGet("/", async (context) =>
{
    await context.Response.WriteAsync("Hello World!");
});

app.Run();