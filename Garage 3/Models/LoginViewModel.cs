using System.ComponentModel.DataAnnotations;

namespace Garage_3.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Användarnamn är obligatoriskt.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Lösenord är obligatoriskt.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        // Nullable för att hantera tomma URL:er
        public string? ReturnUrl { get; set; }
    }
}