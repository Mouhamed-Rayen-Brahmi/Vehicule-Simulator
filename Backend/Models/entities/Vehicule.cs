namespace Backend.Models.entities
{
    public class Vehicule
    {
        public int Id { get; set; }
        public string Immatricule { get; set; } = string.Empty;
        public string Models { get; set; } = string.Empty;
        public string Constructeur { get; set; } = string.Empty;
        public VehiculeCoordinate Coordinate { get; set; } = new VehiculeCoordinate();
    }

    public class VehiculeCoordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long Timestamp { get; set; }
    }
}
