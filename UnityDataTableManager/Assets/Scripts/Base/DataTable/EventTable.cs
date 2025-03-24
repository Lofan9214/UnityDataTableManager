using System;
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

    public override Type DataType =>typeof(Data);

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
