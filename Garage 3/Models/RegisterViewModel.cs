using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Garage_3.Models
{
    public class RegisterViewModel : IValidatableObject
    {
        public InputModel Input { get; set; } = new InputModel();
        public string? ReturnUrl { get; set; }
        public IList<ExternalLogin> ExternalLogins { get; set; } = new List<ExternalLogin>();

        public class InputModel
        {
            [Required(ErrorMessage = "Användarnamn är obligatoriskt.")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "E-post är obligatoriskt.")]
            [EmailAddress(ErrorMessage = "Ogiltig e-postadress.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Lösenord är obligatoriskt.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "Bekräfta lösenordet.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Lösenorden matchar inte.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Förnamn är obligatoriskt.")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Efternamn är obligatoriskt.")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Personnummer är obligatoriskt.")]
            public string SSN { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validera att Förnamn och Efternamn inte är lika
            if (!string.IsNullOrEmpty(Input.FirstName) && !string.IsNullOrEmpty(Input.LastName) &&
                string.Equals(Input.FirstName, Input.LastName, StringComparison.OrdinalIgnoreCase))
            {
                yield return new ValidationResult(
                    "Förnamn och efternamn kan inte vara lika.",
                    new[] { nameof(Input.FirstName), nameof(Input.LastName) });
            }

            // Exempel på kontroll om personnummer är registrerat
            // Kan här anpassa med faktisk logik för att kolla i db, till exempel.
            if (Input.SSN == "1234567890")  // Detta är ett exempel på ett "fiktivt" registrerat SSN.
            {
                yield return new ValidationResult(
                    "Det angivna personnumret är redan registrerat.",
                    new[] { nameof(Input.SSN) });
            }
        }
    }

    // Klasser för att hantera externa inloggningar (om det används)
    public class ExternalLogin
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
