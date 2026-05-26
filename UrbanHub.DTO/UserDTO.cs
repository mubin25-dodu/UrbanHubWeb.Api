using System.ComponentModel.DataAnnotations;

namespace UrbanHub.DTO;

public class UserDTO
{
    [Required]
    public string Name { get; set; } = null!;
    [EmailAddress]
    [Required]

    public string Email { get; set; } = null!;
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters,\n include uppercase, lowercase, number and special character.")]
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    [Required]

    public string Address { get; set; } = null!;

    public DateTime JoinDate { get; set; }
    [Required]
    [RegularExpression(@"^(?:\+8801|01)[3-9]\d{8}$",
        ErrorMessage = "Invalid Bangladeshi phone number")]
    public string Phone { get; set; } = null!;

}