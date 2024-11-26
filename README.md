# Instruction on .Net Application

# Temperature Sensor System

## 1. Project Overview

### Project Description
This project simulates a temperature sensor system for a data center environment. It is designed to monitor temperature readings, validate data, detect anomalies, log readings, store them in a database, and alert users when certain thresholds are breached.  
The application is modular, enabling easy additions of features and enhancements. The development was divided into multiple sprints, each focusing on specific functionalities.

### Function Descriptions
- **SimulateData()**: Generates a random temperature reading with optional noise and faults.
- **ValidateData()**: Validates readings against the acceptable range (`MinValue` and `MaxValue`).
- **LogData()**: Logs readings to a file and the console.
- **StoreData()**: Stores readings in the SQLite database.
- **SmoothData()**: Calculates a smoothed value using a rolling average of recent readings.
- **DetectAnomaly()**: Flags readings that deviate significantly from recent averages.
- **CheckThreshold()**: Alerts if readings are above or below critical thresholds.
- **ShutdownSensor()**: Stops the sensor, clears data, and closes the database connection.

---

## 2. Setup Instructions

### Prerequisites
- **Programming Language**: C# (.NET Core)
- **Database**: SQLite
- **Libraries**:
  - `Newtonsoft.Json` (for JSON handling)
  - `System.Data.SQLite` (for database interactions)

### Installation Steps
1. Clone the repository or download the project files.
2. Install dependencies:
   ```bash
   dotnet new console
   dotnet add package Newtonsoft.Json
   dotnet add package System.Data.SQLite

   dotnet build 
   dotnet run

3. Project Development 


Sprint 1: Research into initial setup of application requirements and task breakdown

Objectives: 
Created a Kanban Board with a backlog of tasks for the  project
Researched into .net console applications to gain better understanding of app requirements

Sprint 2: Github and coding initial setup

Objective: 
Create the base .net application with core component files.
InitialiseSensor(Name, Location, MinValue, MaxValue) 

Usage Example: var sensor = Sensor.InitialiseSensor("DataCentreSensor", "Server Room", 22.0, 24.0, 22.10, 23.90); sensor.StartSensor();

Sprint 3: C# coding progress 

Objectives: Implement methods for data simulation, data validation, data logging and data history storage
Basic sensor simulation: SimulateData(): Generates random temperature values within a specified range.

Usage Example: var sensor = new Sensor("TestSensor", "Lab", 20.0, 30.0, 0, 0); var reading = sensor.SimulateData(); Console.WriteLine($"Simulated Temperature: {reading}°C");

Data validation: ValidateData(): Checks if the temperature reading is within the defined MinValue and MaxValue.

Usage Example: var isValid = sensor.ValidateData(25.5); Console.WriteLine(isValid ? "Valid Reading" : "Invalid Reading");
Invalid readings are flagged in the console terminal






Logging and history storage: LogData(): Logs readings to a text file and console. StoreData(): Saves readings to the SQLite database. Database setup in InitialiseDatabase().

Usage Example: sensor.LogData(25.5); sensor.StoreData(25.5);

Sprint 4: Majority of coded features complete and reviewed

Objectives: Implement methods for data smoothing, anomaly detection, and shutdown sensor. Review code from the previous sprint. 

Data Smoothing: SmoothData(): Calculates the average of the last 3 readings. And smooth out that reading using a rolling average

Usage Example: var smoothed = sensor.SmoothData(); Console.WriteLine($"Smoothed Reading: {smoothed}°C");

Data Anomaly Detection: DetectAnomaly(): Compares current readings against a rolling average of the last 5 readings. Identifying irregular spikes in temperature readings. 

Usage Example: bool isAnomaly = sensor.DetectAnomaly(35.0); Console.WriteLine(isAnomaly ? "Anomaly Detected!" : "Normal Reading");
Deviated readings will be flagged in console

Graceful Shutdown: ShutdownSensor(): Stops the sensor, clears memory, and closes the database connection. Ensuring the sensor can stop safely and release resources

Usage Example: sensor.ShutdownSensor();
Q key used to begin shutdown










Sprint 5: All required features implemented, began work on advanced features

Objectives: Attemp implementation of advanced feature threshold alerts. Review previous code 

Threshold Alerts: CheckThreshold(): Checks if readings are below or above critical limits. Alerts display in console

Usage Example: sensor.CheckThreshold(21.5);

Sprint 6: Completed all required and attempted advanced features



