using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Shared.Configurations
{

    /*
     * bu classtaki property'ler oluşturulacak access tokenın özelliklerini belirleyen claim'ların key olarak
       model almak için oluşturduk.Bu claim'lerin value'ları tokenı dağıtacak olan servisin yani 
       AuthServer.API'deki appsettings.json dosyasındaki değerleri alır.Yani birbirlerine bağlanacaklar.
       Bağlam işlemş AuthServer.API'de program.cs dosyasında yapılmıştır.
     */
    public class CustomTokenOption
    {
        public List<string> Audience { get; set; }
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
    }
}
