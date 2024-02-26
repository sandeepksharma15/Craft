using System.ComponentModel;

namespace Craft.Domain.Enums;

public enum HonorificType
{
    [Description("Dr")]
    Dr,

    [Description("Prof")]
    Prof,

    [Description("Mr")]
    Mr,

    [Description("Ms")]
    Ms,

    [Description("Mrs")]
    Mrs
}
