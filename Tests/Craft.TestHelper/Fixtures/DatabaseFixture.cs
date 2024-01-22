using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Craft.TestHelper.Fixtures;

public class DatabaseFixture : IDisposable
{
    public TestDbContext DbContext { get; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        DbContext = new TestDbContext(options);

        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

[CollectionDefinition("DatabaseCollection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{ }
