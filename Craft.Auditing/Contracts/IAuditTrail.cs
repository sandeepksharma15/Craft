﻿using Craft.Auditing.Enums;
using Craft.Domain.Contracts;

namespace Craft.Auditing.Contracts;

public interface IAuditTrail : ISoftDelete, IHasUser
{
    public string ChangedColumns { get; set; }
    public EntityChangeType ChangeType { get; set; }
    public DateTime DateTimeUTC { get; set; }

    public string KeyValues { get; set; }
    public string NewValues { get; set; }
    public string OldValues { get; set; }
    public bool ShowDetails { get; set; }
    public string TableName { get; set; }
}
