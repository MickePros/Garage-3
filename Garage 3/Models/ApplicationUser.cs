using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Garage_3.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        public long SSN { get; set; }

        // Navigational Property
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
