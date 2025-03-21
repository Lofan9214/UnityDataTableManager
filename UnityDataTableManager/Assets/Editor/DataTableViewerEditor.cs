using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataTableViewer))]
public class DataTableViewerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
    
    public void SaveTable(string tablename, DataTableView view)
    {
        string tableName = $"Assets/Resources/Tables/{tablename}.csv";
        var table = DataTableManager.Get<DataTable>(tablename);
        table.Set(view.GetData());
        table.Save(tableName);
    }
}
