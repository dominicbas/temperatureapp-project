using System;

class Program
{
    static void Main(string[] args)
    {
        // Initialize a basic sensor
        var sensor = Sensor.InitialiseSensor("BaseSensor", "Default Location", 0, 100);

        // Start the sensor
        sensor.StartSensor();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
