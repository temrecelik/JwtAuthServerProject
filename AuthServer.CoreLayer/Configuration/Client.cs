using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Configuration
{
    /*
     *Client class'ında User bilgisi gerektirmeyen API'lere erişmek için kullanılacak olan clientId ve ClientSecretKey'in property'si tutuyoruz.
      Bu class üyelik sisteme gerektirmeyen API'lere erişim sağlamak için oluşturmak için kullanılan CreateTokenByClient methodunda parametre olarak alınır. 
      Ve buradaki ClientId ve ClientSecretKey kullanılarak accessToken oluşturulur.CreateTokenByClient AuthenticationSerice'de kullanılan bir method olduğu için client parametresi
      option pattern ile appsetting.json'dan doldurularak bu methoda verilir.
     */
    public class Client
    {
        public string ClientId{ get; set; }
        public string ClientSecretKey { get; set; }

     /*
       Audiences listesinde ilgili ClientId ve ClientSecretKey'e sahip token'ın hangi API'lere erişebileceği tutulur.Yani kelemi anlamı
       bu clientKey'ini hangi API'ler dinler ve end-point'lerine erişmeye izin verir anlamındadır.
        Audiences'in içeriği => wwww.myApi1.com , www.myApi2.com şeklindeki Apiler'in domain nameleri şeklindedir.
      */

        public List<string> Audiences { get; set; } 
    }
}
