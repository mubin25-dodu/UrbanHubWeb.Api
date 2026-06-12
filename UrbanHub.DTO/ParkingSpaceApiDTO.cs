using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using UrbanHub.Entities;

namespace UrbanHub.DTO
{
    public class ParkingSpaceApiDTO
    {
        public int ID { get; set; }

        public string Address { get; set; } = null!;

        public double lan { get; set; }
        public double lon { get; set; }
        public decimal RentPerHour { get; set; }
        public string Available { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public string Image { get; set; } = null!;
        [StringLength(250)]
        public string Description { get; set; } = null!;
        [StringLength(50)]
        public string VehicleType { get; set; } = null!;
        public int OwnerId { get; set; }
        //holding distance from user to parking space, not stored in database
        public double Distance { get; set; } = 0.00;

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; } = null!;
    }
}
