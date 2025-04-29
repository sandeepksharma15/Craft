using System.ComponentModel.DataAnnotations.Schema;
using Craft.Domain.Base;

namespace Craft.FAQ.Domain;

[Table("FQ_SubSections")]
public class FaqSubSection : EntityBase, IFaqSubSection
{
    public string Title { get; set; }

    [ForeignKey(nameof(FaqSectionId))]
    public virtual FaqSection FaqSection { get; set; }
    public KeyType FaqSectionId { get; set; }

    public ICollection<FaqQuery> Queries { get; set; } = [];
}
