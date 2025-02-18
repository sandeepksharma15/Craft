using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Craft.Infrastructure.ConfigurationProviders;

public class EncryptedConfigurationProvider : ConfigurationProvider
{
    private readonly IConfigurationRoot _configuration;
    private readonly bool _isProduction;

    public EncryptedConfigurationProvider(IConfigurationRoot configuration, bool isProduction)
    {
        _configuration = configuration;
        _isProduction = isProduction;
    }

    public override void Load()
    {
        foreach (var kvp in _configuration.AsEnumerable())
        {
            if (kvp.Value is not null)
            {
                string decryptedValue = _isProduction ? Decrypt(kvp.Value) : kvp.Value;
                Data[kvp.Key] = decryptedValue;
            }
        }
    }

    private static string Decrypt(string encryptedText)
    {
        try
        {
            string key = Environment.GetEnvironmentVariable("AES_ENCRYPTION_KEY") ?? throw new InvalidOperationException("Encryption Key not found");
            string iv = Environment.GetEnvironmentVariable("AES_ENCRYPTION_IV") ?? throw new InvalidOperationException("Encryption IV not found");

            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] buffer = Convert.FromBase64String(encryptedText);

            return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
        }
        catch
        {
            return encryptedText; // Fallback if decryption fails
        }
    }
}
