using System.ComponentModel.DataAnnotations;
using Craft.Domain.Base;

namespace Craft.Testimonials.Domain;

public class Testimonial : EntityBase, ITestimonial
{
    public const int NameLength = 50;
    public const int FeedbackLength = 1000;
    public const int SearchTagsLength = 100;

    [Required]
    [MaxLength(NameLength)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(FeedbackLength)]
    public string Feedback { get; set; } = string.Empty;

    public int Rating { get; set; }

    public bool IsApproved { get; set; }

    [MaxLength(SearchTagsLength)]
    public string SearchTags { get; set; } = string.Empty;

    public KeyType UserId { get; set; }
}
