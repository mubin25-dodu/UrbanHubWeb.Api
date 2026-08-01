using System.ComponentModel.DataAnnotations;

namespace UrbanHub.DTO
{
    public class LoginDTO
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        // ErrorMessage = "Password must be at least 8 characters,\n include uppercase, lowercase, number and special character.")]
        public string Password { get; set; } = null!;

    }
}
