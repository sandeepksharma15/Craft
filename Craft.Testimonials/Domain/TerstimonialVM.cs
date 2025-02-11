using Craft.Domain.Base;

namespace Craft.Testimonials.Domain;

public class TerstimonialVM : VmBase, ITestimonial
{
    public string Name { get; set; } = string.Empty;

    public string Feedback { get; set; } = string.Empty;

    public int Rating { get; set; }

    public bool IsApproved { get; set; }

    public string SearchTags { get; set; } = string.Empty;

    public KeyType UserId { get; set; }
}
