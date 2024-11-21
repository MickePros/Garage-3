using System.ComponentModel.DataAnnotations;

namespace Garage_3.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Användarnamn är obligatoriskt.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-post är obligatoriskt.")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Förnamn är obligatoriskt.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Efternamn är obligatoriskt.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Lösenord är obligatoriskt.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bekräfta lösenordet.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Lösenorden matchar inte.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Personnummer är obligatoriskt.")]
        public string SSN { get; set; }
    }
}