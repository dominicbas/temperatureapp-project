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

    // Method to validate the temperature data
    public bool ValidateData(double temperature)
    {
        // Check if the temperature is within the specified range
        return temperature >= MinValue && temperature <= MaxValue;
    }

    // Method to start the sensor and simulate data
    public void StartSensor()
    {
        Console.WriteLine($"Starting sensor: {Name} located at {Location}");
        
        Random rand = new Random();
        double temperature = rand.NextDouble() * (MaxValue - MinValue) + MinValue;

        // Validate the generated temperature
        if (ValidateData(temperature))
        {
            Console.WriteLine($"Valid temperature reading: {temperature}°C");
        }
        else
        {
            Console.WriteLine($"Invalid temperature reading: {temperature}°C");
        }
    }
}
