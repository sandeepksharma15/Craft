namespace Craft.Domain.Tests.Keys;

public class HashKeyOptions
{
    public string Alphabet { get; set; } = HashidsNet.Hashids.DEFAULT_ALPHABET;
    public int MinHashLength { get; set; } = 0;
    public string Salt { get; set; } = "";
    public string Steps { get; set; } = HashidsNet.Hashids.DEFAULT_SEPS;
}
