using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanHub.Entities
{
    [Table("Wallet")]
    public class Wallet
    {
        [Key , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; } 
        public decimal Amount { get; set; } 
        public bool Status { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; } 
    }
}
