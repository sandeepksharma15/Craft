using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Craft.Auditing.Contracts;
using Craft.Auditing.Enums;
using Craft.Domain.Base;
using Craft.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Craft.Auditing;

[Table("HK_AuditTrail")]
public class AuditTrail : EntityBase, IAuditTrail
{
    public string ChangedColumns { get; set; }
    public EntityChangeType ChangeType { get; set; }
    public DateTime DateTimeUTC { get; set; } = DateTime.UtcNow;

    public string KeyValues { get; set; }
    public string NewValues { get; set; }
    public string OldValues { get; set; }
    public bool ShowDetails { get; set; }
    public string TableName { get; set; }
    public KeyType UserId { get; set; }

    public AuditTrail() { }

    public AuditTrail(EntityEntry entity, KeyType userId)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        CreateAuditEntry(entity);

        UserId = userId;
    }

    private static void GetAddedValues(EntityEntry entity, Dictionary<string, object> keyValues,
        Dictionary<string, object> newValues)
    {
        foreach (PropertyEntry property in entity.Properties)
        {
            string propertyName = property.Metadata.Name;

            if (property.Metadata.IsPrimaryKey())
                keyValues[propertyName] = property.CurrentValue;

            newValues[propertyName] = property.CurrentValue;
        }
    }

    private static void GetDeletedValues(EntityEntry entity, Dictionary<string, object> keyValues,
        Dictionary<string, object> oldValues)
    {
        foreach (PropertyEntry property in entity.Properties)
        {
            string propertyName = property.Metadata.Name;

            if (property.Metadata.IsPrimaryKey())
                keyValues[propertyName] = property.CurrentValue;

            oldValues[propertyName] = property.OriginalValue;
        }
    }

    private static void GetUpdatedValues(EntityEntry entity, Dictionary<string, object> keyValues,
        Dictionary<string, object> oldValues, Dictionary<string, object> newValues, List<string> changedColumns)
    {
        foreach (PropertyEntry property in entity.Properties)
        {
            string propertyName = property.Metadata.Name;
            string dbColumnName = property.Metadata.GetFieldName();

            if (property.Metadata.IsPrimaryKey())
                keyValues[propertyName] = property.CurrentValue;

            if (property.IsModified)
            {
                if (property.CurrentValue != property.OriginalValue)
                    changedColumns.Add(dbColumnName);

                oldValues[propertyName] = property.OriginalValue;
                newValues[propertyName] = property.CurrentValue;
            }
        }
    }

    private static bool IsItSoftDelete(EntityEntry entity)
    {
        foreach (PropertyEntry property in entity.Properties)
            if (property.Metadata.Name == ISoftDelete.ColumnName && property.CurrentValue is true)
                return true;

        return false;
    }

    private void CreateAuditEntry(EntityEntry entity)
    {
        Dictionary<string, object> keyValues = [];
        Dictionary<string, object> oldValues = [];
        Dictionary<string, object> newValues = [];
        List<string> changedColumns = [];

        switch (entity.State)
        {
            case EntityState.Added:
                ChangeType = EntityChangeType.Created;
                GetAddedValues(entity, keyValues, newValues);
                break;

            case EntityState.Deleted:
                // If someone tries hard delete on a soft delete entity
                if (entity.Entity is ISoftDelete)
                    entity.State = EntityState.Modified;

                ChangeType = EntityChangeType.Deleted;
                GetDeletedValues(entity, keyValues, oldValues);
                break;

            case EntityState.Modified:
                // Just to be sure that it is not a soft delete
                if (IsItSoftDelete(entity))
                {
                    ChangeType = EntityChangeType.Deleted;
                    GetDeletedValues(entity, keyValues, oldValues);
                }
                else
                {
                    ChangeType = EntityChangeType.Updated;
                    GetUpdatedValues(entity, keyValues, oldValues, newValues, changedColumns);
                }
                break;

            case EntityState.Detached:
            case EntityState.Unchanged:
                ChangeType = EntityChangeType.None;
                break;
        }

        // Time To Set All The Values In This Audit Entity
        TableName = entity.Metadata.DisplayName();
        DateTimeUTC = DateTime.UtcNow;
        KeyValues = JsonSerializer.Serialize(keyValues);
        ChangedColumns = changedColumns.Count == 0 ? null : JsonSerializer.Serialize(changedColumns);
        OldValues = oldValues.Count == 0 ? null : JsonSerializer.Serialize(oldValues);
        NewValues = newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues);
    }
}
