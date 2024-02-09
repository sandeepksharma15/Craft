using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Craft.TestHelper.Fixtures;

public class DatabaseFixture : IDisposable
{
    #region Public Properties

    public TestDbContext DbContext { get; }

    #endregion Public Properties

    #region Public Constructors

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        DbContext = new TestDbContext(options);

        DbContext.Database.EnsureCreated();
    }

    #endregion Public Constructors

    #region Public Methods

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    #endregion Public Methods
}

[CollectionDefinition("DatabaseCollection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>;
