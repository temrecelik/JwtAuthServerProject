using AuthServer.CoreLayer.Configuration;
using AuthServer.CoreLayer.Dtos;
using AuthServer.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Services
{
    public interface ITokenService
    {
        TokenDto CrateToken(UserApp userApp);
        ClientTokenDto CreateTokenByClient(Client client);
    }
}
