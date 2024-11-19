namespace Garage_3.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }
        public int Status { get; set; }

        // Navigational Property
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
