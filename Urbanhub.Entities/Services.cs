using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace UrbanHub.Entities
{
    [Table("Services")]
    public class Services
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)] public string ServiceName { get; set; } = null!;

        public bool IsAvailable { get; set; }
        public decimal UserPlatformFee { get; set; }
        public decimal OwnerPlatformFee { get; set; }
        public int? LogID { get; set; }
        [ForeignKey("LogID")]
        public virtual Log? Logs { get; set; }

    }
}
