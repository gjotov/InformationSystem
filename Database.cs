using Microsoft.Data.Sqlite;
using System.Data;
using System.IO;

public class Database
{
    private string connectionString;

    public Database(string dbPath)
    {
        connectionString = $"Data Source={dbPath};Version=3;New=False;Compress=True;";
    }
    public SqliteConnection GetConnection()
    {
        return new SqliteConnection(connectionString);
    }

    public DataTable GetTopicContent(string topicName)
    {
        using (var conn = GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT name FROM ThermalMotion WHERE name = @topicName";
                cmd.Parameters.AddWithValue("@topicName", topicName);

                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }
    }
}
