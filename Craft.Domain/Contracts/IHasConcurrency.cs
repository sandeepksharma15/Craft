namespace Craft.Domain.Contracts;

public interface IHasConcurrency
{
    public const string ColumnName = "ConcurrencyStamp";

    public const int MaxLength = 40;

    string ConcurrencyStamp { get; set; }

    public string GetConcurrencyStamp() => ConcurrencyStamp;

    public bool HasConcurrencyStamp() => !ConcurrencyStamp.IsNullOrWhiteSpace();

    public void SetConcurrencyStamp(string stamp = null)
        => ConcurrencyStamp = stamp ?? Guid.NewGuid().ToString();
}
