using Backend.Models.entities;

namespace Backend.DataTest
{
    public class VehiculeTestData
    {
        public static readonly List<Vehicule> _vehicules = new List<Vehicule>
            {
                new Vehicule
                {
                    Id = 1,
                    Immatricule = "ABC-123",
                    Constructeur = "Toyota",
                    Models = "Corolla",
                    Coordinate = new VehiculeCoordinate { Latitude = 34.05, Longitude = -118.25, Timestamp = 0 }
                },
                new Vehicule
                {
                    Id = 2,
                    Immatricule = "XYZ-789",
                    Constructeur = "Honda",
                    Models = "Civic",
                    Coordinate = new VehiculeCoordinate { Latitude = 34.06, Longitude = -118.24, Timestamp = 0 }
                },
                new Vehicule
                {
                    Id = 3,
                    Immatricule = "DEF-456",
                    Constructeur = "Ford",
                    Models = "Focus",
                    Coordinate = new VehiculeCoordinate { Latitude = 34.07, Longitude = -118.23, Timestamp = 0 }
                }
            };
    }
}
