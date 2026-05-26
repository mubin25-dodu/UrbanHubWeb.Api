using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanHub.Entities
{
    [Table("PlatformWallet")]
    public class PlatformWallet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UID { get; set; }

        public decimal? AddMoney { get; set; } 
        public decimal PlatformFee { get; set; } 
        public decimal? WithdrawMoney { get; set; }
        [ForeignKey("UID")]
        public virtual User User { get; set; }

    }
}
