using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Entities
{
    /*
    AuthServer Client tarafına göndereceği refresh token'ı aynı zamanda veri tabanındada hangi kullanıcıya ait olduğu ve token ömrü ile
    beraber tutmalıdır. By neden UserRefreshToken adında bir entity olmalıdır.Refresh token için ekstra bir Id tutmaya gerek yoktur.
   */
    public class UserRefreshToken
    {
        public string UserId { get; set; }
        public string RefreshTokenCode { get; set; }
        public string Expiration { get; set; }
    }
}
