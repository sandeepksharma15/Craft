﻿namespace Craft.Domain.HashIdentityKey;

public class HashKeyOptions
{
    public string Alphabet { get; set; } = HashidsNet.Hashids.DEFAULT_ALPHABET;
    public int MinHashLength { get; set; } = 10;
    public string Salt { get; set; } = "CraftDomainKeySalt";
    public string Steps { get; set; } = HashidsNet.Hashids.DEFAULT_SEPS;
}
