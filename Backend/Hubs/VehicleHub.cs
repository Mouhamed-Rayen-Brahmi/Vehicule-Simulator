using Microsoft.AspNetCore.SignalR;

namespace Backend.Hubs
{
    public class VehicleHub : Hub
    {
        public async Task SendVehicleUpdate(object vehicleData)
        {
            await Clients.All.SendAsync("ReceiveVehicleUpdate", vehicleData);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
        }
    }
}
