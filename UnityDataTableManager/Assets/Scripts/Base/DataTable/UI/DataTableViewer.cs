using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataTableViewer : MonoBehaviour
{
    public TMP_Dropdown tableDropdown;
    public DataTableView dataTableViewPrefab;
    public Transform tableView;

    private Dictionary<string, DataTableView> views = new Dictionary<string, DataTableView>();

    private DataTableView currentView;
    private int currentIndex;

    private void Awake()
    {
        foreach (var table in DataTableManager.Tables)
        {
            SetTable(table);
        }

        tableDropdown.value = 0;
        tableDropdown.RefreshShownValue();
        OnDropDownChanged(0);
    }

    private void SetTable(KeyValuePair<string, DataTable> table)
    {
        var dict = table.Value.TableData;
        var dataType = dict.Values.FirstOrDefault().GetType();

        var properties = dataType.GetProperties();

        AddDropDownOption(table.Key);
        var tableView = Instantiate(dataTableViewPrefab, this.tableView);
        views.Add(table.Key, tableView);
        tableView.SetColumns(properties.Select(p => p.Name).ToArray());

        foreach (var data in dict)
        {
            string[] values = new string[properties.Length];

            for (int i = 0; i < properties.Length; ++i)
            {
                values[i] = properties[i].GetValue(data.Value).ToString();
            }

            tableView.AddRow(values);
        }
        tableView.gameObject.SetActive(false);
    }

    public void SaveTable()
    {
        string tableName = tableDropdown.options[tableDropdown.value].text;
        var table = DataTableManager.Get<DataTable>(tableDropdown.options[tableDropdown.value].text);
        table.Set(currentView.GetData());
        table.Save(tableName);
    }

    private void AddDropDownOption(string name)
    {
        TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
        optionData.text = name;
        tableDropdown.options.Add(optionData);
    }

    public void OnDropDownChanged(int index)
    {
        if (currentView != null)
        {
            currentView.gameObject.SetActive(false);
        }
        currentIndex = index;
        currentView = views[tableDropdown.options[index].text];
        currentView.gameObject.SetActive(true);
    }

    public void ResetTable()
    {
        tableDropdown.ClearOptions();

        foreach (var view in views)
        {
            Destroy(view.Value.gameObject);
        }

        views.Clear();

        Awake();
    }

    public void AddEmptyRow()
    {
        var table = views.ElementAt(currentIndex);
        table.Value.AddRow(new string[table.Value.columnCount]);
    }
}
