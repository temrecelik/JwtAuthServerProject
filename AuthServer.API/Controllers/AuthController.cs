using AuthServer.CoreLayer.Dtos;
using AuthServer.CoreLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[action]")] //bir controller'da bir istekten fazla var ise içerisi action olmalıdır method nameni bakar.
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await _authenticationService.CreateAccesAndRefreshToken(loginDto);
            return ActionResultInstance(result);
        }


        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result =  _authenticationService.CreateAccesTokenByClient(clientLoginDto);
            return ActionResultInstance(result);      
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.RefreshTokenCode);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.CreateAccesTokenByRefreshToken(refreshTokenDto.RefreshTokenCode);
            return ActionResultInstance(result);
        }
    }
}
