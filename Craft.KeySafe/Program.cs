// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2 || (args[0] != "-e" && args[0] != "-d"))
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  Encrypt:   KeySafe.exe -e \"Your text here\"");
    Console.WriteLine("  Decrypt:   KeySafe.exe -d \"Your encrypted text here\"");
    return;
}

string mode = args[0];
string inputText = args[1];

try
{
    string? key = Environment.GetEnvironmentVariable("AES_ENCRYPTION_KEY");
    string? iv = Environment.GetEnvironmentVariable("AES_ENCRYPTION_IV");

    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
    {
        Console.WriteLine("Error: AES_ENCRYPTION_KEY or AES_ENCRYPTION_IV is not set in environment variables.");
        return;
    }

    if (mode == "-e")
    {
        string encrypted = Encrypt(inputText, key, iv);
        Console.WriteLine($"Encrypted: {encrypted}");
    }
    else if (mode == "-d")
    {
        string decrypted = Decrypt(inputText, key, iv);
        Console.WriteLine($"Decrypted: {decrypted}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

static string Encrypt(string plainText, string key, string iv)
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

static string Decrypt(string cipherText, string key, string iv)
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
