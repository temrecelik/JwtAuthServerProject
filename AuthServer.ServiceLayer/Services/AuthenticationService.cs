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
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;    
        private readonly  UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;

        public AuthenticationService(IOptions<List<Client>> OptionsClients, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRepository)
        {
            _clients = OptionsClients.Value;//Option pattern ile apsetting'den bir classWın property'lerini almak için value kullanılır.
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        /*
        bu method ITokenService'den beslenerek sign'in olan kullanıcıya token üretir ve refresh token'ı user'ı ile birlikte
        database kaydeder.
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

        public Task<Response<ClientTokenDto>> CreateAccesTokenByClient(ClientLoginDto clientLoginDto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<TokenDto>> CreateAccesTokenByRefreshToken(string refreshTokenDto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<NoDataDto>> RevokeRefreshToken(string refreshTokenDto)
        {
            throw new NotImplementedException();
        }
    }
}
