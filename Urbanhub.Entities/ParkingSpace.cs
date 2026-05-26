using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace UrbanHub.Entities
{
    [Table("ParkingSpace")]
    public class ParkingSpace
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Address { get; set; } = null!;

        public Point? Location { get; set; }
        public decimal RentPerHour { get; set; }
        public string Available { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public string Image { get; set; } = null!;
        [StringLength(250)]
        public string Description { get; set; } = null!;
        [StringLength(50)]
        public string VehicleType { get; set; } = null!;
        public int OwnerId { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; } = null!;
    }
}
