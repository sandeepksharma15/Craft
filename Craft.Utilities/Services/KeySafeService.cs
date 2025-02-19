using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Craft.Utilities.Services;

public class KeySafeService : IKeySafeService
{
    private readonly IConfiguration _configuration;
    private readonly bool _isProduction;
    private readonly string _keyString;
    private readonly string _ivString;

    public KeySafeService(IConfiguration configuration, bool isProduction)
    {
        _keyString = Environment.GetEnvironmentVariable("AES_ENCRYPTION_KEY")
             ?? throw new InvalidOperationException("Encryption Key not found");
        _ivString = Environment.GetEnvironmentVariable("AES_ENCRYPTION_IV")
            ?? throw new InvalidOperationException("Encryption IV not found");

        if (_keyString.Length != 32)
            throw new InvalidOperationException("Encryption Key must be 32 bytes for AES-256");

        if (_ivString.Length != 16)
            throw new InvalidOperationException("Encryption IV must be 16 bytes");

        _configuration = configuration;
        _isProduction = isProduction;
    }

    public string Decrypt(string cipherText)
        => Decrypt(cipherText, _keyString, _ivString);

    public string Encrypt(string plainText)
        => Encrypt(plainText, _keyString, _ivString);

    public string GetConfigKeyValue(string key)
    {
        var secretValue = _configuration[key];

        // Decrypt the secret if in production
        if (_isProduction && !string.IsNullOrEmpty(secretValue))
            return Decrypt(secretValue);

        // Return as-is if not in production (development mode)
        return secretValue; 
    }

    public static string Encrypt(string plainText, string key, string iv)
    {
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentException("Plain text cannot be null or empty.", nameof(plainText));

        if (string.IsNullOrEmpty(key) || key.Length != 32)
            throw new ArgumentException("Encryption Key must be 32 bytes for AES-256.", nameof(key));

        if (string.IsNullOrEmpty(iv) || iv.Length != 16)
            throw new ArgumentException("Encryption IV must be 16 bytes.", nameof(iv));

        try
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using (var writer = new StreamWriter(cs))
            {
                writer.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }
        catch (CryptographicException ex)
        {
            throw new InvalidOperationException("Encryption failed. The provided key and IV may be incorrect.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred during encryption.", ex);
        }
    }
    public static string Decrypt(string cipherText, string key, string iv)
    {
        if (string.IsNullOrEmpty(cipherText))
            throw new ArgumentException("Cipher text cannot be null or empty.", nameof(cipherText));

        if (string.IsNullOrEmpty(key) || key.Length != 32)
            throw new ArgumentException("Encryption Key must be 32 bytes for AES-256.", nameof(key));

        if (string.IsNullOrEmpty(iv) || iv.Length != 16)
            throw new ArgumentException("Encryption IV must be 16 bytes.", nameof(iv));

        try
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cs);

            return reader.ReadToEnd();
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("Cipher text is not in a valid Base64 format.", nameof(cipherText), ex);
        }
        catch (CryptographicException ex)
        {
            throw new InvalidOperationException("Decryption failed. The provided key and IV may be incorrect.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred during decryption.", ex);
        }
    }

    public string GetIV() => _ivString;

    public string GetKey() => _keyString;
}

public interface IKeySafeService
{
    string GetKey();
    string GetIV();

    string Encrypt(string plainText);
    string Decrypt(string cipherText);
    string GetConfigKeyValue(string key);
}
