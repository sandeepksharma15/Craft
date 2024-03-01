using System.ComponentModel.DataAnnotations.Schema;
using Craft.Domain.Base;

namespace Craft.TestHelper.Models;

public class Company : EntityBase
{
    [ForeignKey("CountryId")]
    public Country Country { get; set; }

    public long CountryId { get; set; }
    public string Name { get; set; }
    public List<Store> Stores { get; set; }
}

public class CompanyName
{
    public string Name { get; set; }
}
