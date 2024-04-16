using Craft.Data.Extensions;
using Craft.Domain.Contracts;
using Craft.MultiTenant.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Craft.Data.DbContexts;

public abstract class CraftTenantDbContext<T> : CraftDbContext, ITenantStoreDbContext<T>
    where T : class, ITenant, new()
{
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>")]
    protected CraftTenantDbContext(DbContextOptions options) : base(options) { }

    public DbSet<T> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // QueryFilters need to be applied before base.OnModelCreating
        builder.AddGlobalQueryFilter<ISoftDelete>(s => !s.IsDeleted);

        base.OnModelCreating(builder);
    }

    public virtual int SaveChanges(KeyType userId)
    {
        OnBeforeSaveChanges(userId);

        return base.SaveChanges();
    }

    public virtual async Task<int> SaveChangesAsync(KeyType userId, CancellationToken cancellationToken = default)
    {
        OnBeforeSaveChanges(userId);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
