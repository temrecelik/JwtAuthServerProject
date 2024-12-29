using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Dtos
{
    /*
     *Token dto'da access token, access token'ın ömrü refresh token ve refresh token'ın ömrü tutulur. Access tokenın ömrü zaten token'ın 
      payload alanında bir claim olarak tutulur ancak kod içinde token'ın ömrüne kolayca erişmek için dto'nın içinde de tutarız.

    *Bu dto'yu client'a tokenları gönderebilmek için oluşturduk birde entity olarak refresh token'ı oluşturmuştuk çünkü refresh token aynı
    *zamanda userID ve ömrü ile beraber AuthServer'ın bağlı olduğu veri tabanında kayıtlı olmalıdır.
     */
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public string RefreshToken {  get; set; }
        public DateTime RefreshTokenExpiration { get; set; }    
    }
}
