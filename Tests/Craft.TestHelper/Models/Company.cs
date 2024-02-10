using System.ComponentModel.DataAnnotations.Schema;

namespace Craft.TestHelper.Models;

public class Company
{
    [ForeignKey("CountryId")]
    public Country Country { get; set; }

    public long CountryId { get; set; }
    public virtual long Id { get; set; }
    public string Name { get; set; }
    public List<Store> Stores { get; set; }
}
