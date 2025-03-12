using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ResourceTable : DataTable
{
    public class Data : DataTableData
    {
        public int ID { get; set; }
        public int NameId { get; set; }
        public int Type { get; set; }
        public int StartQuantity { get; set; }
        public int TurnUsage { get; set; }

        public override void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            NameId = int.Parse(argument[1]);
            Type = int.Parse(argument[2]);
            StartQuantity = int.Parse(argument[3]);
            TurnUsage = int.Parse(argument[4]);
        }
    }

    private Dictionary<int, Data> dict = new Dictionary<int, Data>();
    protected PropertyInfo[] properties = typeof(Data).GetProperties();

    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCsv<Data>(textAsset.text);

        dict.Clear();

        foreach (var item in list)
        {
            if (!dict.ContainsKey(item.ID))
            {
                dict.Add(item.ID, item);
            }
            else
            {
                Debug.Assert(false, $"Key Duplicated: {item.ID}");
            }
        }
    }

    public Data Get(int key)
    {
        if (!dict.ContainsKey(key))
        {
            return null;
        }
        return dict[key];
    }

    public Data Get(ResourceType type)
    {
        var data = GetValues().Where(p => p.Type == (int)type).FirstOrDefault();
        return data;
    }

    public Data[] GetValues()
    {
        return dict.Values.ToArray();
    }

    public override void Save(string path)
    {
        SaveCsv(path, dict.Values.ToList());
    }

    public override Dictionary<int, DataTableData> TableData
    {
        get
        {
            var wrapDict = new Dictionary<int, DataTableData>();
            foreach (var item in dict)
            {
                wrapDict.Add(item.Key, item.Value);
            }
            return wrapDict;
        }
    }

    public override void Set(List<string[]> data)
    {
        var properties = typeof(Data).GetProperties();
        var dictionary = new Dictionary<int, Data>();
        for (int i = 0; i < data.Count; ++i)
        {
            Data datum = new Data();
            datum.Set(data[i]);
            dictionary.Add(datum.ID, datum);
        }

        dict = dictionary;
    }
}
