using System.ComponentModel.DataAnnotations;

namespace Craft.Security.AuthModels;

public class PasswordResetRequest<TKey>
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public TKey Id { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public string Token { get; set; }
}

public class ResetPasswordRequest : PasswordResetRequest<KeyType>;
