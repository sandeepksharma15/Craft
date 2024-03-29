﻿using System.ComponentModel.DataAnnotations;

namespace Craft.Security.AuthModels;

public class UserLoginRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public string IpAddress { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; } = true;
}
