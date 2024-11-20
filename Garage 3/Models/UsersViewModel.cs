using System.ComponentModel.DataAnnotations;

namespace Garage_3.Models
{
    public class UsersViewModel
    {
        public string Id { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Display(Name = "Number of vehicles")]
        public int Vehicles { get; set; }
        [Display(Name = "Parking fee")]
        public double ParkingFee { get; set; }
    }
}
