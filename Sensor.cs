using System;

public class Sensor
{
    public string Name { get; set; }
    public string Location { get; set; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }

    public Sensor(string name, string location, double minValue, double maxValue)
    {
        Name = name;
        Location = location;
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public static Sensor InitialiseSensor(string name, string location, double minValue, double maxValue)
    {
        return new Sensor(name, location, minValue, maxValue);
    }

    public void StartSensor()
    {
        Console.WriteLine($"{Name} sensor initialized at {Location}.");
    }
}
