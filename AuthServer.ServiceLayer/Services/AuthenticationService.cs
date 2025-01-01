using AuthServer.CoreLayer.Configuration;
using AuthServer.CoreLayer.Dtos;
using AuthServer.CoreLayer.Entities;
using AuthServer.CoreLayer.Repositories;
using AuthServer.CoreLayer.Services;
using AuthServer.CoreLayer.UnitOfWork;
using AuthServer.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.ServiceLayer.Services
{
    //AuthenticationController authenticationService'den beslenecek bu authenticationService ise TokenService'den beslenecek.

    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;    
        private readonly  UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;


        public AuthenticationService(IOptions<List<Client>> OptionsClients, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenRepository)
        {
            _clients = OptionsClients.Value;//Option pattern ile apsetting'den bir classWın property'lerini almak için value kullanılır.
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenRepository = userRefreshTokenRepository;
        }

        /*
        bu method ITokenService'den beslenerek sign'in olan kullanıcıya token üretir ve refresh token'ı user'ı ile birlikte
        database kaydeder.Yani endpoint yazarken tokenservice değil authentication service kullanılır.
        */
        public async Task<Response<TokenDto>> CreateAccesAndRefreshToken(LoginDto loginDto)
        {
            if(loginDto == null) { throw new ArgumentNullException(nameof(loginDto)); }

            var user = await  _userManager.FindByEmailAsync(loginDto.Email);
            //client tarafından yapılan hatalar 400'ler ile döndürülür.Kullanıcı yanlış email yada şifre girdiğinde 400 döndürülür.
            if (user == null)  return Response<TokenDto>.Fail("Email or Password is wrong", 400, true); 
            
            if(!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);

            var tokenDto = _tokenService.CrateToken(user);
            var userRefreshToken = await _userRefreshTokenRepository.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            //bir user için daha önce bir refresh token oluşturulmamışsa yeni bir refresh token oluşturulur.
            if (userRefreshToken == null)
            {
                await _userRefreshTokenRepository.AddAsync(new UserRefreshToken
                {
                    RefreshTokenCode = tokenDto.RefreshToken,
                    Expiration = tokenDto.RefreshTokenExpiration,
                    UserId = user.Id
                });
            }
            //bir user için daha önce bir refresh token oluşturulmuşsa bu token güncellenir.
            else
            {
                userRefreshToken.RefreshTokenCode = tokenDto.RefreshToken;
                userRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(tokenDto, 200);   

        }

        public Response<ClientTokenDto> CreateAccesTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.ClientId == clientLoginDto.clientId && x.ClientSecretKey ==clientLoginDto.clientSecret);
            if (client == null) 
                return Response<ClientTokenDto>.Fail("ClientId or ClientSecretKey is wrong", 404, true);

            /*client ile girilen API'ler için refresh token yoktur buradaki tokenDto'da sadece access token ve 
              experssion süresi tutulur. Refresh token oluşturulmaz.
             */
            var tokenDto = _tokenService.CreateTokenByClient(client);

            return Response<ClientTokenDto>.Success(tokenDto, 200);
        }
        
        //verilen refresh token database'de ilgili user'ın id'si ile birlikte kayıtlıdır.
        public async Task<Response<TokenDto>> CreateAccesTokenByRefreshToken(string refreshToken)
        {
            var ExistrefreshToken =await _userRefreshTokenRepository.Where(x => x.RefreshTokenCode == refreshToken).SingleOrDefaultAsync();
            
            if (ExistrefreshToken == null) 
                return Response<TokenDto>.Fail("Refresh Token not found", 404, true);

            var user = await _userManager.FindByIdAsync(ExistrefreshToken.UserId);

            if (user == null)
                return Response<TokenDto>.Fail("User not found", 404, true);

            var tokenDto = _tokenService.CrateToken(user);

            //kullanıcının  refresh token'ı veritabanında güncellenir.
            ExistrefreshToken.RefreshTokenCode = tokenDto.RefreshToken;
            ExistrefreshToken.Expiration = tokenDto.RefreshTokenExpiration;          
            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(tokenDto, 200);

        }

        //kullanıcı çıkış yaptığında refresh token'ı AuthServer'da tutulan refresh token'lar arasından silinir.
        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var ExistrefreshToken =await _userRefreshTokenRepository.Where(x => x.RefreshTokenCode == refreshToken).SingleOrDefaultAsync();
            if (ExistrefreshToken == null)
                return Response<NoDataDto>.Fail("Refresh Token not found", 404, true);

            _userRefreshTokenRepository.Remove(ExistrefreshToken);
            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(200);
        }
    }
}
