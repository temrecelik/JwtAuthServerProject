using AuthServer.CoreLayer.Dtos;
using AuthServer.CoreLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{

    [Route("api/[controller]")]//bir controller'da her istekten 1 adet var ise içerisi controller olabilir
    [ApiController]
    public class UserControlller : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserControlller(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        { 
            return  ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
        }


        /*
         Bu methodu authorize olduktan sonra erişilebildiği için parametre yok user name identity'den alınacak
         bu yüzden sign-ın olmadan bu method çalışmaz. UserName'e tokenda bulunan claim'lardan direkt olarak 
         erişebiliriz ve user name'i  biliyorsak o kullanıcı hakkında bütün işlemleri yaptırabiliriz. Bu nedenle
         token oluştururken claim'ların key kısmına direkt olarak bir string key vermek yerine identity kütüphanesinde
         bulunan identity'nin tanıyacağı key'ler vermek usera direkt identity'den ulaşmak açısından önemlidir.
         */
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }
    }
}
