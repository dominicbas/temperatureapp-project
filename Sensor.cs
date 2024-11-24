using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.SQLite;
using System.Threading.Tasks;

public class Sensor
{
    public string Name { get; set; }
    public string Location { get; set; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
    public double LowerThreshold { get; set; }  // Added LowerThreshold
    public double UpperThreshold { get; set; }  // Added UpperThreshold
    private List<double> DataHistory { get; set; } = new List<double>();
    private bool IsRunning { get; set; }
    private SQLiteConnection DbConnection;

    public Sensor(string name, string location, double minValue, double maxValue, double lowerThreshold, double upperThreshold)
    {
        Name = name;
        Location = location;
        MinValue = minValue;
        MaxValue = maxValue;
        LowerThreshold = lowerThreshold;
        UpperThreshold = upperThreshold;
        InitialiseDatabase();
    }

    public static Sensor InitialiseSensor(string name, string location, double minValue, double maxValue, double lowerThreshold, double upperThreshold)
    {
        return new Sensor(name, location, minValue, maxValue, lowerThreshold, upperThreshold);
    }

    private void InitialiseDatabase()
    {
        DbConnection = new SQLiteConnection("Data Source=Sensor.db;Version=3;");
        DbConnection.Open();
        var cmd = DbConnection.CreateCommand();
        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS SensorData (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                            Temperature REAL,
                            Location TEXT)";
        cmd.ExecuteNonQuery();
    }

    public void StartSensor()
    {
        IsRunning = true;
        Console.WriteLine($"{Name} sensor started at {Location}. Press 'Q' to stop.");

        // Start a separate task to listen for user input (shutdown command)
        Task.Run(() =>
        {
            while (IsRunning)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    ShutdownSensor();
                }
            }
        });

        while (IsRunning)
        {
            var reading = SimulateData();
            if (ValidateData(reading))
            {
                LogData(reading);
                StoreData(reading);
                SmoothData();
                DetectAnomaly(reading);
                CheckThreshold(reading);  // Checking thresholds
            }
            System.Threading.Thread.Sleep(1000); // Simulate real-time data reading every second
        }
    }

    public double SimulateData()
    {
        var random = new Random();
        double noise = random.NextDouble() * 0.5; // Small noise
        double reading = MinValue + (MaxValue - MinValue) * random.NextDouble() + noise;

        // Simulate faults (irregular spikes or sensor failure)
        if (random.Next(0, 100) < 5) // 5% chance of anomaly
        {
            reading += random.Next(5, 10); // Irregular spike
        }
        Console.WriteLine($"Simulated Reading: {reading}");
        return reading;
    }

    public bool ValidateData(double sensorData)
    {
        bool isValid = sensorData >= MinValue && sensorData <= MaxValue;
        if (!isValid)
        {
            Console.WriteLine($"[{DateTime.Now}] Invalid data detected: {sensorData:F2}°C is outside the range [{MinValue}°C, {MaxValue}°C].");
        }
        return isValid;
    }

    public void LogData(double sensorData)
    {
        string logMessage = $"{DateTime.Now}: Temperature Reading - {sensorData}°C";
        Console.WriteLine($"[{DateTime.Now}] Sensor: {Name}, Location: {Location}, Temperature: {sensorData:F2}°C");
        File.AppendAllText("SensorLog.txt", logMessage + Environment.NewLine);
    }

    public void StoreData(double sensorData)
    {
        DataHistory.Add(sensorData);
        using var cmd = DbConnection.CreateCommand();
        cmd.CommandText = "INSERT INTO SensorData (Temperature, Location) VALUES (@temperature, @location)";
        cmd.Parameters.AddWithValue("@temperature", sensorData);
        cmd.Parameters.AddWithValue("@location", Location);
        cmd.ExecuteNonQuery();
    }

    public double SmoothData()
    {
        if (DataHistory.Count < 3) return DataHistory.Last(); // Not enough data to smooth
        double smoothedValue = DataHistory.Skip(Math.Max(0, DataHistory.Count() - 3)).Average();
        Console.WriteLine($"Smoothed Data: {smoothedValue}°C");
        return smoothedValue;
    }

    public bool DetectAnomaly(double sensorData)
    {
        if (DataHistory.Count < 5)
        {
            // Not enough data to perform anomaly detection
            return false;
        }

        double recentAverage = DataHistory.Skip(Math.Max(0, DataHistory.Count - 5)).Average();
        double threshold = 0.3; // You can adjust this threshold for sensitivity

        bool isAnomaly = Math.Abs(sensorData - recentAverage) > threshold;
        if (isAnomaly)
        {
            Console.WriteLine($"[{DateTime.Now}] Anomaly detected! Temperature: {sensorData:F2}°C deviates from recent average: {recentAverage:F2}°C");
        }

        return isAnomaly;
    }

    public void CheckThreshold(double sensorData)
    {
        // Define the significant threshold values
        double lowerAlertThreshold = 22.10;  // Lower threshold for alert
        double upperAlertThreshold = 23.90;  // Upper threshold for alert

        // Check if the sensor data is significantly lower or higher than the thresholds
        if (sensorData < lowerAlertThreshold)
        {
            Console.WriteLine($"ALERT: Temperature below {lowerAlertThreshold}°C! ({sensorData:F2}°C)");
        }
        else if (sensorData > upperAlertThreshold)
        {
            Console.WriteLine($"ALERT: Temperature above {upperAlertThreshold}°C! ({sensorData:F2}°C)");
        }
    }

public void ShutdownSensor()
{
    IsRunning = false;
    DataHistory.Clear();
    Console.WriteLine("Sensor is shutting down...");

    if (DbConnection?.State == System.Data.ConnectionState.Open)
    {
        using var cmd = DbConnection.CreateCommand();
        cmd.CommandText = "DELETE FROM SensorData";
        cmd.ExecuteNonQuery();
        Console.WriteLine("Sensor database cleared.");

        DbConnection.Close();
        Console.WriteLine("Database connection closed.");
    }

    Console.WriteLine("Sensor shutdown complete.");
}



}
