using System.ComponentModel.DataAnnotations.Schema;

namespace Craft.TestHelper.Models;

public class Store
{
    public string City { get; set; }

    [ForeignKey("CompanyId")]
    public Company Company { get; set; }

    public long CompanyId { get; set; }
    public virtual long Id { get; set; }
    public string Name { get; set; }
}
