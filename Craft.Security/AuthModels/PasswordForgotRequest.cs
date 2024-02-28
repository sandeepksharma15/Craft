using System.ComponentModel.DataAnnotations;

namespace Craft.Security.AuthModels;

public class PasswordForgotRequest
{
    [Required]
    public string ClientURI { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
