using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EventTable : DataTable
{
    public class Data : DataTableData
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public int Script { get; set; }
        public int RelativeProbability { get; set; }
        public string DetailedType { get; set; }
        public int AcceptScript { get; set; }

        /// <summary>
        /// 타겟값#수량_타겟값#수량 생각중
        /// </summary>
        public string AcceptResult { get; set; }
        public int NeglectScript { get; set; }

        /// <summary>
        /// 타겟값#수량_타겟값#수량 생각중
        /// </summary>
        public string NeglectResult { get; set; }

        public override void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            Type = int.Parse(argument[1]);
            Script = int.Parse(argument[2]);
            RelativeProbability = int.Parse(argument[3]);
            DetailedType = argument[4];
            AcceptScript = int.Parse(argument[5]);
            AcceptResult = argument[6];
            NeglectScript = int.Parse(argument[7]);
            NeglectResult = argument[8];
        }
        public override int GetID()
        {
            return ID;
        }
    }

    private Dictionary<int, Data> dict = new Dictionary<int, Data>();

    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var textAsset = Resources.Load<TextAsset>(path);
        //var textAsset = Addressables.LoadAssetAsync<TextAsset>(path);
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

    public Data[] GetValues()
    {
        return dict.Values.ToArray();
    }

    public override void Save(string fileName)
    {
        string path = string.Format(FormatPath, fileName);
        SaveCsv(path, dict.Values.ToList());
    }

    public override void Set(List<string[]> data)
    {
        var dictionary = new Dictionary<int, Data>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            dictionary.Add(datum.ID, datum);
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
