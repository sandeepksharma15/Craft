using System.ComponentModel.DataAnnotations.Schema;
using Craft.Domain.Base;

namespace Craft.FAQ.Domain;

[Table("FQ_Sections")]
public class FaqSection : EntityBase, IFaqSection
{
    public string Title { get; set; }

    public ICollection<FaqSubSection> SubSections { get; set; } = [];
    public ICollection<FaqQuery> Queries { get; set; } = [];
}
