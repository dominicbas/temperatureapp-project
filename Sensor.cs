using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

public class Sensor
{
    public string Name { get; set; }
    public string Location { get; set; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
    private SQLiteConnection? DbConnection;
    private List<double> DataHistory { get; set; } = new List<double>();

    public Sensor(string name, string location, double minValue, double maxValue)
    {
        Name = name;
        Location = location;
        MinValue = minValue;
        MaxValue = maxValue;
        InitialiseDatabase();
    }

    private void InitialiseDatabase()
    {
        DbConnection = new SQLiteConnection("Data Source=SensorData.db;Version=3;");
        DbConnection.Open();

        var cmd = DbConnection.CreateCommand();
        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS SensorData (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                            Temperature REAL,
                            Location TEXT)";
        cmd.ExecuteNonQuery();

        Console.WriteLine("Database initialized and table created (if not existing).");
    }

    public bool ValidateData(double temperature)
    {
        return temperature >= MinValue && temperature <= MaxValue;
    }

    public void StartSensor()
    {
        Console.WriteLine($"Starting sensor: {Name} located at {Location}");

        while (true) // Simulating continuous readings
        {
            double temperature = SimulateData();
            if (ValidateData(temperature))
            {
                Console.WriteLine($"Valid temperature reading: {temperature}°C");
                StoreData(temperature);
                SmoothData();
                DetectAnomaly(temperature);
            }
            else
            {
                Console.WriteLine($"Invalid temperature reading: {temperature}°C");
            }

            System.Threading.Thread.Sleep(1000); // Delay to simulate real-time data
        }
    }

    private double SimulateData()
    {
        var rand = new Random();
        return rand.NextDouble() * (MaxValue - MinValue) + MinValue;
    }

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
        DataLog(temperature);
        DataHistory.Add(temperature);
    }

    private void DataLog(double temperature)
    {
        string logMessage = $"{DateTime.Now}: Temperature Reading - {temperature}°C at {Location}";
        File.AppendAllText("SensorLog.txt", logMessage + Environment.NewLine);
        Console.WriteLine($"Logged data: {logMessage}");
    }

    public double SmoothData()
    {
        if (DataHistory.Count < 3) return DataHistory.Last();
        double smoothedValue = DataHistory.Skip(Math.Max(0, DataHistory.Count - 3)).Average();
        Console.WriteLine($"Smoothed Data: {smoothedValue}°C");
        return smoothedValue;
    }

    public bool DetectAnomaly(double sensorData)
    {
        if (DataHistory.Count < 5)
        {
            return false; // Not enough data for anomaly detection
        }

        double recentAverage = DataHistory.Skip(Math.Max(0, DataHistory.Count - 5)).Average();
        double threshold = 0.3;

        bool isAnomaly = Math.Abs(sensorData - recentAverage) > threshold;
        if (isAnomaly)
        {
            Console.WriteLine($"Anomaly detected! {sensorData:F2}°C deviates from recent average: {recentAverage:F2}°C");
        }

        return isAnomaly;
    }
}
