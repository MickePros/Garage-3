using System.ComponentModel.DataAnnotations;

namespace Garage_3.Models.ViewModels
{
    public class ParkingSpotsOverviewViewModel
    {
        [Key]
            public int ParkingSpotId { get; set; }
            public string RegNr { get; set; }
 
            public bool IsOccupied { get; set; }

            public string VehicleOwner { get; set; }
            public string VehicleType { get; set; }
        
    }
}

//ParkingSpotVehicle
//Add new
//Update existing
//Change status
//Mark with color all spots occupied by the same vehicle
// Text in first one