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
        public override int GetID()
        {
            return stringID;
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
        var dictionary = new Dictionary<int, Data>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            dictionary.Add(datum.GetID(), datum);
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
