using System.ComponentModel.DataAnnotations.Schema;
using Craft.Auditing.Attributes;
using Craft.Domain.Contracts;
using Craft.Security.Contracts;

namespace Craft.Security.Models;

public class RefreshToken<TKey> : IRefreshToken<TKey> where TKey : IEquatable<TKey>
{
    public RefreshToken() { }

    public RefreshToken(string refreshToken, DateTime refreshTokenExpiryTime, TKey userId)
    {
        Token = refreshToken;
        ExpiryTime = refreshTokenExpiryTime;
        UserId = userId;
    }

    public DateTime ExpiryTime { get; set; }

    public TKey Id { get; set; }

    // IsDeleted is not used in this class, but is required as there is a GlobalQueryFilter
    // On CraftUser that requires it.
    public bool IsDeleted { get; set; }

    public string Token { get; set; }

    [ForeignKey("UserId")]
    public TKey UserId { get; set; }
}

[NoAudit]
[Table("ID_RefreshTokens")]
public class RefreshToken : RefreshToken<KeyType>, IEntity, IModel;
