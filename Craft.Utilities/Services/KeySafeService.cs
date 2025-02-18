using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Craft.Utilities.Services;

public class KeySafeService : IKeySafeService
{
    private readonly string _key;
    private readonly string _iv;
    private readonly IConfiguration _configuration;
    private readonly bool _isProduction;

    public KeySafeService(IConfiguration configuration, bool isProduction)
    {
        _key = Environment.GetEnvironmentVariable("AES_ENCRYPTION_KEY")
            ?? throw new InvalidOperationException("Encryption Key not found");
        _iv = Environment.GetEnvironmentVariable("AES_ENCRYPTION_IV")
            ?? throw new InvalidOperationException("Encryption IV not found");
        _configuration = configuration;
        _isProduction = isProduction;
    }

    public string Decrypt(string cipherText)
    {
        try
        {
            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.IV = Encoding.UTF8.GetBytes(_iv);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cs);

            return reader.ReadToEnd();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string Encrypt(string plainText)
    {
        try
        {
            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.IV = Encoding.UTF8.GetBytes(_iv);

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using (var writer = new StreamWriter(cs))
            {
                writer.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string GetConfigKeyValue(string key)
    {
        var secretValue = _configuration[key];

        if (_isProduction && !string.IsNullOrEmpty(secretValue))
        {
            // Decrypt the secret if in production
            return Decrypt(secretValue);
        }

        return secretValue; // Return as-is if not in production (development mode)
    }

    public string GetIV() => _iv;

    public string GetKey() => _key;
}

public interface IKeySafeService
{
    string GetKey();
    string GetIV();

    string Encrypt(string plainText);
    string Decrypt(string cipherText);
    string GetConfigKeyValue(string key);
}
