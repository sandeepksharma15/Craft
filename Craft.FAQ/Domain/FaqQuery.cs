using System.ComponentModel.DataAnnotations.Schema;
using Craft.Domain.Base;

namespace Craft.FAQ.Domain;

[Table("FQ_Queries")]
public class FaqQuery : EntityBase, IFaqQuery
{
    public string Question { get; set; }
    public string Answer { get; set; }

    [ForeignKey(nameof(FaqSectionId))]
    public virtual FaqSection? FaqSection { get; set; }
    public KeyType? FaqSectionId { get; set; }

    [ForeignKey(nameof(FaqSubSectionId))]
    public virtual FaqSubSection? FaqSubSection { get; set; }
    public KeyType? FaqSubSectionId { get; set; }
}
