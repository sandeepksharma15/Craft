namespace Craft.Security.Authorization;

public class ModuleItem
{
    public string DisplayName { get; set; }
    public byte Id { get; set; }
    public string Name { get; set; }
    public UserAction PossibleActions { get; set; } = UserAction.CanDoCrud;
}
