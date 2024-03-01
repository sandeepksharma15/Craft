using Craft.MultiTenant;
using Craft.MultiTenant.Contracts;
using Craft.Security.Models;
using Hangfire.Client;
using Hangfire.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Craft.Security.Extensions;

namespace Craft.Jobs.Jobs;

public class JobFilter(IServiceProvider services) : IClientFilter
{
    private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

    private readonly IServiceProvider _services = services;

    public void OnCreated(CreatedContext filterContext) =>
        Logger.InfoFormat(
            "Job created with parameters {0}",
            filterContext.Parameters.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + ";" + s2));

    public void OnCreating(CreatingContext filterContext)
    {
        ArgumentNullException.ThrowIfNull(filterContext, nameof(filterContext));

        Logger.InfoFormat("Set TenantId and UserId parameters to job {0}.{1}...", filterContext.Job.Method.ReflectedType?.FullName, filterContext.Job.Method.Name);

        using var scope = _services.CreateScope();

        var httpContext = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext;
        _ = httpContext ?? throw new InvalidOperationException("Can't create a TenantJob without HttpContext.");

        var tenant = scope.ServiceProvider.GetRequiredService<ITenant>();
        filterContext.SetJobParameter(TenantConstants.TenantKey, tenant);

        string userId = httpContext.User.GetUserId();
        filterContext.SetJobParameter(CraftUser.UserId, userId);
    }
}
