using Backend.DataTest;
using Backend.Models.entities;
using Backend.Hubs;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Protocol;
using System.Text.Json;

namespace Backend.Services
{
    public class MqttSubscriberService : BackgroundService
    {
        private readonly ILogger<MqttSubscriberService> _logger;
        private readonly IHubContext<VehicleHub> _hubContext;
        private readonly IMqttClient _mqttClient;
        private readonly MqttClientOptions _options;
        private const string Topic = "vehicle/coordinates";

        public MqttSubscriberService(ILogger<MqttSubscriberService> logger, IHubContext<VehicleHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
            var factory = new MqttClientFactory();
            _mqttClient = factory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .WithClientId("backend_subscriber_" + Guid.NewGuid())
                .WithCleanSession()
                .Build();

            // Set up message received handler
            _mqttClient.ApplicationMessageReceivedAsync += OnMessageReceived;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (!_mqttClient.IsConnected)
                    {
                        _logger.LogInformation("Connecting to MQTT Broker...");
                        var result = await _mqttClient.ConnectAsync(_options, stoppingToken);

                        if (result.ResultCode == MqttClientConnectResultCode.Success)
                        {
                            _logger.LogInformation("Connected to MQTT Broker successfully!");

                            // Subscribe to vehicle coordinates topic
                            var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                                .WithTopicFilter(f => f.WithTopic(Topic).WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce))
                                .Build();

                            await _mqttClient.SubscribeAsync(subscribeOptions, stoppingToken);
                            _logger.LogInformation($"Subscribed to topic: {Topic}");
                        }
                        else
                        {
                            _logger.LogError($"Failed to connect to MQTT Broker: {result.ResultCode}");
                        }
                    }

                    await Task.Delay(5000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in MQTT connection: {ex.Message}");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        private Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var payload = e.ApplicationMessage.ConvertPayloadToString();
                _logger.LogInformation($"Received message on topic {e.ApplicationMessage.Topic}: {payload}");

                // Deserialize coordinate data
                var coordinate = JsonSerializer.Deserialize<VehiculeCoordinate>(payload);

                if (coordinate != null)
                {
                    // Update the first vehicle's position (for demo purposes)
                    // In a real app, you'd match by vehicle ID
                    var vehicle = VehiculeTestData._vehicules.FirstOrDefault();
                    if (vehicle != null)
                    {
                        vehicle.Coordinate = coordinate;
                        _logger.LogInformation($"Updated vehicle {vehicle.Immatricule}: Lat={coordinate.Latitude}, Lon={coordinate.Longitude}");
                        
                        // Push update to all connected clients via SignalR
                        _ = _hubContext.Clients.All.SendAsync("ReceiveVehicleUpdate", new
                        {
                            id = vehicle.Id,
                            immatricule = vehicle.Immatricule,
                            latitude = coordinate.Latitude,
                            longitude = coordinate.Longitude,
                            timestamp = coordinate.Timestamp
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_mqttClient.IsConnected)
            {
                await _mqttClient.DisconnectAsync(cancellationToken: cancellationToken);
            }
            _mqttClient?.Dispose();
            await base.StopAsync(cancellationToken);
        }
    }
}
