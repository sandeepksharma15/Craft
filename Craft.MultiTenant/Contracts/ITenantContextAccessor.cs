﻿using Craft.Domain.Contracts;

namespace Craft.MultiTenant.Contracts;

public interface ITenantContextAccessor
{
    ITenantContext TenantContext { get; set; }
}

public interface ITenantContextAccessor<T> where T : class, ITenant, IEntity, new()
{
    ITenantContext<T> TenantContext { get; set; }
}
