using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Dtos
{
    /*
     *Bir kullanıcı kayıt olduktan sonra kullanıcıya ait bilgileri client'a dönmek için UserAppDto sınıfı oluşturuldu.
     */
    public class UserAppDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? City { get; set; }    
    }
}
