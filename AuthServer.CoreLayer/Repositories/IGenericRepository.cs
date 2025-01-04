using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Repositories
{
    /*
     * Önemli Not 
     Eğer bir generic service olutşruduğumuzda bu generic repository içinde geçerlidir.herhangi bir entity için 
     ekstra bir service methodu eklemeyecekse IProductService ve ProductService gibi bir çok service oluşturmak yerine direkt
     olarak controller'da IGenericService<Product,ProductDto> şeklinde bir tanımlama yaparak bu service'i kullanabiliriz.Ama 
     bazı entity'ler için generic repository'de olmayan methodlarda yazılabilir. Bunun için ekstra olarak IProductService ve
     ProductService gibi service'lerİ oluşturup IGenericService'i implemente edebiliriz.
     */
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();

        /*
         Where methodu ile linq ile sorgu yazıldıktan sonra bu sorguyu karşılayan Entity'ler IQueryable olarak döner ancak bu list değildir. 
         List olması için dönen veriyi tolist() methodu ile list'ye çevrilmelidir.Bu bize dönen verinin üzerine ekstra sorgu da yazarak
         veri tabanında veri alabilmemize olanak sağlar.Sorgu yapılmadan veri alınmak istenirse INumerable dönüp
         tekrar bu IeNumerable'ı Tolist() çevirebiliriz. Where methoduna parametre olarak bir delege verilir örnek TEntity bir user ise
         (x => x.age > 25 ) bu koşulu sağlayan user'lar veritabanında alınarak IQueryable koleksiyonu şeklinde geri döndürülür.
        */
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        Task AddAsync(TEntity entity);
        /*
        * EntityFramework kullanarak bir entity silindiğinde savechange işlemi yapana kadar entity'nin bellekte state durumu deleted olarak
          işratlenir. Bu nedenle EntityFramework'ün remove methodu Task değildir.Update işleminde de state updated'e çekilir.
        */
        void Remove(TEntity entity);
        TEntity Update(TEntity entity);
    }
}
