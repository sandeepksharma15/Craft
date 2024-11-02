using System.ComponentModel.DataAnnotations.Schema;
using Craft.Auditing.Attributes;
using Craft.Domain.Contracts;
using Craft.Security.Contracts;

namespace Craft.Security.Models;

public class LoginHistory<TKey> : ILoginHistory<TKey> where TKey : IEquatable<TKey>
{
    public LoginHistory() { }

    public LoginHistory(string lastIpAddress, DateTime? lastLoginOn, TKey userId)
    {
        LastIpAddress = lastIpAddress;
        LastLoginOn = lastLoginOn;
        UserId = userId;
    }

    public TKey Id { get; set; }

    // IsDeleted is not used in this class, but is required as there is a GlobalQueryFilter
    // On CraftUser that requires it.
    public bool IsDeleted { get; set; }

    public string LastIpAddress { get; set; }

    public DateTime? LastLoginOn { get; set; }

    [ForeignKey("UserId")]
    public TKey UserId { get; set; }
}

[NoAudit]
[Table("ID_LoginHistory")]
public class LoginHistory : LoginHistory<KeyType>, IEntity, IModel;
