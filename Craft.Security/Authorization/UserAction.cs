using System.ComponentModel;

namespace Craft.Security.Authorization;

[Flags]
public enum UserAction : byte
{
    [Description("None")]
    None = 0,

    [Description("Create")]
    CanCreate = 1,

    [Description("Read")]
    CanRead = 2,

    [Description("Update")]
    CanUpdate = 4,

    [Description("Delete")]
    CanDelete = 8,

    [Description("CRUD")]
    CanDoCrud = CanCreate | CanRead | CanUpdate | CanDelete,

    [Description("Activate / Deactivate")]
    CanToggleActivate = 16,

    [Description("All")]
    CanDoAll = None | CanCreate | CanRead | CanUpdate | CanDelete | CanToggleActivate,
}
