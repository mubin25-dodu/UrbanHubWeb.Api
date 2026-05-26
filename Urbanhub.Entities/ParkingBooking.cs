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
    [Table("ParkinBooking")]
    public class ParkingBooking
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int? ParkingID { get; set; }
        public int? OwnerID { get; set; }
        public DateTime StartingTime { get; set; } 
        public DateTime EndingTime { get; set; }
        public int RenterID { get; set; }
        public int? ReviewID { get; set; }
        [StringLength(50)]
        public string Status { get; set; } = null!;
        public bool Withdrawn { get; set; } 
        public int? OTP { get; set; } 
        public decimal PaymentAmount { get; set; } 
        public decimal? TotalBill { get; set; } 
        [StringLength(50)]
        public string? PaymentDetails { get; set; } = null!;
        [StringLength(50)]
        public string PaymentStatus { get; set; } = null!;
        public DateTime Date { get; set; }
        
        [ForeignKey("OwnerID")]
        public virtual User Owner { get; set; } = null!;

        [ForeignKey("ParkingID")]
        public virtual ParkingSpace Parking { get; set; } = null!;

        [ForeignKey("RenterID")]
        public virtual User Renter { get; set; } = null!;

        //[ForeignKey("ReviewID")]
        //public virtual Review Review { get; set; } = null!;
    }
}
