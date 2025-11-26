using MQTTnet;
using MQTTnet.Protocol;
using System.Text.Json;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using VehiculeSimulator.VehiculeTestData;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MQTT service
builder.Services.AddSingleton<MqttService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// REST endpoint: publish one message
app.MapPost("/vehicle/publish", async (MqttService mqtt) =>
{
    var coords = mqtt.GenerateRandomCoordinates();
    await mqtt.PublishCoordinates(coords);
    return Results.Ok(new { message = "Coordinates published", data = coords });
});

// REST endpoint: start continuous publishing
app.MapPost("/vehicle/start", async (MqttService mqtt) =>
{
    await mqtt.StartContinuousPublishing();
    return Results.Ok(new { message = "Continuous publishing started" });
});

// REST endpoint: stop publishing
app.MapPost("/vehicle/stop", (MqttService mqtt) =>
{
    mqtt.StopContinuousPublishing();
    return Results.Ok(new { message = "Continuous publishing stopped" });
});

app.Run();

public class MqttService
{
    private readonly IMqttClient _client;
    private readonly MqttClientOptions _options;

    private CancellationTokenSource? _cts;

    private const string Topic = "vehicle/coordinates";

    // Random coordinate bounds
    private const double LatMin = 34.0;
    private const double LatMax = 34.1;
    private const double LonMin = -118.3;
    private const double LonMax = -118.2;

    public MqttService()
    {
        var factory = new MqttClientFactory();
        _client = factory.CreateMqttClient();

        // MQTTnet v5: Correct options builder pattern
        _options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883)
            .WithClientId("vehicle_simulator_" + Guid.NewGuid())
            .WithCleanSession()
            .Build();

        ConnectAsync().Wait();
    }


    private async Task ConnectAsync()
    {
        if (!_client.IsConnected)
        {
            var result = await _client.ConnectAsync(_options);

            if (result.ResultCode != MqttClientConnectResultCode.Success)
            {
                Console.WriteLine($"MQTT connection failed: {result.ResultCode}");
            }
            else
            {
                Console.WriteLine("Connected to MQTT Broker");
            }
        }
    }

    public VehiculeCordinate GenerateRandomCoordinates()
    {
        var random = new Random();
        return new VehiculeCordinate
        {
            Latitude = Math.Round(random.NextDouble() * (LatMax - LatMin) + LatMin, 6),
            Longitude = Math.Round(random.NextDouble() * (LonMax - LonMin) + LonMin, 6),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };
    }

    public async Task PublishCoordinates(VehiculeCordinate coords)
    {
        await ConnectAsync();

        var json = JsonSerializer.Serialize(coords);

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(Topic)
            .WithPayload(json)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        await _client.PublishAsync(message);

        Console.WriteLine($"Published: Lat={coords.Latitude}, Lon={coords.Longitude}");
    }

    public async Task StartContinuousPublishing()
    {
        if (_cts != null) return;

        _cts = new CancellationTokenSource();

        _ = Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var coords = GenerateRandomCoordinates();
                    await PublishCoordinates(coords);
                    await Task.Delay(1000, _cts.Token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        });
    }

    public void StopContinuousPublishing()
    {
        _cts?.Cancel();
        _cts = null;
    }
}
