using System.ComponentModel.DataAnnotations.Schema;
using Craft.Domain.Base;

namespace Craft.TestHelper.Models;

public class Store : EntityBase
{
    public string City { get; set; }

    [ForeignKey("CompanyId")]
    public Company Company { get; set; }

    public long CompanyId { get; set; }
    public string Name { get; set; }
}
