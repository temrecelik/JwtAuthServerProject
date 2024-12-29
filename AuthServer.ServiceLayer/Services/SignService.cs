using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.ServiceLayer.Services
{
    
    public static class SignService
    {
        /*
        *bu method ile oluşturulacak access token'ı simetrik olarak şifreleyecek security key üretilir.Bu key
        kullanılarak token oluşturulur. Yani bu token'da header yada payload alanı bu key bilinmeden değiştirilemez.
        Eğer değiştirilirse token'ın imzası bozulur ve token geçersiz olur. Client bu imzası bozulmuş token'ı
        kullanarak API'lere istek yapamaz.  
        Biz burada token'ı simetrik olarak şifreleyeceğiz. IdentiySERVER4 ile default olarak token'ı asimetrik 
        olarak şifreler. Yani token'ı oluştururken private key kullanır ve token'ı doğrularken public key kullanır.
        
        Asimetrik şifrelemede client'tan public key alınır. Bu public key ile token doğrulanır. Ancak token private
        keye göre şifrelenir.
        */
        public static SecurityKey GetSymmetricSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
