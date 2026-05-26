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
    [Table("Withdrawals")]
    public class Withdrawal{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        [StringLength(50)]
        public string Method { get; set; }=null!;
        [StringLength(250)]
        public string AccountInfo { get; set; }=null!;
        [StringLength(50)]
        public string Status{ get; set; }=null!;
        public decimal Amount { get; set; }
        public DateTime Date{ get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; } = null!;
    }
}
