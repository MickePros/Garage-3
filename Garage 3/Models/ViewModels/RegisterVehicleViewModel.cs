using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Garage_3.Models.ViewModels
{
    public class RegisterVehicleViewModel
    {
        [Display(Name = "License plate")]

        [Remote("CheckRegNr", "Vehicles")]
        public string RegNr { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        [Range(0, 18)]
        public int Wheels { get; set; }

        [Display(Name = "Vehicle type")]
        public string VehicleType { get; set; }
    }
}
