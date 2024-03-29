﻿using Craft.MultiTenant.Contracts;
using Craft.MultiTenant.Models;
using Craft.MultiTenant.Strategies;
using Craft.MultiTenant.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;

namespace Craft.MultiTenant.Tests.StrategyTests;

public class RouteStrategyTests
{
    private static IWebHostBuilder GetTestHostBuilder(string identifier, string routePattern)
    {
        return new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddMultiTenant<Tenant>().WithRouteStrategy().WithInMemoryStore();
                services.AddMvc();
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseMultiTenant();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.Map(routePattern, async context =>
                    {
                        if (context.GetTenantContext<Tenant>()?.Tenant != null)
                            await context.Response.WriteAsync(context.GetTenantContext<Tenant>().Tenant.Identifier);
                    });
                });

                var store = app.ApplicationServices.GetRequiredService<ITenantStore<Tenant>>();

                store.AddAsync(new Tenant { Id = 1, Identifier = identifier }).Wait();
            });
    }

    [Theory]
    [InlineData("/initech", "initech", "initech")]
    [InlineData("/", "initech", "")]
    public async Task ReturnExpectedIdentifier(string path, string identifier, string expected)
    {
        IWebHostBuilder hostBuilder = GetTestHostBuilder(identifier, "{__TENANT__=}");

        using (var server = new TestServer(hostBuilder))
        {
            var client = server.CreateClient();
#pragma warning disable CA2234 // Pass system uri objects instead of strings
            var response = await client.GetStringAsync(path);
#pragma warning restore CA2234 // Pass system uri objects instead of strings
            Assert.Equal(expected, response);
        }
    }

    [Fact]
    public async Task ReturnNullIfNoRouteParamMatch()
    {
        IWebHostBuilder hostBuilder = GetTestHostBuilder("test_tenant", "{controller}");

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
