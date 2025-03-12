using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public static class DataTableManager
{
    public static Dictionary<string, DataTable> Tables { get; private set; } = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
#if UNITY_EDITOR
        foreach (var id in DataTableIds.String)
        {
            var table = new StringTable();
            table.Load(id);
            Tables.Add(id, table);
        }
#else
        var table = new StringTable();
        var stringTableId = DataTableIds.String[(int)Variables.currentLanguage];
        table.Load(stringTableId);
        tables.Add(stringTableId, table);
#endif

        var resourceTable = new ResourceTable();
        var resourceTableId = DataTableIds.Resource;
        resourceTable.Load(resourceTableId);
        Tables.Add(resourceTableId, resourceTable);

        var eventTable = new EventTable();
        var eventTableId = DataTableIds.Event;
        eventTable.Load(eventTableId);
        Tables.Add(eventTableId, eventTable);
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!Tables.ContainsKey(id))
        {
            Debug.LogError("Table Not Exists");
            return null;
        }
        return Tables[id] as T;
    }

    public static StringTable StringTable
    {
        get
        {
            return Get<StringTable>(DataTableIds.String[(int)Variables.currentLanguage]);
        }
    }

    public static ResourceTable ResourceTable
    {
        get
        {
            return Get<ResourceTable>(DataTableIds.Resource);
        }
    }

    public static EventTable EventTable
    {
        get
        {
            return Get<EventTable>(DataTableIds.Event);
        }
    }
}
