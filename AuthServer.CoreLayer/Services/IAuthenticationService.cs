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
         client tarafına döndürülür.
         */
        Task<Response<TokenDto>> CreateAccesAndRefreshToken(LoginDto loginDto);

        /*
         * Eğer kullanıcı refresh tokenın ömrü bitmeden belirli periyotlarda uygulamayı açıyorsa access tokenın ömrü bitmiş olabilir.
           Bu durumda ilgili API AuthServer'a bir hata kodu döner ancak client tarafı bundan etkilenmez.Bu hata ile birlikte ömrü tükenmemiş
           olan RefreshToken'ı alıp doğrulayan AuthServer ilgili  kullanıcı için yeni bir access token ve refresh token oluşturur ve client'a 
           gönderir.Bu sayede kullanıcı çıkış yaptırılmadan access token'ı ve refresh token'ı yenilenmiş olur.Aşağıda method bu durum için
           kullanılır.
         */
        Task<Response<TokenDto>> CreateAccesTokenByRefreshToken(string refreshToken);

        /*
         Kullanıcı çıkış yaptığında refresh token'ı AuthServer'da tutulan refresh token'lar arasından silinir.Bu method ile hem client'ın
         local storage'ından refresh token silinir hemde AuthServer'da refresh token silinir.
         */
        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);

        /*Üyelik sistemi bulunmayan API'lere client tarafından erişmek için kullanılacak access token bu method ile oluşturulur.Üyelik
          sistemi gerektirmeyen API'lerde refretoken oluşturulmaz ve kullanıcıya gönderilmez. Sadece access token oluşturulur. Client
          eğer access token'ın ömrü dolarsa elinde clientID ve clientSecret ile AuthServer'a istekte bulunarak yeni bir access token alır.
         */
        Response<ClientTokenDto> CreateAccesTokenByClient(ClientLoginDto clientLoginDto);
    }
}
