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

**OR use the automated scripts:**
```powershell
.\start.ps1   # Starts all components
.\test.ps1    # Tests the system
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

## 🔧 Configuration

### Coordinate Bounds (Los Angeles Area)
Edit in `VehiculeSimulator/Program.cs`:
```csharp
private const double LatMin = 34.0;
private const double LatMax = 34.1;
private const double LonMin = -118.3;
private const double LonMax = -118.2;
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

## 🐛 Troubleshooting

| Problem | Solution |
|---------|----------|
| Cannot connect to MQTT broker | Ensure Mosquitto is running on port 1883 |
| Port already in use | Change port in `launchSettings.json` |
| No coordinates updating | Verify all 3 components are running |
| Build errors | Run `dotnet restore` then `dotnet build` |

For detailed troubleshooting, see [SETUP_GUIDE.md](SETUP_GUIDE.md)

## 📝 How It Works

1. **VehiculeSimulator** generates random GPS coordinates
2. Coordinates are serialized to JSON: `{"latitude": 34.05, "longitude": -118.24, "timestamp": 1700000000}`
3. Message is published to MQTT broker on topic `vehicle/coordinates`
4. **Backend** subscriber receives the message
5. Backend updates vehicle position in memory
6. REST API returns updated vehicle data

## 🔐 Security Notice

⚠️ **This configuration is for DEVELOPMENT ONLY**

For production:
- Enable MQTT authentication
- Use TLS/SSL encryption
- Add API authentication (JWT)
- Implement proper error handling
- Add rate limiting

## 🎯 Future Enhancements

- [ ] Multi-vehicle support with vehicle IDs
- [ ] Database persistence (SQL Server/PostgreSQL)
- [ ] Web UI with real-time map (React/Blazor)
- [ ] SignalR for real-time web updates
- [ ] Route history and replay
- [ ] Geofencing and alerts
- [ ] Speed and distance calculations
- [ ] Docker containerization
- [ ] Azure/AWS deployment

## 📖 Additional Documentation

- **[SETUP_GUIDE.md](SETUP_GUIDE.md)** - Complete setup instructions with troubleshooting
- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Detailed system architecture and diagrams
- **[QUICK_REFERENCE.md](QUICK_REFERENCE.md)** - Command cheat sheet

## 🤝 Contributing

Feel free to:
- Report bugs
- Suggest features
- Submit pull requests
- Improve documentation

## 📄 License

This project is for educational and demonstration purposes.

## 🙏 Acknowledgments

- **MQTTnet** - https://github.com/dotnet/MQTTnet
- **Mosquitto** - https://mosquitto.org/
- **MQTT Protocol** - https://mqtt.org/

---

**Ready to simulate? Follow the Quick Start section above!** 🚗💨
