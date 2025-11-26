using Backend.Services;
using Backend.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR
builder.Services.AddSignalR();

// Add MQTT Subscriber as a background service
builder.Services.AddHostedService<MqttSubscriberService>();

// Add CORS for frontend integration (must allow credentials for SignalR)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5148", "null", "file://")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANT: CORS must be before routing
app.UseCors();
app.UseRouting();
app.UseHttpsRedirection();

// Map endpoints
app.MapControllers();
app.MapHub<VehicleHub>("/vehicleHub");

app.Run();

