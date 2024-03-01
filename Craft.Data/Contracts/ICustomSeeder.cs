namespace Craft.Data.Contracts;

public interface ICustomSeeder
{
    Task InitializeAsync(CancellationToken cancellationToken);
}
