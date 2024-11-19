using System;
using System.IO;
using Newtonsoft.Json.Linq;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Load configuration from JSON file
            var config = LoadConfiguration("appsettings.json");

            // Ensure the "Sensor" section is not null
            var sensorConfig = config["Sensor"];
            if (sensorConfig == null)
            {
                throw new InvalidOperationException("Sensor configuration section is missing in appsettings.json");
            }

            // Read the sensor configuration values with null checks
            string name = sensorConfig["Name"]?.ToString();
            string location = sensorConfig["Location"]?.ToString();
            double minValue = sensorConfig["MinValue"] != null ? double.Parse(sensorConfig["MinValue"].ToString()) : 0;
            double maxValue = sensorConfig["MaxValue"] != null ? double.Parse(sensorConfig["MaxValue"].ToString()) : 0;
            double lowerThreshold = sensorConfig["LowerThreshold"] != null ? double.Parse(sensorConfig["LowerThreshold"].ToString()) : 0;
            double upperThreshold = sensorConfig["UpperThreshold"] != null ? double.Parse(sensorConfig["UpperThreshold"].ToString()) : 0;

            // Check if any of the required values are missing
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(location))
            {
                throw new InvalidOperationException("Sensor name or location is missing in appsettings.json");
            }

            // Initialize the sensor with thresholds
            var sensor = Sensor.InitialiseSensor(name, location, minValue, maxValue, lowerThreshold, upperThreshold);
            
            // Start the sensor
            sensor.StartSensor();

            Console.WriteLine("Press any key to stop the sensor...");
            Console.ReadKey();
            
            // Shutdown the sensor
            sensor.ShutdownSensor();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static JObject LoadConfiguration(string filePath)
    {
        // Check if configuration file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Configuration file '{filePath}' not found.");
        }

        // Read and parse the JSON configuration file
        var json = File.ReadAllText(filePath);
        return JObject.Parse(json);
    }
}
