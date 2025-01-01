using AuthServer.CoreLayer.Dtos;
using AuthServer.CoreLayer.Entities;
using AuthServer.CoreLayer.Services;
using AuthServer.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.ServiceLayer.Services
{
    public class UserService : IUserService
    {
        private UserManager<UserApp> _userManager;

        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new UserApp
            {
                UserName = createUserDto.UserName,
                Email = createUserDto.Email
            };

            /*user oluştururken entity'ye gelen password girmeye gerek yok user manager yeni bir user oluşturuken 
              ikinci parametre olarak bu password'u alır veri tabanına kaydederken hash'lenmiş halini kaydeder.Veri tabanına
              password'ler asla direkt olarak kaydedilmemelidir.
             */
            var result =await _userManager.CreateAsync(user, createUserDto.Password);

            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
           var user = await _userManager.FindByNameAsync(userName);
 
            if (user == null)
            {
                return Response<UserAppDto>.Fail("User not found", 404, true);
            }

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }
    }
}
