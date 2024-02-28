namespace Craft.Security.Authorization;

public class ModuleInfo
{
    public string DisplayName { get; set; }
    public byte Id { get; set; }
    public List<ModuleItem> Items { get; set; } = [];
    public string Name { get; set; }
}
