using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Entities
{
    /*
      User'lar için identity ile üyelik sistemini oluşturulacak. İdentityUser classında user için default property'ler tanımlıdır. IdentityUser
      classında go to implementation diyerek bir user için barındırdığı property'leri görebilirsin. Ekstra property'de UserApp entity'si
      içinde eklenebilir. İdentity'de default olarak tanımlanan property'ler de silinebilir ya da veri tipi değiştirilebilir.
    */
    public class UserApp : IdentityUser
    {
        public string city { get; set; }


    }
}
