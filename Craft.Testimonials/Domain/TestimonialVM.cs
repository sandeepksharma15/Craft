using System.ComponentModel.DataAnnotations;
using Craft.Domain.Base;
using Craft.Domain.Helpers;

namespace Craft.Testimonials.Domain;

public class TestimonialVM : VmBase, ITestimonial
{
    [Required(ErrorMessage = DomainConstants.RequiredError)]
    [StringLength(Testimonial.NameLength, ErrorMessage = DomainConstants.LengthError)]
    [RegularExpression(DomainConstants.NameRegExpr, ErrorMessage = DomainConstants.AlphabetAndSpecialCharError)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = DomainConstants.RequiredError)]
    [StringLength(Testimonial.FeedbackLength, ErrorMessage = DomainConstants.LengthError)]
    [Display(Name = "Feedback")]
    public string Feedback { get; set; } = string.Empty;

    [Required(ErrorMessage = DomainConstants.RequiredError)]
    [Range(1, 5, ErrorMessage = DomainConstants.RangeError)]
    [Display(Name = "Rating")]
    public int Rating { get; set; }

    [Display(Name = "Approved")]
    public bool IsApproved { get; set; }

    [Display(Name = "Search Tags")]
    public string SearchTags { get; set; } = string.Empty;

    public KeyType UserId { get; set; }
}
