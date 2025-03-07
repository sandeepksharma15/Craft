﻿using System.Diagnostics.CodeAnalysis;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Collections.Generic;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class EnumerableExtensions
{
    public static Dictionary<string, string> GetListDataForSelect<T>([NotNull] this IEnumerable<T> items,
        string valueField, string displayField)
    {
        Dictionary<string, string> listItems = [];

        foreach (T item in items)
        {
            if (valueField != null && displayField != null)
            {
                listItems.Add(
                   item!.GetType()!.GetProperty(valueField)!.GetValue(item)!.ToString()!,
                   item.GetType()?.GetProperty(displayField)?.GetValue(item)?.ToString());
            }
            else
            {
                listItems.Add(item!.ToString()!, item.ToString());
            }
        }

        return listItems!;
    }
}
