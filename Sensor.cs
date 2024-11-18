using System;
using System.Data.SQLite;

public class Sensor
{
    public string Name { get; set; }
    public string Location { get; set; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
    private SQLiteConnection? DbConnection;  // Marked as nullable

    public Sensor(string name, string location, double minValue, double maxValue)
    {
        Name = name;
        Location = location;
        MinValue = minValue;
        MaxValue = maxValue;
        InitialiseDatabase();  // Initialize the database when the sensor is created
    }

    private void InitialiseDatabase()
    {
        // Initialize the SQLite database connection
        DbConnection = new SQLiteConnection("Data Source=SensorData.db;Version=3;");
        DbConnection.Open();

        // Create the SensorData table if it doesn't exist
        var cmd = DbConnection.CreateCommand();
        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS SensorData (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                            Temperature REAL,
                            Location TEXT)";
        cmd.ExecuteNonQuery();

        Console.WriteLine("Database initialized and table created (if not existing).");
    }
}
