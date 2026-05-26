using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanHub.Entities
{
    public class Email
    {
        public int Id { get; set; }

        public string MailTo { get; set; } = null!;

        public string Body { get; set; } = null!;

        public DateTime Date { get; set; }

        public int LogId { get; set; }
    }
}
