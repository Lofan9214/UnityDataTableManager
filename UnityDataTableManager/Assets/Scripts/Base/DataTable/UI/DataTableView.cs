using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataTableView : MonoBehaviour
{
    [SerializeField]
    private DataRow dataRowPrefab;

    [SerializeField]
    private TextMeshProUGUI columnCellPrefab;

    public ScrollRect columns;
    public ScrollRect cells;

    [SerializeField]
    private Transform columnContent;
    [SerializeField]
    private Transform cellsContent;

    public int columnCount;

    public List<DataRow> rows = new List<DataRow>();

    public void OnColumnMoved(Vector2 rot)
    {
        cells.horizontalNormalizedPosition = rot.x;
    }

    public void OnCellMoved(Vector2 rot)
    {
        columns.horizontalNormalizedPosition = rot.x;
    }

    public void SetColumns(string[] columns)
    {
        columnCount = columns.Length;
        for (int i = 0; i < columns.Length; ++i)
        {
            var text = Instantiate(columnCellPrefab, columnContent);
            text.text = columns[i];
        }
    }

    public void AddRow(string[] rowdata)
    {
        var row = Instantiate(dataRowPrefab, cellsContent);
        row.SetCells(rowdata);
    }

    public void AddRows(string[][] rowsData)
    {
        for (int i = 0; i < rowsData.GetLength(0); ++i)
        {
            AddRow(rowsData[i]);
        }
    }
}
