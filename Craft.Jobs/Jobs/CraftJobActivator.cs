using Craft.MultiTenant;
using Craft.MultiTenant.Contexts;
using Craft.MultiTenant.Contracts;
using Craft.MultiTenant.Models;
using Craft.Security.CurrentUserService;
using Craft.Security.Models;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;

namespace Craft.Jobs.Jobs;

public class CraftJobActivator(IServiceScopeFactory scopeFactory) : JobActivator
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public override JobActivatorScope BeginScope(PerformContext context) =>
        new Scope(context, _scopeFactory.CreateScope());

    private class Scope : JobActivatorScope, IServiceProvider
    {
        private readonly PerformContext _context;
        private readonly IServiceScope _scope;

        public Scope(PerformContext context, IServiceScope scope)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            ArgumentNullException.ThrowIfNull(scope, nameof(scope));

            _context = context;
            _scope = scope;

            ReceiveParameters();
        }

        private void ReceiveParameters()
        {
            var tenant = _context.GetJobParameter<Tenant>(TenantConstants.TenantKey);

            if (tenant is not null)
                _scope.ServiceProvider.GetRequiredService<ITenantContextAccessor>()
                    .TenantContext = new TenantContext<Tenant>
                    {
                        Tenant = tenant
                    };

            KeyType userId = _context.GetJobParameter<KeyType>(CraftUser.UserId);

            if (userId != default)
                _scope.ServiceProvider.GetRequiredService<ICurrentUser>()
                    .SetCurrentUserId(userId);
        }

        object IServiceProvider.GetService(Type serviceType) =>
            serviceType == typeof(PerformContext)
                ? _context
                : _scope.ServiceProvider.GetService(serviceType);

        public override object Resolve(Type type) =>
                    ActivatorUtilities.GetServiceOrCreateInstance(this, type);
    }
}
