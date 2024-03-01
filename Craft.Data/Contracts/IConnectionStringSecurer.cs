namespace Craft.Data.Contracts;

public interface IConnectionStringSecurer
{
    string MakeSecure(string connectionString, string dbProvider = null);
}
