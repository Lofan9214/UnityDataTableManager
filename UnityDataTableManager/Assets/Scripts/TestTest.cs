using MySqlConnector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTest : MonoBehaviour
{
    private string server = "146.56.191.180";
    private int port = 3306;
    private string database = "db_spaceminer";
    private string id = "spaceminer";
    private string pw = "space";
    private MySqlConnection connection;
    private const string connectFormat = "Server={0};Port={1};Database={2};Uid={3};Pwd={4};";

    private void Awake()
    {
        string connectionString = string.Format(connectFormat, server, port, database, id, pw);
        connection = new MySqlConnection(connectionString);
        connection.Open();
    }

    private void Start()
    {
        SelectStringTable();
    }

    private void OnDestroy()
    {
        connection.Close();
    }

    private void SelectStringTable()
    {
        string query = "SELECT * FROM stringtable";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log($"{reader.GetString(0)}, {reader.GetString(1)}");
        }
    }
}
