using HashidsNet;

namespace Craft.Domain.HashIdentityKey;

public class HashKeys(HashKeyOptions options)
    : Hashids(options.Salt, options.MinHashLength, options.Alphabet, options.Steps), IHashKeys
{
}
