namespace Craft.TestHelper.Models;

public class Country
{
    public List<Company> Companies { get; set; }
    public virtual long Id { get; set; }
    public string Name { get; set; }
}

public class CompanyName
{
    public string Name { get; set; }
}
