using AuthServer.CoreLayer.Dtos;
using AuthServer.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Services
{
    public interface IUserService
    {
        /// <summary>
        ///  Client'tan alınan createUserDto map'lenerek userApp entity'si oluşturulur ve veri tabanına kaydedilir.Daha sonra bu entity tekrar
        ///  UserAppDto'ya map'lenerek client'a döner.
        /// </summary>
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);    

        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
    }
}
