using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Configuration
{
    /*
     *Client class'ında User bilgisi gerektirmen API'lere erişmek için kullanılacak olan clientId ve ClientSecretKey'in property'si tutuyoruz.
      bu client class'ı entity ya da Dto değildir.Yani client'a tarafına gönderilmez.Back-end tarafında kullanılmak için hazırlanmıştır
      ClientId'yi username clientSecretKey'de password gibi düşünülür bu nedenle clientId  string tutulması daha mantıklıdır.
      İsimlendirmeler protokolden gelen isimlendirmedir.
     */
    public class Client
    {
        public string ClientId{ get; set; }
        public string ClientSecretKey { get; set; }


        /*Audiences listesinde ilgili ClientId ve ClientSecretKey'e sahip tokenın hangi API'lere erişebileceiği tutulur.Yani keleme anlamı
        bu clientKey'ini hangi API'ler dinler ve end-point'lerine erişmeye izin verir anlamındadır.
        Audiences'in içeriği => wwww.myApi1.com , www.myApi2.com şeklindeki Apiler'in domain nameleri şeklindedir.
         */

        public List<string> Audiences { get; set; } 
    }
}
