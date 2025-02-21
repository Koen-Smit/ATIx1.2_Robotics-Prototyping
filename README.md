# ATIx1.2 Robotics Prototyping 2024-25 P2
- (01-11-2024 / 19-01-2025)
- Eindpunt(gemiddelde): **8**
- **Stack:** (Blazor, C#, HTML, CSS, Bootstrap, SQL, MQTT)
- Robot door school geleverd, bevat onder andere: `Raspberry Pi` en de `Romi32U4 controller`

**Belangrijk**: Applicatie compleet gemaakt rondom de robot en mqtt, alleen de robot moest ingeleverd worden bij school, dus daardoor helaas dit project moeten afsluiten. Wel is alles zo goed mogelijk gedocumenteerd en heb ik bijna alles afgekregen wat ik met die project wou bereiken.

## Project Overview
This repository consists of two main components:

1. **Robot Code**: Contains the logic and functionality to control and manage the robot.
2. **Blazor Web App**: A user interface for interacting with the robot remotely, built using Blazor.

---

## Folder Structure

### **Robot Code**
De robotcode bevindt zich in `/Robot-Code` en bevat alle logica voor de werking van de robot.

#### **Belangrijke bestanden:**:

- **Mqtt**
  - `ConnectionStrings.cs`: Beheert de MQTT-verbindingen.
  - `MqttConnection.cs`: Implementeert MQTT-logica.
  - `SetConnection.cs`: Definieert verbindingsinstellingen.
  - `Startup.cs`: Initialiseert de robotcomponenten.
  - `Communication.cs`: Beheert communicatie via MQTT.
  - `SimpleMqttClient.cs`: Een lichte MQTT-clientimplementatie meegekregen door school(kleine aanpassingen aan gedaan).

- **Sensors**
  - `Button.cs`: Interactie met de knoppen van de robot.
  - `Display.cs`: Beheer van het robot-lcd-display.
  - `Distance.cs`: Meet de afstand met een ultrasonic-sensor.
  - `Lux.cs`: Leest lichtintensiteit met de luxsensor.

- **Tasks**
  - `Alert.cs`: Beheert alerts.
  - `Battery.cs`: Houdt de batterijstatus bij.
  - `Drive.cs`: Regelt de beweging van de robot.
  - `ObstacleDetection.cs`: Detecteert obstakels en ontwijkt deze.
  - `Sensor.cs`: Algemene sensorbeheerfunctionaliteit.
  - `TaskType.cs` & `TaskTypeList.cs`: Definieert en beheert robottaken.
  - `PinReader.cs`: Leest GPIO-pinnen uit `/appsettings.json` zie: `/appsettings.development.json` voor template.

- `FZHRobot.cs`: De hoofdklasse die de robot bestuurt.

---

### **Blazor Web App**
De Blazor-app bevindt zich in `/Robot-App` en biedt een interface voor interactie met de robot.
#### **Belangrijke bestanden:**:

- **Pages**
  - `Robot.razor`: De hoofdinterface voor interactie met de robot.

- **Backend**
  - `Battery.cs`, `IBattery.cs`, `BatteryService.cs`: Beheer van batterijgegevens.
  - `Lux.cs`, `ILux.cs`, `LuxService.cs`: Beheer van luxsensorgegevens.
  - `ITask.cs`, `TaskList.cs`, `TaskType.cs`, `TaskService.cs`: Takenbeheer.
  - `MqttConfig.cs`, `MqttProcessingService.cs`, `SimpleMqttClient.cs`: MQTT-configuratie.
  - `StopService.cs`: Beheer noodstop-knop.

---

## Functionaliteiten

1. **Real-time Robotbesturing**:
   - Start/stop de robot via MQTT-opdrachten(aangegeven door "in behandeling"). De status van of de robot rijdt of niet is hier ook te zien.
   ![Img](Assets/Readme_addons/Robot_status.PNG)
   ![Img](Assets/Readme_addons/In_behandeling.gif)
   - Stuur en beheer robottaken via de webinterface.

2. **Sensoren en Waarschuwingen**:
   - Monitor afstanden, lichtintensiteit en knoppenstatus.
   ![Demo](Assets/Readme_addons/Rondrij_mode.gif)
   - Activeer waarschuwingen en stuur meldingen.
   ![Demo](Assets/Readme_addons/Stilstaan_mode_taak.gif)

3. **Webinterface**:
   - Bekijk real-time gegevens van de robot.
![Img](Assets/Readme_addons/Webpagina.PNG)
   - Pas taken aan en stuur commando's.
![Img](Assets/Readme_addons/Taken_scherm.PNG)

---

## Installatie en Setup

## Vereisten
- .NET SDK 8.0 of 9.0
- Visual Studio Code met C# Dev Kit & IntelliCode
- Installatie van een aangepaste extensie: `Assets/Setup_files/avans-statisticalrobot-1.0.0.vsix`


### **Robot Code**
1. Configure `appsettings.json` with MQTT broker details, DB connection string and robot-specific settings.
2. Build and deploy the code to the robot's environment.

### **Blazor App**
1. Update `appsettings.Development.json` with the MQTT broker details and db connection string.
2. Run the Blazor app locally or deploy it to a hosting service.


## Installing a NuGet Package

1. **Open terminal:**:  
   Open de terminal in VS Code (`Ctrl + `` `).

2. **Voeg NuGet-package toe**:  
   ```bash
   dotnet add package <package_name>
   ```

   For `HiveMQTT` (a lightweight MQTT library for .NET):  
   ```bash
   dotnet add package HiveMQTT
   ```

3. **Herstel dependencies:**:  
   ```bash
   dotnet restore
   ```

---

##  Over HiveMQTT & Avans.StatisticalRobot

- **HiveMQTT**:  
  Een lichtgewicht MQTT-library voor communicatie tussen de robot en externe systemen.

- **Avans.StatisticalRobot**:  
  Een aangepaste library die communicatie tussen de Raspberry Pi en de Romi32U4 controller optimaliseert. Voor meer info: [Klik hier](https://www.nuget.org/packages/Avans.StatisticalRobot).

---

## Debugging & Tips

- Zorg ervoor dat je laptop en de robot zich op hetzelfde Wi-Fi-netwerk bevinden.
- Gebruik `dotnet run` om de applicatie lokaal te testen.

---
