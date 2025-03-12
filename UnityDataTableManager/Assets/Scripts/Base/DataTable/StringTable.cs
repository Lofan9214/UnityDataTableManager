using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StringTable : DataTable
{
    public class Data : DataTableData
    {
        public int stringID { get; set; }
        public string line { get; set; }

        public override void Set(string[] argument)
        {
            stringID = int.Parse(argument[0]);
            line = argument[1];
        }
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
            if (!dict.ContainsKey(item.stringID))
            {
                dict.Add(item.stringID, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.stringID}");
            }
        }
    }

    public string Get(int key)
    {
        if (!dict.ContainsKey(key))
        {
            return "NULL";
        }
        return dict[key].line;
    }

    public override void Save(string path)
    {
        SaveCsv(path, dict.Values.ToList());
    }

    public override void Set(List<string[]> data)
    {
        var properties = typeof(Data).GetProperties();
        var dictionary = new Dictionary<int, Data>();
        for (int i = 0; i < data.Count; ++i)
        {
            Data datum = new Data();
            datum.Set(data[i]);
            dictionary.Add(datum.stringID, datum);
        }

        dict = dictionary;
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
}
