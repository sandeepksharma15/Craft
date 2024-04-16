using Craft.Data.Contracts;
using Craft.Data.Extensions;
using Craft.Data.Options;
using Craft.Domain.Contracts;
using Craft.MultiTenant.Contracts;
using Craft.Security.CurrentUserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Craft.Data.DbContexts;

public abstract class CraftAppDbContext : CraftDbContext, ICraftAppDbContext
{
    private readonly ITenant _currentTenant;
    private readonly ICurrentUser _currentUser;
    private readonly DatabaseOptions _dbOptions;

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>")]
    protected CraftAppDbContext(DbContextOptions options, IOptions<DatabaseOptions> dbOptions,
        ITenant currentTenant, ICurrentUser currentUser) : base(options)
    {
        _dbOptions = dbOptions.Value;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.EnableSensitiveDataLogging(_dbOptions.EnableSensitiveDataLogging);
        optionsBuilder.EnableDetailedErrors(_dbOptions.EnableDetailedErrors);

        if (!_currentTenant.ConnectionString.IsNullOrEmpty())
            optionsBuilder.UseDatabase(_dbOptions.DbProvider, _currentTenant.ConnectionString,
                _dbOptions.MaxRetryCount, _dbOptions.MaxRetryDelay, _dbOptions.CommandTimeout);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // QueryFilters need to be applied before base.OnModelCreating
        builder.AddGlobalQueryFilter<ISoftDelete>(s => !s.IsDeleted);

        base.OnModelCreating(builder);
    }

    public override int SaveChanges()
    {
        OnBeforeSaveChanges(_currentUser.GetId());

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaveChanges(_currentUser.GetId());

        return await base.SaveChangesAsync(cancellationToken);
    }
}
