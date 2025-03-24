using System;
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

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        var list = LoadCsv<Data>(text);
        dict.Clear();
        TableData.Clear();

        foreach (var item in list)
        {
            if (!dict.ContainsKey(item.stringID))
            {
                dict.Add(item.stringID, item);
                TableData.Add(item.stringID, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.stringID}");
            }
        }
    }

    public Data GetData(int key)
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

    public override void Set(List<string[]> data)
    {
        var dictionary = new Dictionary<int, Data>();
        var tableData = new Dictionary<int, DataTableData>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            dictionary.Add(datum.stringID, datum);
            tableData.Add(datum.stringID, datum);
        }
        dict = dictionary;
        TableData = tableData;
    }

    public override string GetCsvData()
    {
        return CreateCsv(dict.Values.ToList());
    }
}
