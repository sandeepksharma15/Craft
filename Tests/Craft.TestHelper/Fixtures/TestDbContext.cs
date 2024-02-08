using Craft.TestHelper.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Craft.TestHelper.Fixtures;

public class TestDbContext(DbContextOptions<TestDbContext> options) : IdentityDbContext<TestUser, TestRole, int>(options)
{
    #region Public Properties

    public DbSet<Company> Companies { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Store> Stores { get; set; }

    #endregion Public Properties

    #region Protected Methods

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Country>().Navigation(c => c.Companies).AutoInclude();

        _ = builder.Entity<Country>().HasData(CountrySeed.Get());
        _ = builder.Entity<Company>().HasData(CompanySeed.Get());
        _ = builder.Entity<Store>().HasData(StoreSeed.Get());
    }

    #endregion Protected Methods
}