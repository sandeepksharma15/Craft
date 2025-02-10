using System.ComponentModel.DataAnnotations;
using Craft.Domain.Base;

namespace Craft.CustomerFeedback.Domain;

public class CustFeedback : EntityBase, ICustFeedback
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Feedback { get; set; } = string.Empty;

    public int Rating { get; set; }

    public bool IsApproved { get; set; }

    [Required]
    public KeyType UserId { get; set; }
}
