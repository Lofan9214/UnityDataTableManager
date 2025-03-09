using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataTableViewer : MonoBehaviour
{
    public TMP_Dropdown tableDropdown;
    public DataTableView dataTableViewPrefab;
    public Transform tableView;

    private Dictionary<string, DataTableView> views = new Dictionary<string, DataTableView>();

    private int currentIndex;

    private void Awake()
    {
        SetStringTable();
        SetEventTable();

        tableDropdown.value = 0;
        tableDropdown.RefreshShownValue();
        OnDropDownChanged(0);
    }

    private void SetStringTable()
    {
        var tableType = typeof(StringTable);
        var dataType = typeof(StringTable.Data);
        var properties = typeof(StringTable.Data).GetProperties();

        AddDropDownOption(tableType.Name);

        var stringtableview = Instantiate(dataTableViewPrefab, tableView);
        var stringTableData = DataTableManager.StringTable.GetAllData();

        views.Add(tableType.Name, stringtableview);
        stringtableview.SetColumns(dataType.GetProperties().Select(p => p.Name).ToArray());

        foreach (var data in stringTableData)
        {
            stringtableview.AddRow(new string[] { data.Key.ToString(), data.Value });
        }
    }

    private void SetEventTable()
    {
        var tableView = Instantiate(dataTableViewPrefab, this.tableView);

        var eventTableData = DataTableManager.EventTable.GetAllData();

        var tableType = typeof(EventTable);
        var dataType = typeof(EventTable.Data);
        var properties = typeof(EventTable.Data).GetProperties();

        AddDropDownOption(tableType.Name);

        views.Add(tableType.Name, tableView);
        tableView.SetColumns(properties.Select(p => p.Name).ToArray());

        foreach (var data in eventTableData)
        {
            string[] values = new string[properties.Length];

            for (int i = 0; i < properties.Length; ++i)
            {
                values[i] = properties[i].GetValue(data.Value).ToString();
            }

            tableView.AddRow(values);
        }
    }

    private void AddDropDownOption(string name)
    {
        TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
        optionData.text = name;
        tableDropdown.options.Add(optionData);
    }

    public void OnDropDownChanged(int index)
    {
        currentIndex = index;
        foreach (var view in views)
        {
            view.Value.gameObject.SetActive(view.Key == tableDropdown.options[index].text);
        }
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
