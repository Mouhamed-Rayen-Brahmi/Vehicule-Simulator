using Backend.Models.entities;

namespace Backend.Models.Services
{
    public class VehiculeServices
    {
        public void CheckAndLogCoordinate(VehiculeCoordinate coordinate)
        {
            if (coordinate.Latitude == 0 && coordinate.Longitude == 0)
            {
                Console.WriteLine($"Vehicle coordinate: Not initialized");
                return;
            }

            Console.WriteLine($"Vehicle coordinate: Lat={coordinate.Latitude}, Lon={coordinate.Longitude}, Time={coordinate.Timestamp}");
        }

        public bool IsCoordinateValid(VehiculeCoordinate coordinate)
        {
            return coordinate.Latitude != 0 || coordinate.Longitude != 0;
        }
    }
}
