using AuthServer.CoreLayer.Configuration;
using AuthServer.CoreLayer.Dtos;
using AuthServer.CoreLayer.Entities;
using AuthServer.CoreLayer.Services;
using AuthServer.Shared.Configurations;
using AuthServer.Shared.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.ServiceLayer.Services
{
    /*
     TokenService controller'da kullanılmaz.Burada createAccesToken  ve CreateRefresh Token methodları yazılır. AuthenticationService TokenService'den nesne üreterek bu methodları
     kullanılır.Ve dönen tokenları tokenDto classında property'leri atarak client'a döner.
     */

    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly CustomTokenOption _tokenOption;//Property'ler appsetting.json'dan optionpattern ile doldurulacak.

        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOption> tokenOption)
        {
            _userManager = userManager;
            _tokenOption = tokenOption.Value;
        }

        /*Bu method ile RefreshToken üretiriz.Üretilen refresh token unique bir değerdir.32 bitlik unique
          bir değer üretiriz.*/

        private  static string CreateRefreshToken()
        {
            var numberByte = new Byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        /*
        *GetClaims Methodu accessToken üretirken token'ın payload bölümünde bulunacak key-value çiftlerini yapılandırmak için kullanılır.
        
        *AccessToken payload'ında olacak claim'ları hazırlarken bu claim'ların key'i sabit olmalıdır.Çünkü kendimiz bu cliamlara key yazarsak identiy kütüphanesi bu token'daki 
         claimların neye karşılık geldiğini algılamaz. Ve girişi yapan kullanıcının access tokenındaki ıd değerini ve name değerini veri tabanında arayamaz. Bunun için key değeri 
         claims classından olşturulanyada JwtRegisteredClaimNames classından oluşturulan sabit stringler olmalıdır. Kendimiz string verirsek User'ın tüm cliam'larını gerek ilgili 
         claim bulmak zorunda kalırız.
        */ 
       
        private IEnumerable<Claim> GetClaims(UserApp userApp ,List<String> audiences)
        {
            var claims = new List<Claim>();
          
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new Claim(JwtRegisteredClaimNames.Email,userApp.Email!),
                new Claim(ClaimTypes.Name,userApp.UserName!),

                //bu claim ike ile access token'a bir Id verilir. Kullanılmasada olabilir.
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            claims.AddRange(userList);
            claims.AddRange(audiences.Select(audiences => new Claim(JwtRegisteredClaimNames.Aud, audiences)));

            return claims;
        }
            

        /*
         * üyelik istemeyen client'lar için oluşturualacak accesstoken'ın claimlarında clientsecretkey tutulmaz
           bu key gizlidir.Token'ı ele geçeren birisi paylaod'da tutulan bu claim'arı encoded ederek çözümleyebilir.
         */
        private IEnumerable<Claim> GetClaimByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud,x)));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, client.ClientId.ToString()));
           
            return claims;
        }


        public TokenDto CreateToken(UserApp userApp)
        {
         
             var AccessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);

            //securityKey kullanılarak token'ın imzası oluşturulur.Tek bir key olduğu için bu imza symmetric olarak oluşturulur.
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecretKey);

            //Security key'e göre token için bir imza üretiyor.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            
            //imza ile token'ın içeriği hazırlanıyor.
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                expires: AccessTokenExpiration,
                notBefore: DateTime.Now, //token'ın ömrünün başlama anı
                claims: GetClaims(userApp, _tokenOption.Audience),
                signingCredentials: signingCredentials
                );

            //accestokenı hazırlayan sınıf  JwtSecurityTokenHandle sınıfıdır.
            var handler = new JwtSecurityTokenHandler();
            var accesstoken = handler.WriteToken(jwtSecurityToken);
           
            var TokenDto = new TokenDto
            {
                AccessToken = accesstoken,
                AccessTokenExpiration = AccessTokenExpiration,
                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration)
            };

            return TokenDto;
        }

        public ClientTokenDto CreateTokenByClient(Client client)//property değerleri appsettings.json dosyasından alınır.
        {
            var accesTokenExpiration = DateTime.Now.AddMinutes( _tokenOption.AccessTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecretKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

             /*bu token'da dinleyiciler olmaz clientKey ile client ilgili api'ye erişir.Ve oradan user ile ilgili bir bilgi
             olmadan api'ye erişir*/
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                expires: accesTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaimByClient(client),
                signingCredentials: signingCredentials
                );
            var handler = new JwtSecurityTokenHandler();
            var accessToken = handler.WriteToken(jwtSecurityToken);

            var clientTokenDto = new ClientTokenDto
            {
                AccessToken = accessToken,
                AccessTokenExpiration = accesTokenExpiration,
            };
            return clientTokenDto;
        }        
    }
}
