namespace Craft.Domain.HashIdentityKey;

public static class KeyTypeExtensions
{
    public static string ToHashKey(this KeyType keyType)
    {
        var options = Activator.CreateInstance<HashKeyOptions>();
        var hashKeys = (HashKeys)Activator.CreateInstance(typeof(HashKeys), options);

        return hashKeys.EncodeLong(keyType);
    }

    public static KeyType ToKeyType(this string hashKey)
    {
        var options = Activator.CreateInstance<HashKeyOptions>();
        var hashKeys = (HashKeys)Activator.CreateInstance(typeof(HashKeys), options);

        return hashKeys.DecodeLong(hashKey)[0];
    }
}
