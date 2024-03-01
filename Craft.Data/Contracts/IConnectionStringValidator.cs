namespace Craft.Data.Contracts;

public interface IConnectionStringValidator
{
    bool TryValidate(string connectionString, string dbProvider = null);
}
