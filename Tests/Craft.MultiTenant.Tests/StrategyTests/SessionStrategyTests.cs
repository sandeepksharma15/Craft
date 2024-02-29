using Craft.MultiTenant.Contracts;
using Craft.MultiTenant.Models;
using Craft.MultiTenant.Options;
using Craft.MultiTenant.Strategies;
using Craft.MultiTenant.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Craft.MultiTenant.Tests.StrategyTests;

public class SessionStrategyTests
{
    private static IWebHostBuilder GetTestHostBuilder(string identifier, string sessionKey)
    {
        return new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddDistributedMemoryCache();
                services.AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromSeconds(10);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                });

                services.AddMultiTenant<Tenant>()
                    .WithSessionStrategy(sessionKey)
                    .WithInMemoryStore();
                services.AddMvc();
            })
            .Configure(app =>
            {
                app.UseSession();
                app.UseMultiTenant();
                app.Run(async context =>
                {
                    context.Session.SetString(sessionKey, identifier);
                    if (context.GetTenantContext<Tenant>()?.Tenant != null)
                        await context.Response.WriteAsync(context.GetTenantContext<Tenant>().Tenant.Identifier);
                });

                var store = app.ApplicationServices.GetRequiredService<ITenantStore<Tenant>>();
                store.AddAsync(new Tenant { Id = 1, Identifier = identifier }).Wait();
            });
    }

    [Fact]
    public async Task ReturnNullIfNoSessionValue()
    {
        var hostBuilder = GetTestHostBuilder("test_tenant", "__TENANT__");

        using (var server = new TestServer(hostBuilder))
        {
            var client = server.CreateClient();
#pragma warning disable CA2234 // Pass system uri objects instead of strings
            var response = await client.GetStringAsync("/test_tenant");
#pragma warning restore CA2234 // Pass system uri objects instead of strings
            Assert.Equal("", response);
        }
    }
}
