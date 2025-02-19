using System.Text;
using Craft.Utilities.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Craft.Utilities.Tests.Services;

public class KeySafeServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly KeySafeService _keySafeService;
    private const string ValidKey = "12345678901234567890123456789012"; // 32 bytes key
    private const string ValidIV = "1234567890123456"; // 16 bytes IV
    private const string PlainText = "Hello, World!";
    private readonly string _encryptedText;

    public KeySafeServiceTests()
    {
        Environment.SetEnvironmentVariable("AES_ENCRYPTION_KEY", ValidKey);
        Environment.SetEnvironmentVariable("AES_ENCRYPTION_IV", ValidIV);

        _mockConfiguration = new Mock<IConfiguration>();
        _keySafeService = new KeySafeService(_mockConfiguration.Object, true);
        _encryptedText = KeySafeService.Encrypt(PlainText, ValidKey, ValidIV);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenKeyNotFound()
    {
        Environment.SetEnvironmentVariable("AES_ENCRYPTION_KEY", null);
        Assert.Throws<InvalidOperationException>(() => new KeySafeService(_mockConfiguration.Object, true));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenIVNotFound()
    {
        Environment.SetEnvironmentVariable("AES_ENCRYPTION_IV", null);
        Assert.Throws<InvalidOperationException>(() => new KeySafeService(_mockConfiguration.Object, true));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenKeyLengthIsInvalid()
    {
        Environment.SetEnvironmentVariable("AES_ENCRYPTION_KEY", "shortkey");
        Assert.Throws<InvalidOperationException>(() => new KeySafeService(_mockConfiguration.Object, true));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenIVLengthIsInvalid()
    {
        Environment.SetEnvironmentVariable("AES_ENCRYPTION_IV", "shortiv");
        Assert.Throws<InvalidOperationException>(() => new KeySafeService(_mockConfiguration.Object, true));
    }

    [Fact]
    public void Encrypt_ShouldThrowArgumentException_WhenPlainTextIsNull()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Encrypt(null, ValidKey, ValidIV));
    }

    [Fact]
    public void Encrypt_ShouldThrowArgumentException_WhenPlainTextIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Encrypt(string.Empty, ValidKey, ValidIV));
    }

    [Fact]
    public void Encrypt_ShouldThrowArgumentException_WhenKeyIsNull()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Encrypt(PlainText, null, ValidIV));
    }

    [Fact]
    public void Encrypt_ShouldThrowArgumentException_WhenKeyIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Encrypt(PlainText, string.Empty, ValidIV));
    }

    [Fact]
    public void Encrypt_ShouldThrowArgumentException_WhenKeyLengthIsInvalid()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Encrypt(PlainText, "shortkey", ValidIV));
    }

    [Fact]
    public void Encrypt_ShouldThrowArgumentException_WhenIVIsNull()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Encrypt(PlainText, ValidKey, null));
    }

    [Fact]
    public void Encrypt_ShouldThrowArgumentException_WhenIVIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Encrypt(PlainText, ValidKey, string.Empty));
    }

    [Fact]
    public void Encrypt_ShouldThrowArgumentException_WhenIVLengthIsInvalid()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Encrypt(PlainText, ValidKey, "shortiv"));
    }

    [Fact]
    public void Encrypt_ShouldReturnEncryptedString_WhenInputIsValid()
    {
        var encryptedText = KeySafeService.Encrypt(PlainText, ValidKey, ValidIV);
        Assert.NotNull(encryptedText);
        Assert.NotEqual(PlainText, encryptedText);
    }

    [Fact]
    public void Decrypt_ShouldThrowArgumentException_WhenCipherTextIsNull()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Decrypt(null, ValidKey, ValidIV));
    }

    [Fact]
    public void Decrypt_ShouldThrowArgumentException_WhenCipherTextIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Decrypt(string.Empty, ValidKey, ValidIV));
    }

    [Fact]
    public void Decrypt_ShouldThrowArgumentException_WhenKeyIsNull()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Decrypt(_encryptedText, null, ValidIV));
    }

    [Fact]
    public void Decrypt_ShouldThrowArgumentException_WhenKeyIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Decrypt(_encryptedText, string.Empty, ValidIV));
    }

    [Fact]
    public void Decrypt_ShouldThrowArgumentException_WhenKeyLengthIsInvalid()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Decrypt(_encryptedText, "shortkey", ValidIV));
    }

    [Fact]
    public void Decrypt_ShouldThrowArgumentException_WhenIVIsNull()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Decrypt(_encryptedText, ValidKey, null));
    }

    [Fact]
    public void Decrypt_ShouldThrowArgumentException_WhenIVIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Decrypt(_encryptedText, ValidKey, string.Empty));
    }

    [Fact]
    public void Decrypt_ShouldThrowArgumentException_WhenIVLengthIsInvalid()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Decrypt(_encryptedText, ValidKey, "shortiv"));
    }

    [Fact]
    public void Decrypt_ShouldThrowArgumentException_WhenCipherTextIsNotBase64()
    {
        Assert.Throws<ArgumentException>(() => KeySafeService.Decrypt("InvalidBase64", ValidKey, ValidIV));
    }

    [Fact]
    public void Decrypt_ShouldThrowInvalidOperationException_WhenDecryptionFails()
    {
        var invalidEncryptedText = Convert.ToBase64String(Encoding.UTF8.GetBytes("InvalidEncryptedText"));
        Assert.Throws<InvalidOperationException>(() => KeySafeService.Decrypt(invalidEncryptedText, ValidKey, ValidIV));
    }

    [Fact]
    public void Decrypt_ShouldReturnPlainText_WhenInputIsValid()
    {
        var decryptedText = KeySafeService.Decrypt(_encryptedText, ValidKey, ValidIV);
        Assert.Equal(PlainText, decryptedText);
    }

    [Fact]
    public void GetConfigKeyValue_ShouldReturnDecryptedValue_WhenInProduction()
    {
        var encryptedValue = KeySafeService.Encrypt("SecretValue", ValidKey, ValidIV);
        _mockConfiguration.Setup(c => c["TestKey"]).Returns(encryptedValue);

        var result = _keySafeService.GetConfigKeyValue("TestKey");

        Assert.Equal("SecretValue", result);
    }

    [Fact]
    public void GetConfigKeyValue_ShouldReturnPlainValue_WhenNotInProduction()
    {
        var plainValue = "PlainValue";
        _mockConfiguration.Setup(c => c["TestKey"]).Returns(plainValue);

        var keySafeService = new KeySafeService(_mockConfiguration.Object, false);
        var result = keySafeService.GetConfigKeyValue("TestKey");

        Assert.Equal(plainValue, result);
    }

    [Fact]
    public void GetKey_ShouldReturnKey()
    {
        var key = _keySafeService.GetKey();
        Assert.Equal(ValidKey, key);
    }

    [Fact]
    public void GetIV_ShouldReturnIV()
    {
        var iv = _keySafeService.GetIV();
        Assert.Equal(ValidIV, iv);
    }
}
