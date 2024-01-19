namespace Craft.TestHelper.Models;

public interface ISoftDelete
{
    public const string IsDeletedColumnName = "IsDeleted";

    bool IsDeleted { get; set; }

    public void Delete() => IsDeleted = true;

    public void Restore() => IsDeleted = false;
}
