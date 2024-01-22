using Craft.TestHelper.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Craft.TestHelper.Fixtures;

public class TestDbContext(DbContextOptions<TestDbContext> options) : IdentityDbContext<TestUser, TestRole, int>(options)
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Store> Stores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().Navigation(c => c.Companies).AutoInclude();

        _ = modelBuilder.Entity<Country>().HasData(CountrySeed.Get());
        _ = modelBuilder.Entity<Company>().HasData(CompanySeed.Get());
        _ = modelBuilder.Entity<Store>().HasData(StoreSeed.Get());
    }
}
