using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UrbanHub.ViewModels
{
    public class ParkingSpaceCreateVM
    {
        [Required]
        [StringLength(200)]
        public string Address { get; set; } = null!;

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal RentPerHour { get; set; }

        [Required]
        public string VehicleType { get; set; } = null!;

        [Required]
        public string Available { get; set; } = null!;

        public bool IsAvailable { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; } = null!;

        [Required]
        public IFormFile ImageFile { get; set; } = null!;
    }
}