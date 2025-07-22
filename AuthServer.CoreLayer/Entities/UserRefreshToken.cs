using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Entities
{
    /*
    Refresh token veri tabanında UserId ve ömrü ile beraber tutulmalıdır.
   */
    public class UserRefreshToken
    {
        public string UserId { get; set; } 
        public string RefreshTokenCode { get; set; }
        public DateTime Expiration { get; set; }
    }
}
