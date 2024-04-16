using Craft.Auditing;
using Craft.Data.Converters;
using Craft.Security.Models;
using Craft.Auditing.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Craft.Domain.Contracts;
using System.Diagnostics.CodeAnalysis;
using Craft.Data.Contracts;

namespace Craft.Data.DbContexts;

public abstract class CraftDbContext : IdentityDbContext<CraftUser, CraftRole, KeyType,
    IdentityUserClaim<KeyType>, IdentityUserRole<KeyType>, IdentityUserLogin<KeyType>,
    IdentityRoleClaim<KeyType>, IdentityUserToken<KeyType>>, ICraftDbContext
{
    protected static readonly ILoggerFactory loggerFactory
        = LoggerFactory.Create(builder => builder.AddConsole());

    public DbSet<AuditTrail> AuditTrails { get; set; }
    public DbSet<LoginHistory> LoginHistories { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    //[SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>")]
    protected CraftDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override sealed void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTime>()
            .HaveConversion(typeof(DateTimeToDateTimeUtc));
    }

    protected void OnBeforeSaveChanges(KeyType userId)
    {
        List<AuditTrail> auditLogs = [];

        ChangeTracker.DetectChanges();

        foreach (EntityEntry entry in ChangeTracker.Entries())
        {
            if (entry.Entity.GetType().HasNoAuditAttribute())
                continue;

            if (entry.Entity is AuditTrail || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            auditLogs.Add(new AuditTrail(entry, userId));

            // Upate the ConcurrencyStamp
            if (entry.Entity is IHasConcurrency concurrency)
                concurrency.SetConcurrencyStamp();

            // Upate the Version
            if (entry.Entity is IHasVersion version)
                version.IncrementVersion();
        }

        AuditTrails.AddRange(auditLogs);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(loggerFactory);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        // Comment out the following line to disable client query evaluation
        // optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CraftRole>(b => b.ToTable("Id_Roles"));
        builder.Entity<CraftUser>(b => b.ToTable("Id_Users"));
        builder.Entity<IdentityRoleClaim<KeyType>>(b => b.ToTable("Id_RoleClaims"));
        builder.Entity<IdentityUserClaim<KeyType>>(b => b.ToTable("Id_UserClaims"));
        builder.Entity<IdentityUserLogin<KeyType>>(b => b.ToTable("Id_Logins"));
        builder.Entity<IdentityUserRole<KeyType>>(b => b.ToTable("Id_UserRoles"));
        builder.Entity<IdentityUserToken<KeyType>>(b => b.ToTable("Id_UserTokens"));
    }
}
