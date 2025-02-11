using System.ComponentModel.DataAnnotations;
using Craft.Domain.Base;

namespace Craft.Testimonials.Domain;

public class Testimonial : EntityBase, ITestimonial
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Feedback { get; set; } = string.Empty;

    public int Rating { get; set; }

    public bool IsApproved { get; set; }

    [MaxLength(100)]
    public string SearchTags { get; set; } = string.Empty;

    public KeyType UserId { get; set; }
}
