using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Dtos
{
    /*
     User bilgisi gerektermiyen API'ler her clienttan gelen isteği yapmaması için AccessToken'da ClientId ve ClientSecret tutulur.
     daha sonra bu bilgiler AuthServer'da da tutulur. Önce client bu bilgileri AuthServer'a gönderir ve AuthServer bu bilgileri kontrol eder.
     Eğer doğruysa AccessToken verir.Bu accessToken'da user bilgisi bulunmaz.
     */
    public class ClientLoginDto
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }
    }
}
