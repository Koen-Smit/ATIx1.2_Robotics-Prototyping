# ATIx1.2 Robotics Prototyping 2024-25 P2
(01-11-2024 / 19-01-2025)

## Project Overview
This repository consists of two main components:

1. **Robot Code**: Contains the logic and functionality to control and manage the robot.
2. **Blazor Web App**: A user interface for interacting with the robot remotely, built using Blazor.

---

## Folder Structure

### **Robot Code**
Located in the `Robot` folder, this part of the project is responsible for all robot-related logic. 

#### **Structure**:

- **Mqtt**
  - `ConnectionStrings.cs`: Handles the MQTT connection strings.
  - `MqttConnection.cs`: Implements MQTT connection logic.
  - `SetConnection.cs`: Defines connection configuration.
  - `Startup.cs`: Initializes the robot's components.
  - `Communication.cs`: Manages communication via MQTT.
  - `SimpleMqttClient.cs`: A lightweight MQTT client implementation.

- **Sensors**
  - `Button.cs`: Interacts with the robot's buttons.
  - `Display.cs`: Handles the robot's display functionality.
  - `Distance.cs`: Manages distance measurement sensors.
  - `Lux.cs`: Reads light intensity using the lux sensor.

- **Tasks**
  - `Alert.cs`: Manages alert systems.
  - `Battery.cs`: Tracks and manages battery status.
  - `Drive.cs`: Handles robot movement.
  - `ObstacleDetection.cs`: Detects obstacles and manages avoidance.
  - `Sensor.cs`: General sensor management.
  - `TaskType.cs` & `TaskTypeList.cs`: Define and manage robot tasks.
  - `PinReader.cs`: Handles GPIO pin reading.

- `FZHRobot.cs`: The main class managing the robot's functionality and task flow.

---

### **Blazor Web App**
Located in the `Robot-App` folder, this is the user interface for interacting with the robot.

#### **Structure**:

- **Components**
  - `Layout` folder: Contains layout-related Razor components.

- **Pages**
  - `Error.razor`: Displays error messages.
  - `Robot.razor`: The main interaction page for the robot.
  - `_Imports.razor`: Razor imports shared across components.

- **Models**
  - `Battery.cs`, `IBattery.cs`: Models and interfaces for battery management.
  - `Lux.cs`, `ILux.cs`: Models and interfaces for lux sensor data.
  - `ITask.cs`, `TaskList.cs`, `TaskType.cs`: Define and manage robot tasks.
  - `MqttConfig.cs`: MQTT configuration details.

- **Services**
  - `BatteryService.cs`: Service for managing battery data.
  - `LuxService.cs`: Service for managing lux sensor data.
  - `MqttProcessingService.cs`: Processes MQTT messages for various tasks.
  - `SimpleMqttClient.cs`: Implements MQTT client logic.
  - `StopService.cs`: Service for handling robot stop commands.
  - `TaskService.cs`: Manages robot tasks and updates.

- **wwwroot**: Contains static files such as CSS, JavaScript, and images.

- **App.razor**: Root Razor component for the application.
- **Routes.razor**: Defines routing for the application.
- `Program.cs`: Entry point for the Blazor app.

---

## Key Classes and Files

### **Robot Code**
- `FZHRobot.cs`: Central robot management class that coordinates all components.
- `MqttProcessingService.cs`: Handles MQTT messages and integrates with robot logic.

### **Blazor App**
- `Robot.razor`: UI for monitoring and controlling the robot.
- `MqttProcessingService.cs`: Processes MQTT messages on the Blazor app side to update the UI and send commands.

---

## Getting Started

### **Robot Code**
1. Configure `appsettings.json` with MQTT broker details, DB connection string and robot-specific settings.
2. Build and deploy the code to the robot's environment.

### **Blazor App**
1. Update `appsettings.Development.json` with the MQTT broker details and db connection string.
2. Run the Blazor app locally or deploy it to a hosting service.

---

## Key Features

1. **Real-time Robot Control**:
   - Start/stop the robot using MQTT commands.
   - Update robot tasks and send updates to the interface.

2. **Sensors and Alerts**:
   - Monitor sensor data like distance, light intensity (lux), and button presses.
   - Trigger alerts and display information on the robot's screen.

3. **Blazor Interface**:
   - Interact with the robot via an intuitive web interface.
   - View real-time sensor data and control tasks.

---


# Setting up .NET Libraries for Your Robot Project

To ensure the proper functionality of your robot project, you will need to install and manage several .NET libraries, such as `Avans.StatisticalRobot` and `HiveMQTT`. Follow the steps below to set up these libraries effectively within your Visual Studio Code environment.

---

## Prerequisites

- Install .NET SDK version 8.0 (or 9.0 if you'd like to experiment, though adjustments may be needed).
- Visual Studio Code should already be set up with the **C# Dev Kit** and optionally **IntelliCode for C# Dev Kit** extensions.
- set up the custom extension to manage the robot, its added in this project: `avans-statisticalrobot-1.0.0.vsix`

---

## Installing a NuGet Package

In Visual Studio Code, you can add NuGet packages directly to your project using the terminal:

1. **Open the Terminal**:  
   Use the shortcut `Ctrl + `` ` to open the terminal in Visual Studio Code.

2. **Add a NuGet Package**:  
   Use the following command to install the required NuGet package:  
   ```bash
   dotnet add package <package_name>
   ```

   Example: To install the `Avans.StatisticalRobot` library:  
   ```bash
   dotnet add package Avans.StatisticalRobot
   ```

   For `HiveMQTT` (a lightweight MQTT library for .NET):  
   ```bash
   dotnet add package HiveMQTT
   ```

3. **Restore Dependencies**:  
   Once you’ve added the package, ensure all dependencies are restored by running:  
   ```bash
   dotnet restore
   ```

---

## Managing and Updating Libraries

Over time, you may need to update the libraries in your project. For this, use the `dotnet-outdated` tool.

1. **Install the `dotnet-outdated` Tool**:  
   This tool checks for outdated packages and simplifies updates. Install it globally using the following command:  
   ```bash
   dotnet tool install --global dotnet-outdated-tool
   ```

2. **Check for Updates**:  
   In the terminal, run:  
   ```bash
   dotnet-outdated
   ```

   This will list all outdated packages in your project.

3. **Update Packages**:  
   To update all outdated packages:  
   ```bash
   dotnet-outdated -u
   ```

   Alternatively, if you need to update a specific package to a known version (e.g., a beta release), you can run:  
   ```bash
   dotnet add package <package_name> --version <version_number>
   ```

   Example: To update `Avans.StatisticalRobot` to version `1.1.6-beta`:  
   ```bash
   dotnet add package Avans.StatisticalRobot --version 1.1.6-beta
   ```

---

## About HiveMQTT and Avans.StatisticalRobot

- **HiveMQTT**:  
  This lightweight library allows your robot to communicate via MQTT, a messaging protocol ideal for IoT devices. You'll use this library to send and receive data between the robot and external systems.

- **Avans.StatisticalRobot**:  
  This custom library simplifies communication between the Raspberry Pi (via .NET) and the Romi32U4 control board. It also provides additional tools for handling sensors, motors, and other peripherals.  

  For installation, follow the steps above. You can also view the full version history and updates [here](https://www.nuget.org/packages/Avans.StatisticalRobot).

---

## Debugging and Tips for Development

- Always ensure that the robot and your laptop are connected to the same Wi-Fi network for seamless communication.

---
