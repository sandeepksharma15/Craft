using Craft.Domain.Base;

namespace Craft.TestHelper.Models;

public class Country : EntityBase
{
    public List<Company> Companies { get; set; }
    public string Name { get; set; }
}

public class CompanyName
{
    public string Name { get; set; }
}
