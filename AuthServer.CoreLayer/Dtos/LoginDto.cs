using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Dtos
{
    /*
     * Authentication işlemlerinde kullanılacak olan LoginDto sınıfı oluşturuldu.
     */
    public class LoginDto
    {
        public string Email { get; set; }   
        public string Password { get; set; }    
    }
}
