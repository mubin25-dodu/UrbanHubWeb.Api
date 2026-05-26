using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UrbanHub.shared
{
    public class UserCard(IHttpContextAccessor accessor)
    {
        public bool IsAuthendicated {
            get
            {
                try
                {
                    return (bool)accessor.HttpContext?.User?.Identity?.IsAuthenticated;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            set {}
        }

        public int UserId
        {
            get
            {
                var ID= accessor.HttpContext?.User?.FindFirst("UserID")?.Value;
                return int.Parse(ID)!=null? int.Parse(ID) : 0;
            }
            set { }
        }
       
    }
}
