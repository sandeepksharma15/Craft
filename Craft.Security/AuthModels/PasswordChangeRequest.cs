using System.ComponentModel.DataAnnotations;

namespace Craft.Security.AuthModels;

public class PasswordChangeRequest<TKey>
{
    [Required]
    [DataType(DataType.Password)]
    public string ConfirmNewPassword { get; set; } = default!;

    public TKey Id { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required]
    public string Token { get; set; }
}

public class PasswordChangeRequest : PasswordChangeRequest<KeyType>;
