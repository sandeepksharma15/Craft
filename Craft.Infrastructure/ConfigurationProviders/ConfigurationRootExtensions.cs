using Craft.Utilities.Services;
using Microsoft.Extensions.Configuration;

namespace Craft.Infrastructure.ConfigurationProviders;

public static class ConfigurationRootExtensions
{
    public static IConfigurationRoot Decrypt(this IConfigurationRoot root, string cipherPrefix)
    {
        var cipher = new KeySafeService();

        DecryptInChildren(root);

        return root;

        void DecryptInChildren(IConfiguration parent)
        {
            foreach (var child in parent.GetChildren())
            {
                if (child.Value?.StartsWith(cipherPrefix) == true)
                {
                    var cipherText = child.Value[cipherPrefix.Length..];
                    parent[child.Key] = cipher.Decrypt(cipherText);
                }

                DecryptInChildren(child);
            }
        }
    }
}
