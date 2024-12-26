using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Dtos
{
    /// <summary>
    /// Bir user eklemek istersek client tarafında CreateUserDto alınır ve UserApp ile bu dto map'lenerek oluşturulan userApp entity'si
    /// veri tabanına kaydedilir.
    /// </summary>
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
