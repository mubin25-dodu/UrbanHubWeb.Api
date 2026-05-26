using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanHub.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String From { get; set; } 
        public int To { get; set; } 

        public string? Message { get; set; }
        public string? Title { get; set; }
        public bool Seen { get; set; } = false;
        public DateTime Date { get; set; }

        [ForeignKey("To")]
        public virtual User ToUserID { get; set; }    
       

    }
}
