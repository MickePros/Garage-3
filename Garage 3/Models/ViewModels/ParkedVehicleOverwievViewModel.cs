using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Garage_3.Models.ViewModels
{
    public class ParkedVehicleOverwievViewModel
    {
        public string Owner { get; set; }

        public string Membership {  get; set; }

        public string Type { get; set; }
        public string RegNr { get; set; }

        public int ParkingSpotId { get; set; }

        [ReadOnly(true)]
        public DateTime Arrival { get; private set; }

        [ReadOnly(true)]
        public TimeSpan ParkLenght => DateTime.Now - Arrival;

        [Display(Name = "Time parked")]
        public string ParkLengthFormatted
        {
            get
            {
                var days = ParkLenght.Days;
                var hours = ParkLenght.Hours;
                var minutes = ParkLenght.Minutes;

                //Returns a string with only values greater than 0 
                return days > 0
                    ? $"{days}days {hours}h {minutes}min"
                    : hours > 0
                        ? $"{hours}h {minutes}min"
                        : $"{minutes} minutes";
            }
        }
    }
}
