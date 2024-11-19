using System;
using System.Data.SQLite;

public class Sensor
{
    public string Name { get; set; }
    public string Location { get; set; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
    private SQLiteConnection? DbConnection;  // Nullable connection

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

    // Validate if the temperature is within acceptable range
    public bool ValidateData(double temperature)
    {
        return temperature >= MinValue && temperature <= MaxValue;
    }

    // Start the sensor and generate temperature readings
    public void StartSensor()
    {
        Console.WriteLine($"Starting sensor: {Name} located at {Location}");
        Random rand = new Random();
        double temperature = rand.NextDouble() * (MaxValue - MinValue) + MinValue;

        if (ValidateData(temperature))
        {
            Console.WriteLine($"Valid temperature reading: {temperature}°C");
            StoreData(temperature);  // Store valid data
        }
        else
        {
            Console.WriteLine($"Invalid temperature reading: {temperature}°C");
        }
    }

    // Store the valid temperature data in the database
    private void StoreData(double temperature)
    {
        if (DbConnection == null)
        {
            Console.WriteLine("Database connection is not initialized.");
            return;
        }

        var cmd = DbConnection.CreateCommand();
        cmd.CommandText = @"INSERT INTO SensorData (Temperature, Location) 
                            VALUES (@temperature, @location)";
        cmd.Parameters.AddWithValue("@temperature", temperature);
        cmd.Parameters.AddWithValue("@location", Location);

        int rowsAffected = cmd.ExecuteNonQuery();
        Console.WriteLine($"Data stored successfully. Rows affected: {rowsAffected}");
        DataLog(temperature);  // Log the stored data
    }

    // Log data storage information to a text file
    private void DataLog(double temperature)
    {
    string logMessage = $"{DateTime.Now}: Temperature Reading - {temperature}°C at {Location}";
    // Appending the log message to the SensorLog.txt file
    File.AppendAllText("SensorLog.txt", logMessage + Environment.NewLine);
    Console.WriteLine($"Logged data: {logMessage}");
    }

}
