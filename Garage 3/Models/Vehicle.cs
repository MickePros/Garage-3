using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garage_3.Models
{
    public class Vehicle
    {
        [Key]
        [Display(Name = "License plate")]
        [Remote("CheckRegNr", "Vehicles")]
        public string RegNr { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int Wheels { get; set; }
        [Display(Name = "Registration date")]

        public DateTime Registration { get; private set; }
        [Display(Name = "Parking date")]
        public DateTime? Arrival { get; set; }

        // Foreign Keys
        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }
        [Display(Name = "Vehicle type")]
        public VehicleType VehicleType { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        //Used for displaying full name in Index
        [Display(Name = "Full name")]
        public ApplicationUser ApplicationUser { get; set; }

        // Navigational Property
        public ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();

        public Vehicle()
        {
            Registration = DateTime.Now;
        }
    }
}
