using Craft.MultiTenant.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Craft.MultiTenant.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMultiTenant(this IApplicationBuilder builder)
        => builder.UseMiddleware<TenantMiddleware>();
}
