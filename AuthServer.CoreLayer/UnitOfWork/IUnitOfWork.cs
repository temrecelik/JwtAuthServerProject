using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.UnitOfWork
{
   public interface IUnitOfWork
    {
        /*
         *CommitAsync metodu ile veri tabanına asenkron olarak kayıt işlemi yapılır.Savechange concrete classında savechange metodu
         *kullanılacağı için burada isim karışıklığı olmaması için CommitAsync olarak isimlendirildi.
         */
        Task CommitAsync();
        void Commit();
    }
}
