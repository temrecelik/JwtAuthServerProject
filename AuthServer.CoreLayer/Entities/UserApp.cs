using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Entities
{
/*
   Identity kütüphanesi ile üyelik sistemi inşa ederken dbcontext'te bizden appuser ve approle entity'leri ister bu entity'lere göre üyelik için gerekli tüm tabloları
   kendisi oluşturur.
*/
    public class UserApp : IdentityUser<string>
    {
        public string? city { get; set; }
    }
}
