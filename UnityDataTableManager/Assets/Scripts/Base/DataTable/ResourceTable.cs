using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceTable : DataTable
{
    public class Data
    {
        public int ID { get; set; }
        public int NameId { get; set; }
        public ResourceType Type { get; set; }
        public int StartQuantity { get; set; }
        public int TurnUsage { get; set; }
    }

    private Dictionary<int, Data> dict = new Dictionary<int, Data>();

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
        var data = GetValues().Where(p => p.Type == type).FirstOrDefault();
        return data;
    }

    public Data[] GetValues()
    {
        return dict.Values.ToArray();
    }
}
