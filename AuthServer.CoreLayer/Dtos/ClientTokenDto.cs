using Microsoft.AspNetCore.Authentication.BearerToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Dtos
{
    /*
    Bazı API'lerde user bilgisi olmasına gerek olmaz örneğin hava durumunu yayınlayan bir end-point'i düşünelim. Bu durumlarda client tarafında 
    bir clientID ve client secretKey oluşturulur. Bu clientKey ve client secret Key aynı zamanda AuthServer'da tutulur. Client'tan gelen ıd ve 
    secretkey ile doğrulama yapılırsa AuthServer Client'a access token gönderir. Bu tokenda herhangi bir user ile ilgili bilgi tutulmaz.
    Gönderilen bu access token için  ClientTokenDto oluşturulmuştur. Yani client tarafına dönülecek token clientTokenDto olarak dönülüyor bu 
    dto'nun property'sinde tutulur.Client aldığı bu token ile ilgili API'ye erişebilir.
     */

    public class ClientTokenDto
    {
        public string AccessToken { get; set; }
        public string AccessTokenExpiration { get; set; }   

    }
}
