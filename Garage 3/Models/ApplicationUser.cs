using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Garage_3.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required] // Om denna egenskap ska ha ett värde
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty; // Sätt default till en tom sträng

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty; // Sätt default till en tom sträng

        // Om SSN kan vara null, gör följande
        [Required] // Om SSN alltid ska anges
        [Display(Name = "Social Security Number (SSN)")]
        public long SSN { get; set; }
    }
}