using Craft.Domain.Contracts;
using Craft.MultiTenant.Builder;
using Craft.MultiTenant.Contracts;
using Craft.MultiTenant.Extensions;
using Craft.MultiTenant.Models;
using Craft.MultiTenant.Options;
using Craft.MultiTenant.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Craft.MultiTenant.Extensions;

public static class ServiceCollectionExtensions
{
    public static TenantBuilder<T> AddMultiTenant<T>(this IServiceCollection services, Action<TenantOptions> config)
        where T : class, ITenant, IEntity, new()
    {
        services.AddScoped<ITenantResolver<T>, TenantResolver<T>>();
        services.AddScoped(sp => (ITenantResolver)sp.GetRequiredService<ITenantResolver<T>>());

        services.AddScoped(sp => sp.GetRequiredService<ITenantContextAccessor<T>>().TenantContext);

        services.AddScoped(sp => sp.GetRequiredService<ITenantContextAccessor<T>>().TenantContext?.Tenant);
        services.AddScoped<ITenant>(sp => sp.GetService<T>());

        services.AddSingleton<ITenantContextAccessor<T>, TenantContextAccessor<T>>();
        services.AddSingleton(sp => (ITenantContextAccessor)sp.GetRequiredService<ITenantContextAccessor<T>>());

        services.Configure(config);

        return new TenantBuilder<T>(services);
    }

    public static TenantBuilder<T> AddMultiTenant<T>(this IServiceCollection services)
        where T : class, ITenant, IEntity, new()
    {
        return services.AddMultiTenant<T>(_ => { });
    }

    public static TenantBuilder<Tenant> AddMultiTenant(this IServiceCollection services)
    {
        return services.AddMultiTenant<Tenant>(_ => { });
    }
}
