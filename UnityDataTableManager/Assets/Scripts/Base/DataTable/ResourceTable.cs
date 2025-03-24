using JetBrains.Annotations;
using System;
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
        public override int GetID()
        {
            return ID;
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
            if (!dict.ContainsKey(item.ID))
            {
                dict.Add(item.ID, item);
                TableData.Add(item.ID, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
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
            dictionary.Add(datum.ID, datum);
            tableData.Add(datum.ID, datum);
        }
        dict = dictionary;
        TableData = tableData;
    }

    public override string GetCsvData()
    {
        return CreateCsv(dict.Values.ToList());
    }
}
