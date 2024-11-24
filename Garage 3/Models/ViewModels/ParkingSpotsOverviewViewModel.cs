using System.ComponentModel.DataAnnotations;

namespace Garage_3.Models.ViewModels
{
    public class ParkingSpotsOverviewViewModel
    {
        [Display(Name = "Parking spot")]
            public int ParkingSpotId { get; set; }
        [Display(Name = "Registration number")]
        public string RegNr { get; set; }
        [Display(Name = "Status")]
        public bool IsOccupied { get; set; }
        [Display(Name = "Vehicle owner")]
        public string VehicleOwner { get; set; }
        [Display(Name = "Vehicle type")]
        public string VehicleType { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<ParkingSpot> ParkingSpots { get; set;} = new List<ParkingSpot>();
        
    }
}

//ParkingSpotVehicle
//Add new
//Update existing
//Change status
//Mark with color all spots occupied by the same vehicle
// Text in first one