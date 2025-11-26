# 🚗 Vehicle Simulator with MQTT

A real-time vehicle tracking simulator built with .NET 8.0 and MQTT messaging protocol. This project demonstrates IoT-style communication between a publisher (simulator) and subscriber (backend) using the Mosquitto MQTT broker.

## 📋 What It Does

- **Simulates vehicle GPS tracking** - Generates random coordinates (latitude, longitude, timestamp)
- **Real-time MQTT messaging** - Publisher sends coordinates to MQTT broker
- **Backend subscription** - Subscriber receives and updates vehicle positions
- **REST API** - Control publishing and query vehicle data via HTTP endpoints
- **Swagger UI** - Interactive API documentation for easy testing

## 🏗️ Architecture

```
VehiculeSimulator (Publisher)  →  Mosquitto MQTT Broker  →  Backend (Subscriber)
   Port 5148                         Port 1883                  Port 5249
   Publishes coordinates              Routes messages            Receives updates
```

## 🚀 Quick Start

### 1. Install Mosquitto MQTT Broker
Download from: https://mosquitto.org/download/
```powershell
# Install to: C:\Program Files\mosquitto
```

### 2. Start Mosquitto
```powershell
cd "C:\Program Files\mosquitto"
.\mosquitto.exe -c mosquitto.conf -v
```

### 3. Run Backend (Terminal 1)
```powershell
cd Backend
dotnet run
```

### 4. Run Simulator (Terminal 2)
```powershell
cd VehiculeSimulator
dotnet run
```

### 5. Test the System
```powershell
# Start publishing coordinates
Invoke-RestMethod -Uri "http://localhost:5148/vehicle/start" -Method Post

# View updated coordinates
Invoke-RestMethod -Uri "http://localhost:5249/api/vehicule/coordinates" -Method Get
```


## 🌐 Access Points

- **Backend Swagger:** http://localhost:5249/swagger
- **Simulator Swagger:** http://localhost:5148/swagger


## 🎯 API Endpoints

### VehiculeSimulator (Port 5148)
- `POST /vehicle/publish` - Publish one coordinate
- `POST /vehicle/start` - Start continuous publishing (every 1s)
- `POST /vehicle/stop` - Stop publishing

### Backend (Port 5249)
- `GET /api/vehicule/all` - Get all vehicles with details
- `GET /api/vehicule/{id}` - Get specific vehicle by ID
- `GET /api/vehicule/coordinates` - Get all coordinates

## 🛠️ Built With

- **.NET 8.0** - Application framework
- **ASP.NET Core** - Web API
- **MQTTnet 5.0** - MQTT client library
- **Mosquitto** - MQTT broker
- **Swashbuckle** - OpenAPI/Swagger documentation

## 📦 Project Structure

```
VehiculeSimulator/
├── Backend/                    # Subscriber application
│   ├── Controllers/            # REST API controllers
│   ├── Services/               # MQTT subscriber service
│   ├── Models/                 # Data models
│   └── Program.cs              # App configuration
├── VehiculeSimulator/          # Publisher application
│   ├── Program.cs              # MQTT client & API
│   └── VehiculeTestData/       # Coordinate models
├── mosquitto.conf              # MQTT broker config
├── start.ps1                   # Quick start script
└── test.ps1                    # Test script
```

### Ports
Edit in `Properties/launchSettings.json` for each project:
```json
{
  "applicationUrl": "http://localhost:5249;https://localhost:7249"
}
```

### MQTT Broker
Edit `mosquitto.conf`:
```conf
listener 1883
allow_anonymous true
```

## 📝 How It Works

1. **VehiculeSimulator** generates random GPS coordinates
2. Coordinates are serialized to JSON: `{"latitude": 34.05, "longitude": -118.24, "timestamp": 1700000000}`
3. Message is published to MQTT broker on topic `vehicle/coordinates`
4. **Backend** subscriber receives the message
5. Backend updates vehicle position in memory
6. REST API returns updated vehicle data

## 🔐 Security Notice






## 📄 License

This project is for educational purposes.

## 🙏 Acknowledgments

- **MQTTnet** - https://github.com/dotnet/MQTTnet
- **Mosquitto** - https://mosquitto.org/
- **MQTT Protocol** - https://mqtt.org/

---

**Ready to simulate? Follow the Quick Start section above!** 🚗💨
