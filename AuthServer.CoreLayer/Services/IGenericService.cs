using AuthServer.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Services
{
    /*
   *Busines kodlarının interface buradadır. End-pointler bu methodlardan beslenecektir. Bu nedenle hepsinin dönüş tipi Response olarak
    oluşturuldu. Ayrıca burada mapping yapılarak veri tabanından alınan entity'ler dto'ya çevrilerek kullanılacaktır.
   */
    public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<Response<TDto>> GetByIdAsync(int id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        /*
         *Burada artık tüm business kodu yazıldığı için tekrar olarka IQueryabla olarak aranan entity'leri dönmeye gerek yoktur.Daha fazla
          sorgu olmayacağı için IEnumerabla olarak dönülebilir.
        */
         Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate); 

        Task<Response<TDto>> AddAsync(TDto dto);

        //bir kayıt silindiğinde silinen kayıdı döndürmeye gerek yoktur bu nedenle response'ın data kısmına boş bir dto gönderebiliriz.
        Task<Response<NoDataDto>> Remove(int Id);

        Task<Response<NoDataDto>> Update(TDto dto,int Id);
    }
}
