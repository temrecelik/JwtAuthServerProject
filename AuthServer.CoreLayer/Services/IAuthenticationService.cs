using AuthServer.CoreLayer.Dtos;
using AuthServer.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Services
{
    public interface IAuthenticationService
    {
        /*
         *User giriş yaptıktan sonra işlem başarılı ise AccessToken ve RefreshToken oluşturulur. Ve bu tokenlar TokenDto'ya eklenir.Ve 
         client tarafına döndürülür.Yani bu methodda signanager ile loginDto kullanılarak sisteme giriş yapılır ve ilgili kullanıcıya göre
         accessToken ve refresToken üretilir. Üretiken Token'lar end-point'te client'a TokenDto nesnesi olarak gönderilir.
         */
        Task<Response<TokenDto>> CreateAccesAndRefreshToken(LoginDto loginDto);
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /*
         * Eğer kullanıcı refresh tokenın ömrü bitmeden belirli periyotlarda uygulamayı açıyorsa access tokenın ömrü bitmiş olabilir.Bu durumda
           client accessToken'ı API'ya gönderdiğinde API bu token'ın ömrünün bitmiş olduğuna anlar ve client API'ye erişemez ve API client'a
           401 kodu döner.Kodu alan client locastorega'ında bulunan refreshToken'ı AuthServer'a gönderir ve bu token kontrol edildikten sonra
           ilgili kullanıcı için yeni refreshToken ve accessToken oluşturulur. Bu method işlevi görmektedir.Refresh Token userıd ile bulunarak 
           parametre olarak verilir.Oluşturulan tokenlar TokenDto dto'suna eklenerek client'a gönderilir.
          
         */
        Task<Response<TokenDto>> CreateAccesTokenByRefreshToken(string refreshToken);
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        /*
           Logout işleminden sonra kullanıcının refresh token'ı veritabanından veri localstorage'dan silinmelidir. RevokeRefreshToken methodu ile
           bu işlem yapılır. Silinecek refreshtoken parametre olarak vermek için usermanager'dan user'ın id'sine oradanda refreshToken tablosundan user'ın refreshtokenına erişilerek 
           verilir.
         */
        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
       
        /*
         Eğer kullanıcı üyelik gerektirmeyen bir API erişmek istersek bu method çalışır. ClientLoginDto'da option pattern ile ClientKey ve ClienId appsetting.json'dan 
         alınır ve İlgili accesstoken üretilir bu token ClientTokenDto nesnesine property olarak verilere client'a döndürülür.
         */
        Response<ClientTokenDto> CreateAccesTokenByClient(ClientLoginDto clientLoginDto);

 //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    }
}
