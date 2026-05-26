using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanHubManagement.repo
{
    public class Result<Table>
    {
        public Table? Data { get; set; }
        public bool Error { get; set; }
        public string? Message { get; set; }

    }
}
