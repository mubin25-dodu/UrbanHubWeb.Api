using System.ComponentModel.DataAnnotations;

namespace UrbanHub.DTO;

public class RegistrationDTO
{
    //[ValidateNever]
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}